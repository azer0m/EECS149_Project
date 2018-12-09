import hypermedia.net.*;

import processing.video.*;
import java.io.*;
import java.net.*;
import javax.imageio.*;
import java.awt.image.*;

import processing.serial.*;

// Ahmed Malik
// EECS 149 Final Project
// GOAL: Hand tracking via centroid algorithm
// 12/5/2018

// Import OpenKinect library
import org.openkinect.processing.*;

DatagramSocket ds; 

// Kinect2 Library object
Kinect2 kinect2;

Serial myPort;

// Object type for writing output file
PrintWriter output;

// minimum and maximum bounds for depth visualization
float minThresh = 400;
float maxThresh = 800;

// Rendered image object
PImage img;

// Depth array
int[] depth;
// Location in grid of pixels in actual list data structure
int offset;
// Depth value d, in mm
int d;
// Current mouse location
int loc;

// Sum of all x pixel locations in image, within threshold
int sumX;
// Sum of all y pixel locations in image, within threshold
int sumY;
// Total number of individual pixels in image, within threshold
int totalPixels;
// Average x pixel location
int avgX;
// Average y pixel location
int avgY;

void setup() {
  // Size of Kinect depth camera visualization
  size(512, 424);
  
  try {
    ds = new DatagramSocket();
  } catch (SocketException e) {
    e.printStackTrace();
  }
  
  println("************************"+Serial.list()[1]);
  String portName = Serial.list()[1];
  myPort = new Serial(this, portName, 9600);

  // Setup Kinect2 and image
  kinect2 = new Kinect2(this);
  kinect2.initDepth();
  kinect2.initVideo();
  kinect2.initDevice();
  img = createImage(kinect2.depthWidth, kinect2.depthHeight, RGB);
  
  //output = createWriter("hand_coordinates.csv");
}

void draw() {
  background(0);
  img.loadPixels();
  
  // Get the raw depth as array of integers
  depth = kinect2.getRawDepth();
  
  sumX = 0;
  sumY = 0;
  totalPixels = 0;
  
  // Iterate through each pixel along the x-axis
  for (int x=0; x < kinect2.depthWidth; x++) {
    // Iterate through each pixel along the y-axis
    for (int y=0; y < kinect2.depthHeight; y++) {
      // Set offset
      offset = x + (y * kinect2.depthWidth);
      // Set depth value for current location in list
      d = depth[offset];
      
      // If the depth value of the object is within our thresholds
      if (d > minThresh && d < maxThresh) {
        // Color it
        img.pixels[offset] = color(255, 0, 155);
        // Increment vars for centroid algorithm
        sumX += x;
        sumY += y;
        totalPixels++;
      } else {
        // Otherwise leave it black
        img.pixels[offset] = color(0);
      }
    }
  }
  
  // Color the pixels by updating them 
  img.updatePixels();
  // Draw image to display window
  image(img,0,0);
  // Current mouse location determined similarly to offset, in list data structure
  //loc = mouseX + (mouseY*kinect2.depthWidth);
  // Set size of onscreen text display
  textSize(32);
  //text("("+mouseX+", "+mouseY+", "+depth[loc]+")",10,64);
  
  if (totalPixels < 1) {
    totalPixels = 1;
  }
  avgX = sumX/totalPixels;
  avgY = sumY/totalPixels;
  fill(255);
  ellipse(avgX, avgY, 10, 10);
  int new_offset = avgX + (avgY * kinect2.depthWidth);
  PVector apoint = depthToPointCloudPos(avgX, avgY, depth[new_offset]);
  //text("("+int(apoint.x)+", "+int(apoint.y)+", "+int(apoint.z) +")", 10, 64);
  
  //output.println(int(apoint.x) + ", " + int(apoint.y) + ", " + int(apoint.z));
  
  //for (int x = 0; x < 100; x++) {
  //  myPort.write(x);
  //  println(x);
  //  delay(10);
  //}
  //exit();
}

//void keyPressed() {
//  output.flush();
//  output.close();
//  exit();
//}

//calculte the xyz camera position based on the depth data
PVector depthToPointCloudPos(int x, int y, float depthValue) {
  PVector point = new PVector();
  point.z = (depthValue);// / (1.0f); // Convert from mm to meters
  point.x = (x - CameraParams.cx) * point.z / CameraParams.fx;
  point.y = (y - CameraParams.cy) * point.z / CameraParams.fy;
  return point;
}
