using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class TeddyBearController : MonoBehaviour {

	public AudioSource audSrc;

	private bool hasPerformedAction = false;

	private float colorVal;
	public float tgtColorVal = 3f;
	public float tweenTime = 0.5f;

	public MeshRenderer mRend;
	
	// Use this for initialization
	void Start () {
		colorVal = mRend.material.GetFloat("_yPosHigh");
	}
	
	// Update is called once per frame
	void Update () {
		if(hasPerformedAction)
			mRend.material.SetFloat("_yPosHigh", colorVal);
	}

	public void PerformUndeformAction()
	{
		if(hasPerformedAction)
			return;
		hasPerformedAction = true;

		Invoke("PlayAudio", tweenTime * 0.5f);

	}

	void PlayAudio()
	{
		audSrc.Play();
		DOTween.To(() => colorVal, x => colorVal = x, tgtColorVal, tweenTime).SetEase(Ease.OutExpo);


	}
}
