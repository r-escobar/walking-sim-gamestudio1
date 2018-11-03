using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GazeToReveal : MonoBehaviour {

	public Camera fpCamera;
	public float gazeRange = 5f;

	public CursorDotController cursorScript;

	void FixedUpdate()
	{
		RaycastHit hit;
		if (Physics.Raycast(transform.position, fpCamera.transform.forward, out hit, gazeRange))
		{
			if (hit.collider.tag == "CanPickup" || hit.collider.tag == "RevealedByGaze")
			{
				
				if(cursorScript && hit.collider.tag == "CanPickup")
					cursorScript.EnlargeCursor();
				
				MeshDeformerTest glitchScript = hit.collider.gameObject.GetComponent<MeshDeformerTest>();

				if (glitchScript)
				{
					glitchScript.GazeAtObject();
				}
			}
			
		} else
		{
			if(cursorScript)
				cursorScript.ResetCursorSize();
		}
	}
}
