using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjIntrinsics : MonoBehaviour
{
    public Camera projView;

    public UDPReceiver udpReceiver;
    
    private float f = 35.0f;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        changeCameraParam();
    }

    public void changeCameraParam()
    {
        float ax, ay, sizeX, sizeY;
        float x0, y0, shiftX, shiftY;
        int width, height;

        string data = udpReceiver.data;
        if (string.IsNullOrEmpty(data)){
            return;
        }
        // Debug.Log(data);
        //[3693.120191041108, 0.0, 1049.1639714569662, 0.0, 3680.442667586036, 1405.6081678063272, 0.0, 0.0, 1.0]

        // Decode the JSON string
        data = data.Replace("[", "");
        data = data.Replace("]", "");
        data = data.Replace(" ", "");
        string[] parameters = data.Split(',');
        for (int i = 0; i < parameters.Length; i++){
            parameters[i] = parameters[i].Replace(".", ",");
        }

        Debug.Log("parameters: " + parameters);
 
        ax = float.Parse(parameters[0]);
        ay = float.Parse(parameters[4]);
        x0 = float.Parse(parameters[2]);
        y0 = float.Parse(parameters[5]);
 
        width = 1920;
        height = 1080;
 
        sizeX = f * width / ax;
        sizeY = f * height / ay;
 
        //PlayerSettings.defaultScreenWidth = width;
        //PlayerSettings.defaultScreenHeight = height;
 
        shiftX = -(x0 - width / 2.0f) / width;
        shiftY = (y0 - height / 2.0f) / height;

        Debug.Log("sizeX, sizeY: " + sizeX +", " + sizeY);
        Debug.Log("shiftX, shiftY: " + shiftX +", " + shiftY);
 
        projView.sensorSize = new Vector2(sizeX, sizeY);     // in mm, mx = 1000/x, my = 1000/y
        projView.focalLength = f;                            // in mm, ax = f * mx, ay = f * my
        projView.lensShift = new Vector2(shiftX, shiftY);    // W/2,H/w for (0,0), 1.0 shift in full W/H in image plane

        Debug.Log("Proj intrinsics set...");

        this.enabled = false;
 
    }
}
