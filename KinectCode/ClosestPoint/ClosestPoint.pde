// Ahmed Malik
// EECS 149 Final Project
// GOAL: Hand tracking via world-record algorithm
// 12/5/2018

// Import OpenKinect library
import org.openkinect.processing.*;
//import java.nio.FloatBuffer;

// Kinect2 Library object
Kinect2 kinect2;

// Object type for writing output file
PrintWriter output;

// Set minimum and maximum bounds for depth visualization
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

// Record minimum depth value recorded so far 
int recordD;
// Corresponding x pixel location
int recordX;
// Corresponding y pixel location
int recordY;

void setup() {
  // Size of Kinect depth camera visualization
  size(512, 424);

  // Setup Kinect2 and image
  kinect2 = new Kinect2(this);
  kinect2.initDepth();
  kinect2.initVideo();
  kinect2.initDevice();
  img = createImage(kinect2.depthWidth, kinect2.depthHeight, RGB);
  
  output = createWriter("hand_coordinates.csv");
}

void draw() {
  background(0);
  img.loadPixels();
  
  // Get the raw depth as array of integers
  depth = kinect2.getRawDepth();
  
  // Reset to maximum value of 4500mm every draw iteration
  //recordD = 4500;
  
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
        // Set vars for closest point algorithm
        if (d < recordD) {
          recordD = d;
          recordX = x;
          recordY = y;
        }
      } else {
        recordD = 4500;
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
  
  fill(150, 0, 255);
  ellipse(recordX, recordY, 18, 18);
  int new_offset = recordX + (recordY * kinect2.depthWidth);
  text("("+recordX+", "+recordY+", "+depth[new_offset]+")",10,64);
  //println("WIDTH: "+width+", HEIGHT: "+height);
  
  output.println(recordX + ", " + recordY + ", " + depth[new_offset]);
}

void keyPressed() {
  output.flush();
  output.close();
  exit();
}
