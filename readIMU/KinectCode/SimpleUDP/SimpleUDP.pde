// Simple code to send data over UDP
 
import javax.imageio.*;
import java.awt.image.*; 
import java.net.*;
import java.io.*; 
import java.nio.*;
 
// This is the port we are sending to
int clientPort = 9100; 
// This is our object that sends UDP out
DatagramSocket ds;

int array_count;
int[] sendNum_int_array;
byte[] numsSent_byte_array;
 
void setup() {
  size(480, 480);
  
  // Setting up the DatagramSocket, requires try/catch
  try {
    ds = new DatagramSocket();
  } 
  catch (SocketException e) {
    e.printStackTrace();
  }
  
  //define array_count based on skip  
  array_count = 0;
  for (int x = 0; x<100; x++) {
    array_count++;
  }
  
  sendNum_int_array = new int[array_count];
  numsSent_byte_array = new byte[array_count*4];

  array_count = 0; 
 
  }
 
void draw() {
 
  background(0);
 
  for (int x = 0; x<100; x++) {
    sendNum_int_array[array_count] = x;
    array_count++;
  }
 
  //convert int array to byte array
  ByteBuffer byteBuffer = ByteBuffer.allocate(sendNum_int_array.length * 4);
  IntBuffer intBuffer = byteBuffer.asIntBuffer();
  intBuffer.put(sendNum_int_array);
  
  numsSent_byte_array = byteBuffer.array();
  
  try {
    println("Sending datagram with " + numsSent_byte_array.length + " bytes");
    ds.send(new DatagramPacket(numsSent_byte_array, numsSent_byte_array.length, InetAddress.getByName("localhost"), clientPort));
  }
  catch (Exception e) {
    e.printStackTrace();
  }
  
  array_count = 0;
}
 
// These functions come from: <a href="http://graphics.stanford.edu/~mdfisher/Kinect.html" target="_blank" rel="nofollow">http://graphics.stanford.edu/~mdfisher/Kinect.html</a>
float rawDepthToMeters(int depthValue) {
  if (depthValue < 2047) {
    return (float)(1.0 / ((double)(depthValue) * -0.0030711016 + 3.3309495161));
  }
  return 0.0f;
}
