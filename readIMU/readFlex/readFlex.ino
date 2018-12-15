#include <Wire.h>
#include <Adafruit_Sensor.h>


// Pin connected to voltage divider output
// straight_res 90_deg_res actual_res finger
// will have to remeasure these after set in glove
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


/**************************************************************************/
/*
    Arduino setup function (automatically called at startup)
*/
/**************************************************************************/
void setup(void)
{
  Serial.begin(9600);
  pinMode(FLEX_PIN0, INPUT);
  pinMode(FLEX_PIN1, INPUT);
  pinMode(FLEX_PIN2, INPUT);
  pinMode(FLEX_PIN3, INPUT);
  pinMode(FLEX_PIN4, INPUT);
}

/**************************************************************************/
/*
    Arduino loop function, called once 'setup' is complete (your own code
    should go here)
*/
/**************************************************************************/
void loop(void)
{  
  // Read the ADC, and calculate voltage and resistance from it
  int flexADC0 = analogRead(FLEX_PIN0);
  float flexV0 = flexADC0 * VCC / 1023.0;
  float flexR0 = R_DIV * (VCC / flexV0 - 1.0);
  
  int flexADC1 = analogRead(FLEX_PIN1);
  float flexV1 = flexADC1 * VCC / 1023.0;
  float flexR1 = R_DIV * (VCC / flexV1 - 1.0);
  
   int flexADC2 = analogRead(FLEX_PIN2);
  float flexV2 = flexADC2 * VCC / 1023.0;
  float flexR2 = R_DIV * (VCC / flexV2 - 1.0);
  
   int flexADC3 = analogRead(FLEX_PIN3);
  float flexV3 = flexADC3 * VCC / 1023.0;
  float flexR3 = R_DIV * (VCC / flexV3 - 1.0);
  
   int flexADC4 = analogRead(FLEX_PIN4);
  float flexV4 = flexADC4 * VCC / 1023.0;
  float flexR4 = R_DIV * (VCC / flexV4 - 1.0);
 
  Serial.println("Resistance: " + String(flexR0) + " ohms");
  Serial.println("Resistance: " + String(flexR1) + " ohms");
  Serial.println("Resistance: " + String(flexR2) + " ohms");
  Serial.println("Resistance: " + String(flexR3) + " ohms");
  Serial.println("Resistance: " + String(flexR4) + " ohms");

  // Use the calculated resistance to estimate the sensor's
  // bend angle:
  float angle0 = map(flexR0, STRAIGHT_RESISTANCE, BEND_RESISTANCE, 0, 90.0);
  Serial.println("Bend: " + String(angle0) + " degrees");
  
  float angle1 = map(flexR1, STRAIGHT_RESISTANCE, BEND_RESISTANCE, 0, 90.0);
  Serial.println("Bend: " + String(angle1) + " degrees");
  
  float angle2 = map(flexR2, STRAIGHT_RESISTANCE, BEND_RESISTANCE, 0, 90.0);
  Serial.println("Bend: " + String(angle2) + " degrees");
  
  float angle3 = map(flexR3, STRAIGHT_RESISTANCE3, BEND_RESISTANCE3, 0, 90.0);
  Serial.println("Bend: " + String(angle3) + " degrees");
  
  float angle4 = map(flexR4, STRAIGHT_RESISTANCE, BEND_RESISTANCE, 0, 90.0);
  Serial.println("Bend: " + String(angle4) + " degrees");
  
  delay(250);

}
