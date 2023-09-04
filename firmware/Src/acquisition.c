/* Includes ------------------------------------------------------------------*/
#include "main.h"
#include "acquisition.h"
#include "stm32f1xx_hal.h"
#include "usb_device.h"
#include "ADXL.h"
#include "usbd_cdc_if.h"
#include "fifo_functions.h"

/* Private define ------------------------------------------------------------*/
#define START_TIMER(x, duration)  (x = (uwTick + duration))
#define TIMER_ELAPSED(x)  ((uwTick >= x) ? 1 : 0)
#define SET_TIMESTAMP(x)  (x = uwTick)

// TIM2 is at APB1 bus, freq is 72 MHz
#define ACQUISITION_TIM_OBJ             htim2

#define ACQUISITION_TIM_FREQ            (50e3)

#define ACQUISITION_TIM_PRESC           (72e6 / ACQUISITION_TIM_FREQ)

/// Size in bytes
#define ACQUISITION_FIFO_SIZE           (2000)
#define ACQUISITION_TX_BUF_SIZE         (700)

#define ACQUISITION_SEND_PERIOD_MS      (5)

//Number of turns to calculate speed
#define ACQUISITION_SPEED_CALC_CNT      (10)

// Packed to bytes
#pragma pack(push)
#pragma pack(1)
typedef struct 
{
    uint8_t header1; //0xAA
    uint8_t header2; //0x55
    uint8_t type;
    uint16_t time;
    int16_t value_x;
    int16_t value_y;
} tx_item_t;
#pragma pack(pop)



/* Private variables ---------------------------------------------------------*/
extern TIM_HandleTypeDef ACQUISITION_TIM_OBJ;

uint8_t acquisition_init_ok = 0;
volatile uint8_t acquisition_irq_flag = 0;

fifo_struct_t acquisition_fifo;
ADXL_InitTypeDef acc_sensor;

uint8_t acquisition_fifo_buf[ACQUISITION_FIFO_SIZE];
uint8_t acquisition_tx_buf[ACQUISITION_TX_BUF_SIZE];

uint32_t acquisition_sending_timer;

/// Time when accelerometer value was captured
volatile uint16_t acquisition_last_time = 0;

volatile uint32_t acquisition_speed_time_counter = 0;
volatile uint8_t acquisition_speed_counter = 0;

/// Used for zero sensor signal filtering, part of one turn period
uint16_t acquisition_speed_threshold = 10;

/// Counter of accelerometer events (measurements)
volatile uint32_t acquisition_acc_events_cnt = 0;

/// Timestamp for measuring accelerometer data rate
uint32_t acquisition_acc_speed_timestamp_ms = 0;

/// Measured accelerometer data rate
uint16_t acquisition_acc_data_rate_hz = 3600;

//For debug only
volatile uint16_t tmp_buf[10];

extern __IO uint32_t uwTick;

/* Private function prototypes -----------------------------------------------*/

void acquisition_timer_init(void);
void acquisition_generate_speed_data(uint16_t time_val);

//*************************************************************


/// Called from Accelerometer EXTI IRQ
void acquisition_acc_irq(void)
{
    acquisition_last_time = __HAL_TIM_GET_COUNTER(&ACQUISITION_TIM_OBJ);
    acquisition_irq_flag = 1;
    acquisition_acc_events_cnt++;
}

/// Called from Zero Sensor EXTI IRQ
void acquisition_zero_sens_irq(void)
{
    //reset counter
    volatile uint16_t curr_val = __HAL_TIM_GET_COUNTER(&ACQUISITION_TIM_OBJ);
    if (curr_val < acquisition_speed_threshold) //filter
        return;
    __HAL_TIM_SET_COUNTER(&ACQUISITION_TIM_OBJ, 0);
    if (acquisition_speed_counter <= 9)
        tmp_buf[acquisition_speed_counter] = curr_val;
    acquisition_speed_time_counter += curr_val;
    acquisition_speed_counter++;

}

