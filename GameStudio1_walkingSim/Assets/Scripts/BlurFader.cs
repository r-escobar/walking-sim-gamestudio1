using System.Collections;
using System.Collections.Generic;
using SuperBlur;
using UnityEngine;

public class BlurFader : MonoBehaviour
{

	public float maxBlur = 1f;
	public float blurSpeed = 50f;
	
	private float tgtBlur;
	private float curBlur;

	public SuperBlurBase blurScript;

	public void BlurFadeIn()
	{
		tgtBlur = maxBlur;
	}

	public void BlurFadeOut()
	{
		tgtBlur = 0f;
	}
	
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update ()
	{
		curBlur += (tgtBlur - curBlur) * 0.5f * Time.deltaTime * blurSpeed;
		blurScript.interpolation = curBlur;
	}
}
