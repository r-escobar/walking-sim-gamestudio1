using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FootstepController : MonoBehaviour
{

	public ParticleSystem partSys;
	public AudioSource audSrc;

	public void StartStep()
	{
		partSys.transform.position = new Vector3(transform.position.x, 0.001f, transform.position.z);
		partSys.Play();
		audSrc.Play();	
	}
	
}