void acquisition_periodic_handler(void)
{
    if (acquisition_irq_flag)
    {
        acquisition_irq_flag = 0;
        acquisition_read_acc();
    }
    
    if (acquisition_speed_counter >= ACQUISITION_SPEED_CALC_CNT)
    {
        uint32_t save;
        ENTER_CRITICAL(save);
        //Time is in ticks of HW timer
        uint32_t tmp_time = acquisition_speed_time_counter;
        uint8_t tmp_count = acquisition_speed_counter;
        
        acquisition_speed_counter = 0;
        acquisition_speed_time_counter = 0;
        LEAVE_CRITICAL(save);
        
        tmp_time = tmp_time / tmp_count; //averaging
        
        acquisition_speed_threshold = tmp_time / 10;//10%
        if (acquisition_speed_threshold < 5)
        {
            acquisition_speed_threshold = 5;
        }
        
        acquisition_generate_speed_data(tmp_time);
    }
    
    
    //Measuring accelerometer data rate
    uint32_t diff_ms = uwTick - acquisition_acc_speed_timestamp_ms;
    if (diff_ms >= 3000)
    {
        //one second
        uint32_t tmp_cnt = acquisition_acc_events_cnt;
        acquisition_acc_data_rate_hz = tmp_cnt * diff_ms / (3000 * 3);//normalize to 1 second
        
        acquisition_acc_speed_timestamp_ms = uwTick;
        uint32_t save;
        ENTER_CRITICAL(save);
        acquisition_acc_events_cnt = 0;
        LEAVE_CRITICAL(save);
    }
    
    
    //Send data from TX FIFO
    if (TIMER_ELAPSED(acquisition_sending_timer))
    {
        START_TIMER(acquisition_sending_timer, ACQUISITION_SEND_PERIOD_MS);
        
        uint16_t tx_size = fifo_get_count(&acquisition_fifo);
        if (tx_size >= ACQUISITION_TX_BUF_SIZE)
            tx_size = ACQUISITION_TX_BUF_SIZE;
        
        for (uint8_t i = 0; i < tx_size; i++)
        {
             fifo_get_byte(&acquisition_fifo, &acquisition_tx_buf[i]);
        }
        
        //cdc_tx_big_packet((uint8_t*)&acquisition_tx_buf[0], tx_size);
        cdc_tx_small_packet2((uint8_t*)&acquisition_tx_buf[0], tx_size);
    } 
}

void acquisition_init(void)
{
    acc_sensor.LPMode = LPMODE_NORMAL;
    acc_sensor.Rate = BWRATE_3200;
    acc_sensor.SPIMode = SPIMODE_4WIRE;
    acc_sensor.IntMode = INT_ACTIVEHIGH;
    acc_sensor.Justify = JUSTIFY_SIGNED;
    acc_sensor.Resolution = RESOLUTION_10BIT;
    acc_sensor.Range = RANGE_2G;
    acc_sensor.AutoSleep = AUTOSLEEPOFF;
    acc_sensor.LinkMode = LINKMODEOFF;
    
    
    adxlStatus res = ADXL_Init(&acc_sensor);
    if (res != ADXL_OK)
    {
        return;
    }
    
    fifo_init_struct(&acquisition_fifo, acquisition_fifo_buf, ACQUISITION_FIFO_SIZE);
    
    ADXL_Measure(OFF);
    
    ADXL_enableDataReady(INT1);
    int16_t meas_values_g[3];
    ADXL_getAccel(&meas_values_g, OUTPUT_SIGNED);
    

    ADXL_Measure(ON);
    acquisition_timer_init();
    acquisition_init_ok = 1;
}

void acquisition_timer_init(void)
{
    HAL_TIM_Base_Start(&ACQUISITION_TIM_OBJ);
    __HAL_TIM_SET_PRESCALER(&ACQUISITION_TIM_OBJ, ACQUISITION_TIM_PRESC - 1);
}

//Put speed data to FIFO
void acquisition_generate_speed_data(uint16_t time_val)
{
    tx_item_t tx_item;
    tx_item.header1 = 0xAA;
    tx_item.header2 = 0x55;
    tx_item.type = 2;
    tx_item.time = time_val;
    tx_item.value_x = (int16_t)acquisition_acc_data_rate_hz;
    tx_item.value_y = 0;
    
    //copy data bytes to FIFO
    for (uint8_t i = 0; i < sizeof(tx_item); i++)
    {
        uint8_t res = fifo_add_byte(&acquisition_fifo, ((uint8_t*)&tx_item)[i]);
        if (res == 0)
            break;
    }
}

/// Read values from accelerometer
void acquisition_read_acc(void)
{
    int16_t acquisition_last_x;
    int16_t acquisition_last_y;
    
    if (acquisition_init_ok == 0)
    {
        return;
    }
    
    int16_t meas_values_g[3];
    ADXL_getAccel(&meas_values_g, OUTPUT_SIGNED);
    acquisition_last_x = meas_values_g[0];
    acquisition_last_y = meas_values_g[1];
    
    tx_item_t tx_item;
    tx_item.header1 = 0xAA;
    tx_item.header2 = 0x55;
    tx_item.type = 1;
    tx_item.time = acquisition_last_time;
    tx_item.value_x = acquisition_last_x;
    tx_item.value_y = acquisition_last_y;
    
    //copy data bytes to FIFO
    for (uint8_t i = 0; i < sizeof(tx_item); i++)
    {
        uint8_t res = fifo_add_byte(&acquisition_fifo, ((uint8_t*)&tx_item)[i]);
        if (res == 0)
            break;
    }
}

