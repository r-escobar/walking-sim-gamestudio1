using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FootstepController : MonoBehaviour
{

	public ParticleSystem partSys;
	public AudioSource audSrc;

	private bool footstepPlayed = false;
	public float footstepIgnoreTime = 0.5f;
	
	public void StartStep()
	{
		// only play the footstep sound and particle if we haven't already played a footstep recently
		if (footstepPlayed)
			return;

		footstepPlayed = true;
		//Debug.Log("footstep played!");
		
		// move the particle system to the player's feet (0.001f so it's not in the ground)
		partSys.transform.position = new Vector3(transform.position.x, 0.001f, transform.position.z);
		
		partSys.Play();
		audSrc.Play();

		// after footstepIgnoreTime seconds, reset the footstepPlayed variable
		Invoke("ResetFootstepPlayed", footstepIgnoreTime);
	}

	void ResetFootstepPlayed()
	{
		footstepPlayed = false;
	}
}
