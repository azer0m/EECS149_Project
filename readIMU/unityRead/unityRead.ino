#include <Wire.h>
#include <Adafruit_Sensor.h>
#include <Adafruit_BNO055.h>
#include <utility/imumaths.h>
#define BNO055_SAMPLERATE_DELAY_MS (10)
float x; 
float y;
float z;
String data;
Adafruit_BNO055 bno = Adafruit_BNO055();
void setup() {
  Serial.begin(9600);
  // You can choose any baudrate, just need to also change 
  //it in Unity.
  /* Initialise the sensor */
  if(!bno.begin())
  {
    /* There was a problem detecting the BNO055 ... check your connections */
    Serial.print("Ooops, no BNO055 detected ... Check your wiring or I2C ADDR!");
    while(1);
  }
  

}

// Run forever
void loop() {
  imu::Vector<3> euler = bno.getVector(Adafruit_BNO055::VECTOR_EULER);
  x = euler.x();
  y = euler.y();
  z = euler.z();
  data = String(x) + " " + String(y) + " " + String(z);
  Serial.println(data);
  Serial.flush();
  delay(50); 
  // Choose your delay having in mind your ReadTimeout in Unity3D
}

