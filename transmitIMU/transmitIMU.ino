
#include <SPI.h>
#include <nRF24L01.h>
#include <RF24.h>
#include <Wire.h>
#include <Adafruit_Sensor.h>
#include <Adafruit_BNO055.h>
#include <utility/imumaths.h>

/* Set the delay between fresh samples */
#define BNO055_SAMPLERATE_DELAY_MS (100)

Adafruit_BNO055 bno = Adafruit_BNO055();

RF24 radio(7, 8); // CE, CSN
const byte address[6] = "00001";
float x; 
float y;
float z;
int b;
String data;
void setup() {
  if(!bno.begin()) {
    /* There was a problem detecting the BNO055 ... check your connections */
    while(1);
  }
  radio.begin();
  radio.openWritingPipe(address);
  radio.setPALevel(RF24_PA_MIN);
  radio.stopListening();
  delay(1000);
  bno.setExtCrystalUse(true);
}
void loop() {
  imu::Vector<3> euler = bno.getVector(Adafruit_BNO055::VECTOR_EULER);
  x = euler.x();
  y = euler.y();
  z = euler.z();
  data = String(x) + " " + String(y) + " " + String(z);
  const char* text = data.c_str(); 
  // will have to change size of multiplier for longer text
  // remider: try passing in sizeof(*text)
  radio.write(text, sizeof(text)*10);
  delay(250);
}
