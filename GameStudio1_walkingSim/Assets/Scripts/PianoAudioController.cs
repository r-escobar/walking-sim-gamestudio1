using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PianoAudioController : MonoBehaviour
{
	public AudioSource audSrc;

	private bool hasPerformedAction = false;
	

	public void PerformUndeformAction()
	{
		if(hasPerformedAction)
			return;
		hasPerformedAction = true;
		audSrc.Play();
	}
}
