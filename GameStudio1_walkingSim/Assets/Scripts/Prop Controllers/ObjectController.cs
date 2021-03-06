﻿using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class ObjectController : MonoBehaviour
{

	public Vector3 startingPos;
	public Quaternion startingRot;
	public Transform startingParent;

	public bool inspectMode = false;

	public float rotateSpeed = 0.1f;
	public float curRotateSpeed = 0f;
	public float tgtRotateSpeed;
	public float rotationSmoothing = 100f;

	private int defaultLayer;
	public int unblurredLayer = 9;
	
	private Vector2 axis2;

	public float inspectTimer = 0f;

	public bool playAudioOnPickup = false;
	public AudioSource audSrc;
	private float startingVolume;
	
	// Use this for initialization
	void Start ()
	{
		if (audSrc)
			startingVolume = audSrc.volume;
		startingPos = transform.localPosition;
		startingRot = transform.localRotation;
		if (transform.parent)
			startingParent = transform.parent;

		defaultLayer = gameObject.layer;
	}
	
	// Update is called once per frame
	void Update () {
		if (inspectMode)
		{
			inspectTimer += Time.deltaTime;
			
			Vector2 mouseDir = new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));
			
			if (mouseDir.magnitude != 0f)
				axis2 = Vector2.Perpendicular((mouseDir));

			if (mouseDir.magnitude == 0)
				tgtRotateSpeed = 0f;
			else
				tgtRotateSpeed = rotateSpeed;
			
			curRotateSpeed += (tgtRotateSpeed - curRotateSpeed) * 0.1f * Time.deltaTime * rotationSmoothing;

			transform.RotateAround((axis2), Time.deltaTime * curRotateSpeed);
		}
	}

	public void StartInspectingObject(Vector3 inspectPos, Transform newParent)
	{
		SetLayer(gameObject, unblurredLayer);
		inspectMode = true;
		transform.parent = newParent;
		transform.localRotation = Quaternion.identity;
		transform.localPosition = inspectPos;
		if (playAudioOnPickup)
		{
			PlayAudio();
		}
	}
	
	public void StopInspectingObject()
	{
		SetLayer(gameObject, 0);
		inspectMode = false;
		transform.parent = startingParent;
		transform.localRotation = startingRot;
		transform.localPosition = startingPos;
		inspectTimer = 0f;
		if (audSrc)
		{
			if (audSrc.isPlaying)
				audSrc.DOFade(0f, 0.25f);
		}
	}

	public void PlayAudio()
	{
		audSrc.time = 0f;
		audSrc.DOFade(1f, 0.5f);
		audSrc.Play();
	}
	
	public void SetLayer(GameObject go, int layerNumber)
	{
		go.layer = layerNumber;
		foreach (Transform trans in go.GetComponentsInChildren<Transform>(true))
		{
			trans.gameObject.layer = layerNumber;
		}
	}
}
