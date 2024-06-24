using UnityEngine;
using System.Collections;

public class moveHurdles : MonoBehaviour
{
	public float objectSpeed = -20f;
	public ArduinoController _controller;
	private float heartRateThreshold = 50f;

	void Start()
	{
		_controller = GameObject.FindWithTag("Arduino").GetComponent<ArduinoController>();
	}

	void Update () {
		if (_controller.avgBPM < heartRateThreshold)
		{
			objectSpeed = -10f;
		}
		else
		{
			objectSpeed = -20f;
		}

		transform.Translate(new Vector3(0, objectSpeed, 0)*Time.deltaTime);
		
	}
}
