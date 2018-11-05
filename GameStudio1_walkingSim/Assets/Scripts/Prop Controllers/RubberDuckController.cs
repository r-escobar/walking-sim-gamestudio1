using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RubberDuckController : MonoBehaviour
{

	public ObjectController objScript;
	
	private bool hasPerformedAction = false;

	
	public void PerformUndeformAction()
	{
		if(hasPerformedAction)
			return;
		hasPerformedAction = true;

		objScript.playAudioOnPickup = true;
		if(objScript.inspectMode)
			objScript.PlayAudio();
	}
}
