using System.Collections;
using UnityEngine;
using TMPro;
using System.IO.Ports;
using System.Threading;
using System;

public class ArduinoConnector : MonoBehaviour
{
    public string portName = "COM8"; // Replace with your Arduino's COM port
    public int baudRate = 115200;
    // public TextMeshProUGUI distanceText; // UI Text element to display the distance

    private SerialPort serialPort;
    private Thread readThread;
    private bool isRunning = true;
    private string dataString;
    public float position;
    public float movementDirection;
    public float minimumDistance, maximumDistance;

    void Start()
    {
        serialPort = new SerialPort(portName, baudRate);
        serialPort.ReadTimeout = 500;

        try
        {
            serialPort.Open();
            readThread = new Thread(ReadData);
            readThread.Start();
        }
        catch (System.Exception e)
        {
            Debug.LogError("Failed to open serial port: " + e.Message);
        }
    }

    void Update()
    {
        if (!string.IsNullOrEmpty(dataString))
        {
            ProcessResponse(dataString);
            dataString = null; // Reset the dataString to avoid processing the same data multiple times
        }
    }

    public void PlayLoseSound()
    {
        serialPort.WriteLine("B" + "1");
    }

    void OnApplicationQuit()
    {
        isRunning = false;
        if (readThread != null && readThread.IsAlive)
        {
            readThread.Join();
        }
        if (serialPort != null && serialPort.IsOpen)
        {
            serialPort.Close();
        }
    }

    private void ReadData()
    {
        while (isRunning && serialPort != null && serialPort.IsOpen)
        {
            try
            {
                dataString = serialPort.ReadLine();
            }
            catch (TimeoutException) { }
        }
    }

    void ProcessResponse(string response)
    {
        // Example response format: "Distance=123.45"
        string[] keyValue = response.Split('=');
        if (keyValue.Length == 2 && keyValue[0].Trim() == "Distance")
        {
            float distance;
            if (float.TryParse(keyValue[1].Trim(), out distance))
            {

                // if (distance > maximumDistance)
                // {
                //     distance = (minimumDistance + maximumDistance)/2;
                // }
                position = Mathf.Clamp(distance, minimumDistance, maximumDistance);
                movementDirection = (Mathf.InverseLerp(minimumDistance, maximumDistance, position) * 2) - 1;
                // distanceText.text = "Distance: " + distance.ToString("F2") + " cm";
                
                Debug.Log("Distance: " + distance);
                Debug.Log("Movement Direction: " + movementDirection);
            }
        }
    }
}
