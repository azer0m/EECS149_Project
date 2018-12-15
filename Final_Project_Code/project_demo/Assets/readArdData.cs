//some of these may be redundant 
using System.Collections;
using System.Collections.Generic;
using System;
using System.Globalization;
using UnityEngine;
using System.IO;
using System.IO.Ports;
using System.Threading;



public class readArdData : MonoBehaviour
{
    // Use this for initialization

    // make sure to get the right serial port and baud rate for microcontroller
    SerialPort sp = new SerialPort("/dev/cu.usbmodem14501", 9600);
    string data;
    int x;
    int y;
    int z;
    int a0;
    int a1;
    int a2;
    int a3;
    int a4;
    int i = 0;
    //how to declare objects to attach to child nodes of parent
    //like for fingers to a hand
    public GameObject knuckle_index;
    public GameObject mid_index;
    public GameObject last_index;

    public GameObject knuckle_middle;
    public GameObject mid_middle;
    public GameObject last_middle;

    public GameObject knuckle_ring;
    public GameObject mid_ring;
    public GameObject last_ring;

    public GameObject knuckle_thumb;
    public GameObject mid_thumb;
    public GameObject last_thumb;

    public GameObject knuckle_pinky;
    public GameObject mid_pinky;
    public GameObject last_pinky;

    float ki_rest_x;
    float ki_rest_y;
    float ki_rest_z;

    float mi_rest_x;
    float mi_rest_y;
    float mi_rest_z;

    float li_rest_x;
    float li_rest_y;
    float li_rest_z;

    float km_rest_x;
    float km_rest_y;
    float km_rest_z;

    float mm_rest_x;
    float mm_rest_y;
    float mm_rest_z;

    float lm_rest_x;
    float lm_rest_y;
    float lm_rest_z;

    float kr_rest_x;
    float kr_rest_y;
    float kr_rest_z;

    float mr_rest_x;
    float mr_rest_y;
    float mr_rest_z;

    float lr_rest_x;
    float lr_rest_y;
    float lr_rest_z;

    float kt_rest_x;
    float kt_rest_y;
    float kt_rest_z;

    float mt_rest_x;
    float mt_rest_y;
    float mt_rest_z;

    float lt_rest_x;
    float lt_rest_y;
    float lt_rest_z;


    float kp_rest_x;
    float kp_rest_y;
    float kp_rest_z;

    float mp_rest_x; 
    float mp_rest_y;
    float mp_rest_z;

    float lp_rest_x;
    float lp_rest_y;
    float lp_rest_z;

    public string state = "not grabbing";

    public float GrabDistance = 0.1f;
    public string GrabTag = "Grab";
    public float ThrowMultiplier=1.5f;

    public Transform CurrentGrabObject
    {
        get { return _currentGrabObject; }
        set { _currentGrabObject = value; }
    }

    private Vector3 _lastFramePosition;
    private Transform _currentGrabObject;
    private bool _isGrabbing;

    void Start()
    {

        ki_rest_x = knuckle_index.transform.localEulerAngles.x;
        ki_rest_y = knuckle_index.transform.localEulerAngles.y;
        ki_rest_z = knuckle_index.transform.localEulerAngles.z;

        mi_rest_x = mid_index.transform.localEulerAngles.x;
        mi_rest_y = mid_index.transform.localEulerAngles.y;
        mi_rest_z = mid_index.transform.localEulerAngles.z;

        li_rest_x = last_index.transform.localEulerAngles.x;
        li_rest_y = last_index.transform.localEulerAngles.y;
        li_rest_z = last_index.transform.localEulerAngles.z;

        km_rest_x = knuckle_middle.transform.localEulerAngles.x;
        km_rest_y = knuckle_middle.transform.localEulerAngles.y;
        km_rest_z = knuckle_middle.transform.localEulerAngles.z;

        mm_rest_x = mid_middle.transform.localEulerAngles.x;
        mm_rest_y = mid_middle.transform.localEulerAngles.y;
        mm_rest_z = mid_middle.transform.localEulerAngles.z;

        lm_rest_x = last_middle.transform.localEulerAngles.x;
        lm_rest_y = last_middle.transform.localEulerAngles.y;
        lm_rest_z = last_middle.transform.localEulerAngles.z;

        kr_rest_x = knuckle_ring.transform.localEulerAngles.x;
        kr_rest_y = knuckle_ring.transform.localEulerAngles.y;
        kr_rest_z = knuckle_ring.transform.localEulerAngles.z;

        mr_rest_x = mid_ring.transform.localEulerAngles.x;
        mr_rest_y = mid_ring.transform.localEulerAngles.y;
        mr_rest_z = mid_ring.transform.localEulerAngles.z;

        lr_rest_x = last_ring.transform.localEulerAngles.x;
        lr_rest_y = last_ring.transform.localEulerAngles.y;
        lr_rest_z = last_ring.transform.localEulerAngles.z;

        kt_rest_x = knuckle_thumb.transform.localEulerAngles.x;
        kt_rest_y = knuckle_thumb.transform.localEulerAngles.y;
        kt_rest_z = knuckle_thumb.transform.localEulerAngles.z;

        mt_rest_x = mid_thumb.transform.localEulerAngles.x;
        mt_rest_y = mid_thumb.transform.localEulerAngles.y;
        mt_rest_z = mid_thumb.transform.localEulerAngles.z;

        lt_rest_x = last_thumb.transform.localEulerAngles.x;
        lt_rest_y = last_thumb.transform.localEulerAngles.y;
        lt_rest_z = last_thumb.transform.localEulerAngles.z;

        kp_rest_x = knuckle_pinky.transform.localEulerAngles.x;
        kp_rest_y = knuckle_pinky.transform.localEulerAngles.y;
        kp_rest_z = knuckle_pinky.transform.localEulerAngles.z;

        mp_rest_x = mid_pinky.transform.localEulerAngles.x;
        mp_rest_y = mid_pinky.transform.localEulerAngles.y;
        mp_rest_z = mid_pinky.transform.localEulerAngles.z;

        lp_rest_x = last_pinky.transform.localEulerAngles.x;
        lp_rest_y = last_pinky.transform.localEulerAngles.y;
        lp_rest_z = last_pinky.transform.localEulerAngles.z;

        sp.Open();
        sp.ReadTimeout = 100;
        Thread aThread = new Thread(new ThreadStart(arduinoThread));
        aThread.IsBackground = true;
        aThread.Start();

        _currentGrabObject = null;

        _isGrabbing = false;

        //if timeout set higher unity might freeze while trying to read from the serial port
        //when the rate is low need a try/catch because unity will check if the port is open
        //and throw an exeption when it isn't open
    }

