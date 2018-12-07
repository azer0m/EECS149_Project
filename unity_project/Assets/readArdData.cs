//some of these may be redundant 
using System.Collections;
using System.Collections.Generic;
using System;
using System.Globalization;
using UnityEngine;
using System.IO.Ports;



public class ReadArdData : MonoBehaviour {
    // Use this for initialization

    public float speed;
	private float amountToMove;
    // make sure to get the right serial port and baud rate for microcontroller
    //
	SerialPort sp = new SerialPort("/dev/cu.usbmodem144401", 9600);
    string data;
    float x;
    float y;
    float z;
    //how to declare objects to attach to child nodes of parent
    //like for fingers to a hand
    public GameObject test;

    void Start () {
		sp.Open();
		sp.ReadTimeout = 1; 
		//if timeout set higher unity might freeze while trying to read from the serial port
		//when the rate is low need a try/catch because unity will check if the port is open
		//and throw an exeption when it isn't open
	}
	
	// Update is called once per frame
	void Update () {
		amountToMove = speed * Time.deltaTime; // makes movement framerate independent
		if (sp.IsOpen) {
			try {
                data = sp.ReadLine();
                string[] orientation = data.Split(' ');
                x = Convert.ToSingle(orientation[0]);
                y = Convert.ToSingle(orientation[1]);
                z = Convert.ToSingle(orientation[2]);
                transform.eulerAngles = new Vector3(-y, x, z);

            using(var reader = new StreamReader(@"~/149/EECS149_Project/KinectCode/CentroidTracking/hand_coordinates.csv")) {

                List<string> listA = new List<string>();
                List<string> listB = new List<string>();
                List<string> listC = new List<string>();
                while (!reader.EndOfStream) {

                    var line = reader.ReadLine();
                    var values = line.Split(';');
                    listA.Add(values[0]);
                    listB.Add(values[1]);
                    listC.Add(values[2]);
                    }
                }
            }
			catch(System.Exception) {
			}
		}
        //rotates object by repective degrees
        //transform.Rotate(x_degree * Time.deltaTime, y_degree * Time.deltaTime, z_degree * Time.deltaTime, Space.World);

        //rotate child object
        //test.transform.Rotate(10 * Time.deltaTime, 0, 0, Space.World);
    }

    // was going to create function to take care of parsing done in update
    // maybe a todo if it gets messy
    void parseAndAction(string data) {

    }
    
    // move object to left or right by amount to move, probably similar logic for up and down
	void MoveObject(int Direction) {

		if (Direction == 1) {
			transform.Translate(Vector3.left * amountToMove, Space.World);
			//Space.World makes movement relative to world frame

		}
		if (Direction == 2) {
			transform.Translate(Vector3.right * amountToMove, Space.World);

		}

	}
}
