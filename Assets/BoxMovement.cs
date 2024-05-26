using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using Palmmedia.ReportGenerator.Core.Parser.Analysis;
using Unity.VisualScripting;
using UnityEngine;

public class BoxMovement : MonoBehaviour
{
    public UDPReceiver udpReceiver;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        string data = udpReceiver.data;
        if (string.IsNullOrEmpty(data)){
            return;
        }
        // Debug.Log(data);
        //(array([ 0.04655706,  0.87039364, -0.49015034, -0.00466955]), array([-0.99192917, -0.01764883, -0.1255589 ,  0.28824726]), array([-0.11793625,  0.49204007,  0.86254716,  0.54929401]), array([0., 0., 0., 1.]))

        // Decode the JSON string
        data = data.Replace("[", "");
        data = data.Replace("]", "");
        data = data.Replace(" ", "");
        string[] ding = data.Split(',');

        // Use the transformationMatrix as needed
        // For example, you can convert it to a Unity Matrix4x4
        Matrix4x4 matrix = new Matrix4x4();
        for (int i = 0; i < 16; i++)
        {
            matrix[i/4, i%4] = float.Parse(ding[i].Replace(".", ","));
        }

        Matrix4x4Helper helper = new Matrix4x4Helper();
        Quaternion quaternion = helper.ExtractRotationFromMatrix(ref matrix);
        // Vector3 euler = quaternion.eulerAngles;
        // quaternion = Quaternion.Euler(euler);
        Vector3 translation = helper.ExtractTranslationFromMatrix(ref matrix);

        gameObject.transform.position = translation;
        gameObject.transform.rotation = quaternion;
    }
}