    // Update is called once per frame
    void Update()

    {
        if (sp.IsOpen)
        {
                if (i == 8)
            {
                try
                {
                    //data = sp.ReadLine();

                    string[] orientation = data.Split(' ');
                    //Debug.Log(orientation.Length);
                    if (orientation.Length == 8)
                    {
                        x = Convert.ToInt32(orientation[0]);
                        y = Convert.ToInt32(orientation[1]);
                        z = Convert.ToInt32(orientation[2]);
                        transform.eulerAngles = new Vector3(-y, x, z);
                        a0 = Convert.ToInt32(orientation[3]);
                        a1 = Convert.ToInt32(orientation[4]);
                        a2 = Convert.ToInt32(orientation[5]);
                        a3 = Convert.ToInt32(orientation[6]);
                        a4 = Convert.ToInt32(orientation[7]);

                        moveFingers("ring", a0);
                        moveFingers("middle", a1);
                        moveFingers("index", a2);
                        moveFingers("thumb", a3);
                        moveFingers("pinky", a4);
                        //Debug.Log(a4);
                        //sp.BaseStream.Flush();

                    }
                    i = 0;
                }
                catch (System.Exception)
                {
                }
            }
            i++;
        }

                //if we don't have an active object in hand, look if there is one in proximity
        if (_currentGrabObject == null)
        {
            //check for colliders in proximity
            Collider[] colliders = Physics.OverlapSphere(transform.position, GrabDistance);
            if (colliders.Length > 0)
            {
                //if there are colliders, take the first one if we press the grab button and it has the tag for grabbing
                if ((state == "grabbing") && colliders[0].transform.CompareTag(GrabTag))
                {
                    //if we are already grabbing, return
                    if(_isGrabbing)
                    {
                        return;
                    }
                    _isGrabbing = true;

                    //set current object to the object we have picked up (set it as child)
                    colliders[0].transform.SetParent(transform);

                    //if there is no rigidbody to the grabbed object attached, add one
                    if(colliders[0].GetComponent<Rigidbody>() == null)
                    {
                        colliders[0].gameObject.AddComponent<Rigidbody>();
                    }

                    //set grab object to kinematic (disable physics)
                    colliders[0].GetComponent<Rigidbody>().isKinematic = true;


                    //save a reference to grab object
                    _currentGrabObject = colliders[0].transform;



                }
            }
        }
        else
        //we have object in hand, update its position with the current hand position (+defined offset from it)
        {

            //if we we release grab button, release current object
            if (state == "not grabbing")
            {


                //set grab object to non-kinematic (enable physics)
                Rigidbody _objectRGB = _currentGrabObject.GetComponent<Rigidbody>();
                _objectRGB.isKinematic = false;
                _objectRGB.collisionDetectionMode = CollisionDetectionMode.Continuous;

                //calculate the hand's current velocity
                Vector3 CurrentVelocity = (transform.position - _lastFramePosition) / Time.deltaTime;

                //set the grabbed object's velocity to the current velocity of the hand
                _objectRGB.velocity = CurrentVelocity * ThrowMultiplier;

                //release the the object (unparent it)
                _currentGrabObject.SetParent(null);

                //release reference to object
                _currentGrabObject = null;
            }

        }

        //release grab ?
        if ((state == "not grabbing") && _isGrabbing)
        {
            _isGrabbing = false;
        }

            //save the current position for calculation of velocity in next frame
            _lastFramePosition = transform.position;
        }

