using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FootstepParticleControllerTammy : MonoBehaviour
{

	public ParticleSystem partSys;

	public void StartStep()
	{
		partSys.transform.position = new Vector3(transform.position.x, 0.001f, transform.position.z);
		partSys.Play();
		GetComponent<AudioSource>().Play();	
	}
	
}
