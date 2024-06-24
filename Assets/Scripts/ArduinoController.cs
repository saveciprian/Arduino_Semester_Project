using System.Collections;
using UnityEngine;
using UnityEngine.Networking;

public class ArduinoController : MonoBehaviour
{
    private string arduinoIP = "192.168.0.107"; 
    private string url;

    public float avgBPM;
    void Start()
    {
        url = "http://" + arduinoIP + "/"; // URL of the Arduino web server
        StartCoroutine(GetBPMData());
    }

    IEnumerator GetBPMData()
    {
        while (true)
        {
            using (UnityWebRequest webRequest = UnityWebRequest.Get(url))
            {
                yield return webRequest.SendWebRequest();

                if (webRequest.result == UnityWebRequest.Result.ConnectionError || webRequest.result == UnityWebRequest.Result.ProtocolError)
                {
                    Debug.LogError("Error: " + webRequest.error);
                }
                else
                {
                    string response = webRequest.downloadHandler.text;
                    Debug.Log("Response: " + response);
                    ProcessResponse(response);
                }
            }
            yield return new WaitForSeconds(1); // Request data every second
        }
    }

    void ProcessResponse(string response)
    {
        // Example response format: "Avg BPM=75"
        string[] keyValue = response.Split('=');
        if (keyValue.Length == 2 && keyValue[0].Trim() == "Avg BPM")
        {
            if (float.TryParse(keyValue[1].Trim(), out avgBPM))
            {
                Debug.Log("Average BPM: " + avgBPM);
            }
        }
    }
}