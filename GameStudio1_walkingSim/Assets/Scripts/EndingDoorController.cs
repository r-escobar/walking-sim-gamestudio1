﻿using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class EndingDoorController : MonoBehaviour {

	private bool hasPerformedAction = false;

	public float slideDuration = 2f;
	public float doorSwingDuration;
	public Vector3 targetDoorRot;
	public Transform doorParent;

	public float yMoveAmount = 2f;

	void Start()
	{
		//PerformUndeformAction();
	}
	
	
	public void PerformUndeformAction()
	{
		if(hasPerformedAction)
			return;
		hasPerformedAction = true;

		transform.DOMoveY(yMoveAmount, slideDuration).SetEase(Ease.InOutQuad).OnComplete(OpenDoor);
	}

	public void OpenDoor()
	{
		doorParent.DOLocalRotate(targetDoorRot, doorSwingDuration).SetEase(Ease.InOutQuad);
	}
}
