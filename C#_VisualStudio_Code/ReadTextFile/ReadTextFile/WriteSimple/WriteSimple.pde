PrintWriter output;
//PrintWriter o2;

void setup() {
  // Create a new file in the sketch directory
  output = createWriter("testread.txt");
  //o2 = createWriter("o2.txt");
}

void draw() {
  background(0);
  fill(255);
  point(mouseX, mouseY);
  output.println(mouseX + "t" + mouseY);
  //o2.println(mouseX + "t" + mouseY);
  output.flush();
  output.close();
}

//void keyPressed() {
//  //o2.flush();
//  //o2.close();
//  exit();
//}
