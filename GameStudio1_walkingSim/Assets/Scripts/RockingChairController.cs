using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class RockingChairController : MonoBehaviour
{

	public Vector3 backRot;
	public Vector3 frontRot;

	public float tiltTime = 1f;
	
	private bool hasPerformedAction = false;
	

	public void RockBackward()
	{
		transform.DORotate(backRot, tiltTime).SetEase(Ease.InOutSine).OnComplete(RockForward);
	}

	public void RockForward()
	{
		transform.DORotate(frontRot, tiltTime).SetEase(Ease.InOutSine).OnComplete(RockBackward);
	}
	
	public void PerformUndeformAction()
	{
		if(hasPerformedAction)
			return;
		hasPerformedAction = true;
		
		RockBackward();
	}
}
