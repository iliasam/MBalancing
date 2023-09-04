
/* Define to prevent recursive inclusion -------------------------------------*/
#ifndef __ACQUISITION_H__
#define __ACQUISITION_H__

/* Includes ------------------------------------------------------------------*/
#include "stm32f1xx_hal.h"

/* Private define ------------------------------------------------------------*/
void acquisition_init(void);
void acquisition_read_acc(void);
void acquisition_acc_irq(void);
void acquisition_zero_sens_irq(void);
void acquisition_periodic_handler(void);

void profiler_systick_handler(void);
void profiler_parse_command(uint8_t* command, uint16_t length);
void profiler_handler(void);
void profiler_start_adc(void);

#endif /* __ACQUISITION_H__ */
