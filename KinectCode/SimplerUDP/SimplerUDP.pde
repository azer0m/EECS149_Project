import hypermedia.net.*;

int port = 9300;
String ip ="127.0.0.1"; //localhost
//String message =new String("Hello Goose");
String message;
int msgNum=-1;
UDP udpTX;

void setup(){
udpTX=new UDP(this);
//udpTX = new UDP(this, port, ip);
//udpTX.loopback(true);r
//udpTX=new UDP(this);
udpTX.log(true);
noLoop();
}

void draw(){
  msgNum++;
  message = str(msgNum);
  udpTX.send(message,ip,port);
  delay(2500);
  loop();
}

void keyPressed(){
  udpTX.close();
  exit();
}
