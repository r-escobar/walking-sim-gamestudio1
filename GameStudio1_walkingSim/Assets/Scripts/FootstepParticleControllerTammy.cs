using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FootstepParticleControllerTammy : MonoBehaviour
{

	public ParticleSystem partSys;

	public CharacterController charController;

	private float timer = 0f;

	private float HeadBobDownTime = 0f;

	void Start()
	{
		var HeadBobScript = GetComponentInChildren<HeadBob>();
		var HeadbobSpeed = HeadBobScript.bobbingSpeed;
		HeadBobDownTime = 1.5f * Mathf.PI / (100f * HeadbobSpeed);
	}
	
	// Update is called once per frame
	void Update ()
	{
		if (charController.velocity.magnitude != 0f)
		{
			timer += Time.deltaTime;
			if (timer >= HeadBobDownTime)
			{
				timer = 0f;
				partSys.transform.position = new Vector3(transform.position.x, 0.001f, transform.position.z);
				partSys.Play();
				
			}
		}
		else
		{
			timer = 0f;
		}
	}
}
