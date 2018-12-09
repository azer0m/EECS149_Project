PrintWriter output;

void setup() {
  // Create a new file in the sketch directory
  output = createWriter("data.txt");
}

void draw() {
  background(0);
  fill(255);
  point(mouseX, mouseY);
  output.println(mouseX + "t" + mouseY);
}

void keyPressed() {
  output.flush();
  output.close();
  exit();
}
