
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

const int FLEX_PIN0 = A0; //8,8 19.5 9.85 ring
const int FLEX_PIN1 = A1;// 9.42 16.0 9.83 middle
const int FLEX_PIN2 = A2;// 9.19 16.25 9.84 index
const int FLEX_PIN3 = A3; // 8.42 15.0 17.4 9.83 thumb
const int FLEX_PIN4 = A6; // 22.8 60.0 26.7 pinky

// Measure the voltage at 5V and the actual resistance of your
// resistor, and enter them below:
const float VCC = 3.3; // Measured voltage of Ardunio 5V line
const float R_DIV = 9840.0; // Measured resistance of 3.3k resistor

// Upload the code, then try to adjust these values to more
// accurately calculate bend degree.
// will have to make one of these for each flex sensor for best accuracy
const float STRAIGHT_RESISTANCE = 9000.0; // resistance when straight
const float BEND_RESISTANCE = 20000.0; // resistance at 90 deg

const float STRAIGHT_RESISTANCE3 = 10700.0; // resistance when straight
const float BEND_RESISTANCE3 = 18500.0; // resistance at 90 deg

int flexADC0;
float flexV0;
float flexR0;
int angle0;
 
int flexADC1;
float flexV1;
float flexR1;
int angle1;
 
int flexADC2;
float flexV2;
float flexR2;
int angle2;

int flexADC3;
float flexV3;
float flexR3;
int angle3;

int flexADC4;
float flexV4;
float flexR4;
int angle4;

RF24 radio(7, 8); // CE, CSN
const byte address[6] = "00001";
int x; 
int y;
int z;
int b;
String data;
void setup() {
  
  pinMode(FLEX_PIN0, INPUT);
  pinMode(FLEX_PIN1, INPUT);
  pinMode(FLEX_PIN2, INPUT);
  pinMode(FLEX_PIN3, INPUT);
  pinMode(FLEX_PIN4, INPUT);
  
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
  
  flexADC0 = analogRead(FLEX_PIN0);
  flexV0 = flexADC0 * VCC / 1023.0;
  flexR0 = R_DIV * (VCC / flexV0 - 1.0);
  angle0 = (int)map(flexR0, STRAIGHT_RESISTANCE, BEND_RESISTANCE, 0, 90.0);
  
  flexADC1 = analogRead(FLEX_PIN1);
  flexV1 = flexADC1 * VCC / 1023.0;
  flexR1 = R_DIV * (VCC / flexV1 - 1.0);
  angle1 = (int)map(flexR1, STRAIGHT_RESISTANCE, BEND_RESISTANCE, 0, 90.0);

  flexADC2 = analogRead(FLEX_PIN2);
  flexV2 = flexADC2 * VCC / 1023.0;
  flexR2 = R_DIV * (VCC / flexV2 - 1.0);
  angle2 = (int)map(flexR2, STRAIGHT_RESISTANCE, BEND_RESISTANCE, 0, 90.0);

  flexADC3 = analogRead(FLEX_PIN3);
  flexV3 = flexADC3 * VCC / 1023.0;
  flexR3 = R_DIV * (VCC / flexV3 - 1.0);
  angle3 = (int)map(flexR3, STRAIGHT_RESISTANCE3, BEND_RESISTANCE3, 0, 90.0);

  flexADC4 = analogRead(FLEX_PIN4);
  flexV4 = flexADC4 * VCC / 1023.0;
  flexR4 = R_DIV * (VCC / flexV4 - 1.0);
  angle4 = (int)map(flexR4, STRAIGHT_RESISTANCE, BEND_RESISTANCE, 0, 90.0);
  
  x = (int)euler.x();
  y = (int)euler.y();
  z = (int)euler.z();
  data = String(x) + " " + String(y) + " " + String(z) + " " + String(angle0) + " " + String(angle1) + " " + String(angle2) + " " + String(angle3) + " " + String(angle4);
  const char* text = data.c_str(); 
  // will have to change size of multiplier for longer text
  // remider: try passing in sizeof(*text)
  radio.write(text, sizeof(text)*20);
  delay(BNO055_SAMPLERATE_DELAY_MS);
}
