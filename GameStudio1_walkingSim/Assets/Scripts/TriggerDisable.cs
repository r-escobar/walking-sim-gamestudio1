using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerDisable : MonoBehaviour
{

	public bool startDisabled = true;

	private MeshDeformerTest deformerScript;
	
	// Use this for initialization
	void Start ()
	{

		deformerScript = GetComponent<MeshDeformerTest>();
		if (startDisabled && deformerScript)
			deformerScript.enabled = false;

	}
	
	// Update is called once per frame
	void Update () {
		
	}

	private void OnTriggerEnter(Collider other)
	{
		if (other.tag == "PlayerTrigger")
		{
			if (deformerScript)
				deformerScript.enabled = true;
		}
	}

	private void OnTriggerExit(Collider other)
	{
		if (other.tag == "PlayerTrigger")
		{
			if (deformerScript)
				deformerScript.enabled = false;
		}
	}
}
