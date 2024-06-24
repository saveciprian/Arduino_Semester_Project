using UnityEngine;
using System.Collections;

public class moveGround : MonoBehaviour 
{
	float speed = .5f;
	public ArduinoController _controller;
	private float heartRateThreshold = 50f;
	void Update ()
	{
		if (_controller.avgBPM < heartRateThreshold)
		{
			speed = 0.1f;
		}
		else
		{
			speed = 0.5f;
		}
		
		float offset = Time.time * speed;                             
		GetComponent<Renderer>().material.mainTextureOffset = new Vector2(0, -offset); 
	}
}
