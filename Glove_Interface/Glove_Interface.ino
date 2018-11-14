#include <Wire.h>
#include <Adafruit_Sensor.h>
#include <Adafruit_BNO055.h>
#include <utility/imumaths.h>

#define BNO055_SAMPLERATE_DELAY_MS (200)

Adafruit_BNO055 bno = Adafruit_BNO055(55);
int pin_A0 = A0;
int pin_A1 = A1;
int pin_A2 = A2;
int pin_A3 = A3;
int pin_A4 = A4;
int pin_A0_analog = 0;
int pin_A1_analog = 0;
int pin_A2_analog = 0;
int pin_A3_analog = 0;
int pin_A4_analog = 0;
float sensitivity = 1.75;

void displayCalStatus(void)
{
  /* Get the four calibration values (0..3) */
  /* Any sensor data reporting 0 should be ignored, */
  /* 3 means 'fully calibrated" */
  uint8_t system, gyro, accel, mag;
  system = gyro = accel = mag = 0;
  bno.getCalibration(&system, &gyro, &accel, &mag);
  imu::Vector<3> acceler = bno.getVector(Adafruit_BNO055::VECTOR_ACCELEROMETER);

  /* The data should be ignored until the system calibration is > 0 */
  Serial.print("\t");
  if (!system)
  {
    Serial.print("! ");
  }

  /* Display the individual values */
  Serial.print("Sys:");
  Serial.print(system, DEC);
  Serial.print(" G:");
  Serial.print(gyro, DEC);
  Serial.print(" A:");
  Serial.print(accel, DEC);
  Serial.print(" M:");
  Serial.print(mag, DEC);
  Serial.print(" A.x:");
  Serial.print(acceler.x(), DEC);
  Serial.print(" A.y:");
  Serial.print(acceler.y(), DEC);
  Serial.print(" A.z:");
  Serial.print(acceler.z(), DEC);
}

void setup() {
  // put your setup code here, to run once:
  Serial.begin(9600);

  /* Initialise the sensor */
  if(!bno.begin())
  {
    /* There was a problem detecting the BNO055 ... check your connections */
    Serial.print("Ooops, no BNO055 detected ... Check your wiring or I2C ADDR!");
    while(1);
  }

  delay(1000);
  bno.setExtCrystalUse(true);
}

void loop() {
  /*Flex sensor output*/
  int output = 0;

  /*Flex sensor read*/
  pin_A0_analog = analogRead(pin_A0);
  pin_A1_analog = analogRead(pin_A1);
  pin_A2_analog = analogRead(pin_A2);
  pin_A3_analog = analogRead(pin_A3);
  pin_A4_analog = analogRead(pin_A4);

  /*9-Axis Absolute Orientation*/
  sensors_event_t event;
  bno.getEvent(&event);

  /*Flex sensor data ouput*/
  float pin_A0_voltage =  (((float)pin_A0_analog*5.0)/1023.0);
  float pin_A1_voltage =  (((float)pin_A1_analog*5.0)/1023.0);
  float pin_A2_voltage =  (((float)pin_A2_analog*5.0)/1023.0);
  float pin_A3_voltage =  (((float)pin_A3_analog*5.0)/1023.0);
  float pin_A4_voltage =  (((float)pin_A4_analog*5.0)/1023.0);

  if (pin_A0_voltage < sensitivity) output = output | B1;
  if (pin_A1_voltage < sensitivity) output = output | B10;
  if (pin_A2_voltage < sensitivity) output = output | B100;
  if (pin_A3_voltage < sensitivity) output = output | B1000;
  if (pin_A4_voltage < sensitivity) output = output | B10000;
  
  /* Display the floating point data */
  Serial.print("X: ");
  Serial.print(event.orientation.x, 4);
  Serial.print("\tY: ");
  Serial.print(event.orientation.y, 4);
  Serial.print("\tZ: ");
  Serial.print(event.orientation.z, 4);

  /* Optional: Display calibration status */
  displayCalStatus();

  Serial.print("\t Flex: ");
  Serial.print(output,BIN);

  /* New line for the next sample */
  Serial.println("");

  /* Wait the specified delay before requesting nex data */
  delay(BNO055_SAMPLERATE_DELAY_MS);
  

}
