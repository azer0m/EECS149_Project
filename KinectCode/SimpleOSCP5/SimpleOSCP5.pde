//load libraries
import netP5.*;
import oscP5.*;
 
OscP5 oscP5;
NetAddress myRemoteLocation;
int somevalue = 15;
 
//make a connection
void setup () {
  oscP5 = new OscP5 (this, 9000);
  myRemoteLocation = new NetAddress("127.0.0.1", 9000);
}
 
// this is where you can make things happen and send values to Unity
void draw () {
  OscMessage myMessage = new OscMessage ("/");
    // add a value to the message (optional)
    myMessage.add(somevalue);
    // add a bool to the message (optional)
    myMessage.add(true);
  oscP5.send(myMessage, myRemoteLocation);
}
 
// this is where you get feedback in Processing
void oscEvent (OscMessage theOscMessage) {
  print("### received an osc message.");
  print(" addrpattern: "+theOscMessage.addrPattern());
  println(" typetag: "+theOscMessage.typetag());
  //println("### received an osc message. with address pattern");
  //println(theOscMessage.addrPattern());
  //println(theOscMessage.typetag());
}