    void moveFingers(String finger, int bend)
    {
        int k = 0;
        int m = 0;
        int l = 0;

        int kt = 0;
        int mt = 0;
        int lt = 0;

        //open
        if (((finger == "ring") && bend < 19) ||
           ((finger == "middle") && bend < 14) ||
            ((finger == "index") && bend < 11) ||
            ((finger == "thumb") && bend < 11) ||
            ((finger == "pinky") && bend < 10))
        {
            state = "not grabbing";
            k = -15;
            m = -30;
            l = -15;

            kt = k;
            mt = m;
            lt = l;
        }

        //rest
        if (((finger == "ring") && (bend >= 19) && (bend < 35)) ||
           ((finger == "middle") && (bend >= 14) && (bend < 32)) ||
            ((finger == "index") && (bend >= 11) && (bend < 30)) ||
            ((finger == "thumb") && (bend >= 11) && (bend < 20)) ||
            ((finger == "pinky") && (bend >= 10) && (bend < 60)))
        {
            state = "not grabbing";
            k = 0;
            m = 0;
            l = 0;

            kt = k;
            mt = m;
            lt = l;
        }

        //semi-closed
        if (((finger == "ring") && (bend >= 35) && (bend < 65)) ||
           ((finger == "middle") && (bend >= 32) && (bend < 60)) ||
            ((finger == "index") && (bend >= 30) && (bend < 60)) ||
            ((finger == "thumb") && (bend >= 20) && (bend < 55)) ||
            ((finger == "pinky") && (bend >= 60) && (bend < 85)))
        {
            state = "grabbing";
            k = 30;
            m = 60;
            l = 120;

            kt = 30;
            mt = 0;
            lt = 0;
        }

        //closed
        if (((finger == "ring") && bend > 65) ||
           ((finger == "middle") && bend > 60) ||
            ((finger == "index") && bend > 60) ||
            ((finger == "thumb") && bend > 55) ||
            ((finger == "pinky") && bend > 85))
        {
            state = "grabbing";
            k = 60;
            m = 120;
            l = 180;

            kt = 30;
            mt = 0;
            lt = 30;
        }

        if (finger == "index")
        {
            knuckle_index.transform.localEulerAngles = new Vector3(ki_rest_x, ki_rest_y, ki_rest_z - k);
            mid_index.transform.localEulerAngles = new Vector3(mi_rest_x, mi_rest_y, mi_rest_z - m);
            last_index.transform.localEulerAngles = new Vector3(li_rest_x, li_rest_y, li_rest_z - l);
        }
        if (finger == "middle")
        {
            knuckle_middle.transform.localEulerAngles = new Vector3(km_rest_x, km_rest_y, km_rest_z - k);
            mid_middle.transform.localEulerAngles = new Vector3(mm_rest_x, mm_rest_y, mm_rest_z - m);
            last_middle.transform.localEulerAngles = new Vector3(lm_rest_x, lm_rest_y, lm_rest_z - l);
        }
        if (finger == "ring")
        {
            knuckle_ring.transform.localEulerAngles = new Vector3(kr_rest_x, kr_rest_y, kr_rest_z - k);
            mid_ring.transform.localEulerAngles = new Vector3(mr_rest_x, mr_rest_y, mr_rest_z - m);
            last_ring.transform.localEulerAngles = new Vector3(lr_rest_x, lr_rest_y, lr_rest_z - l);
        }
        if (finger == "thumb") 
        {
            knuckle_thumb.transform.localEulerAngles = new Vector3(kt_rest_x, kt_rest_y, kt_rest_z - kt);
            mid_thumb.transform.localEulerAngles = new Vector3(mt_rest_x, mt_rest_y, mt_rest_z - mt);
            last_thumb.transform.localEulerAngles = new Vector3(lt_rest_x, lt_rest_y, lt_rest_z - lt);
        }
        if (finger == "pinky")
        {
            knuckle_pinky.transform.localEulerAngles = new Vector3(kp_rest_x, kp_rest_y, kp_rest_z - k);
            mid_pinky.transform.localEulerAngles = new Vector3(mp_rest_x, mp_rest_y, mp_rest_z - m);
            last_pinky.transform.localEulerAngles = new Vector3(lp_rest_x, lp_rest_y, lp_rest_z - l);
        } 

    }

    public void arduinoThread() {
        //sp.Open();
        //sp.ReadTimeout = 120;
        while (true){
            try {
            data = sp.ReadLine();
            sp.BaseStream.Flush();
        }catch (System.Exception)
                {
                }
        }
    }



}
