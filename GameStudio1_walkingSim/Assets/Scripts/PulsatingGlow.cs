using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PulsatingGlow : MonoBehaviour
{

	public float glowAmount = 0.1f;
	private float brightnessMin;
	private float brightnessMax;

	private float amp;
	public float glowSpeed = 2f;
	
	public MeshRenderer mRend;
	
	private float startingBrightness;
	
	// Use this for initialization
	void Start () {
		float yPosHigh = mRend.material.GetFloat("_yPosHigh");
		brightnessMin = yPosHigh * (1f - glowAmount);
		brightnessMax = yPosHigh * (1f + glowAmount);

		startingBrightness = yPosHigh;
		amp = glowAmount * yPosHigh;
	}
	
	// Update is called once per frame
	void Update ()
	{
		float theta = Time.timeSinceLevelLoad * glowSpeed;
		float brightnessChange = amp * Mathf.Sin(theta);
		
		mRend.material.SetFloat("_yPosHigh", startingBrightness + brightnessChange);

	}
}
