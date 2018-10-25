using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GazeToReveal : MonoBehaviour {

	public Camera fpCamera;
	public float gazeRange = 5f;
	
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void FixedUpdate()
	{
		RaycastHit hit;
		if (Physics.Raycast(transform.position, fpCamera.transform.forward, out hit, gazeRange))
		{
			if (hit.collider.tag == "CanPickup")
			{
				MeshDeformerTest glitchScript = hit.collider.gameObject.GetComponent<MeshDeformerTest>();

				if (glitchScript)
				{
					glitchScript.GazeAtObject();
				}
			}
		}
	}
}
