# MBalancing
Device for dynamic motor balancing

This device is based on STM32 Blue Pill board.  
This device consists of:  
* Laser module + phototransistor + resistor = optical sensor of motor zero angle.  
* ADXL345 accelerometer module - used for vibration measurement  
* Blue Pill - STM32F103 board.  
  
ADXL345 accelerometer must be fixed at the body of the motor, perpendicular to its shaft.  
STM32 is capturing data from X/Y axis of ADXL345 and send them to the Windows utility using USB-VCP.  
Zero angle sensor is used for measuring real RPM and for measuring vibration phase phase shift.

Windows utility is displaying:  
* Shaft RPM and rotation speed in Hz
* X axis raw data plot  
* X axis signal spectrum
* Amplitudes of vibration at rotation speed of motor shaft
* Measured phases of vibration at rotation speed of motor shaft
* X axis phase distribution
  
PC utility interface:  
<img src="https://github.com/iliasam/MBalancing/blob/develop/Images/MBalancing.png" width="600">  
  
STM32 Blue Pill Pinout:  
PA0 - zero sensor, IRQ at rising edge.  
PA5 - ADXL345 SPI CLK  
PA6 - ADXL345 SPI MISO  
PA7 - ADXL345 SPI MOSI  
PB0 - ADXL345 SPI CSn  
PB1 - ADXL345 IRQ line  
  
Firmware is assembled with CubeMX for Keil, PC utility is build in MS VS 2017.  
