using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticlePersonController : MonoBehaviour
{

	public ParticleSystem headParticlesInner;
	public ParticleSystem headParticlesOuter;
	public ParticleSystem bodyParticlesInner;
	public ParticleSystem bodyParticlesOuter;


	private bool hasPerformedAction = false;


	public void PerformUndeformAction()
	{
		if(hasPerformedAction)
			return;
		
		PlayParticleSystems();
	}

	public void PlayParticleSystems()
	{
		headParticlesInner.Play();
		headParticlesOuter.Play();
		bodyParticlesInner.Play();
		bodyParticlesOuter.Play();
	}

}
