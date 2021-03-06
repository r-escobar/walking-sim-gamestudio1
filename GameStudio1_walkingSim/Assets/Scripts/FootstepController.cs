﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FootstepController : MonoBehaviour
{

	public ParticleSystem partSys;
	public AudioSource audSrc;

	private bool footstepPlayed = false;
	public float footstepIgnoreTime = 0.5f;

	private float startingPitch;
	public float pitchVariation = 0.05f;

	void Start()
	{
		startingPitch = audSrc.pitch;
	}
	
	public void StartStep()
	{
		// only play the footstep sound and particle if we haven't already played a footstep recently
		if (footstepPlayed)
			return;

		//Debug.Log("footstep played!");
		
		// move the particle system to the player's feet (0.001f so it's not in the ground)
		
		RaycastHit hit;
		if (Physics.Raycast(transform.position, Vector3.down, out hit, 5f))
		{
			if (hit.collider.tag == "Ground")
			{
				footstepPlayed = true;
				
				partSys.transform.position = new Vector3(transform.position.x, 0.001f, transform.position.z);
		
				partSys.Play();
		
				audSrc.pitch = startingPitch + Random.Range(-pitchVariation, pitchVariation);
				audSrc.Play();

				// after footstepIgnoreTime seconds, reset the footstepPlayed variable
				Invoke("ResetFootstepPlayed", footstepIgnoreTime);
			}
		}
	}

	void ResetFootstepPlayed()
	{
		footstepPlayed = false;
	}
}
