using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FootstepParticleController : MonoBehaviour
{

	public ParticleSystem partSys;

	public CharacterController charController;

	public float timeToNextRipple = 100f;

	public float rippleSpacing = 1f;

	
	// Update is called once per frame
	void Update ()
	{
		if (charController.velocity.magnitude != 0f)
		{
			if (Time.timeSinceLevelLoad >= timeToNextRipple)
			{
				// play ripple
				partSys.transform.position = new Vector3(transform.position.x, 0.001f, transform.position.z);
				partSys.Play();
				
				
				//Invoke("ResetParticleSystem", rippleSpacing * 0.95f);
				//Debug.Log("play ripple particles!");

				// set the next ripple timer
				timeToNextRipple = Time.timeSinceLevelLoad + rippleSpacing;
			}
		}
		else
		{
			//Debug.Log(charController.velocity.magnitude);
			timeToNextRipple = Time.timeSinceLevelLoad + rippleSpacing;
		}
	}

//	void ResetParticleSystem()
//	{
//		partSys.Clear();
//	}
}
