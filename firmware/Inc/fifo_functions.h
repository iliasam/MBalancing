#ifndef FIFO_FUNCTIONS_H
#define FIFO_FUNCTIONS_H

#include <stdint.h>

// Defines ********************************************************************

#if defined ( __ICCARM__ ) // IAR
    #define ENTER_CRITICAL(x)       x=__get_interrupt_state(); __disable_interrupt()
    #define LEAVE_CRITICAL(x)       __set_interrupt_state(x)

#elif defined (__CC_ARM) // KEIL
    #define ENTER_CRITICAL(x)       x=__disable_irq()
    #define LEAVE_CRITICAL(x)       if (!x) __enable_irq()
    #define NO_INIT                 __attribute__((zero_init))
#else
  #error ERROR
#endif

typedef struct
{
  //Fifo total size
  uint16_t fifo_size;
  
  //Number of elements in FIFO
  uint16_t fifo_count;
  
  // Current writing pointer, in bytes
  uint16_t write_ptr;
  
  // Current reading pointer, in bytes
  uint16_t read_ptr;
  
  // Pointer to FIFO data
  uint8_t* fifo_data;

} fifo_struct_t;

// Functions ******************************************************************

void fifo_init_struct(fifo_struct_t* fifo, uint8_t* data_buffer, uint16_t size);
uint8_t fifo_add_byte(fifo_struct_t* fifo, uint8_t value);
uint8_t fifo_get_byte(fifo_struct_t* fifo, uint8_t* value);
uint16_t fifo_get_count(fifo_struct_t* fifo);
uint8_t fifo_add_data(fifo_struct_t* fifo, uint8_t* data, uint16_t size);


#endif
