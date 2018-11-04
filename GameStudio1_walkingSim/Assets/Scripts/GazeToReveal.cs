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

				
				
				MeshDeformerTest glitchScript = hit.collider.gameObject.GetComponent<MeshDeformerTest>();

				if (glitchScript)
				{
					glitchScript.GazeAtObject();
					
				}
				
				if (cursorScript)
				{
					if (hit.collider.tag == "CanPickup")
					{
						cursorScript.EnlargeCursor();

					} else if (hit.collider.tag == "RevealedByGaze")
					{
						if (glitchScript.deformModifier > 0f)
						{
							cursorScript.BrightenCursor();
						}
						else
						{
							cursorScript.DimCursor();
						}
					}
				}
			} else
			{
				if (cursorScript)
				{
					cursorScript.ResetCursorSize();
					cursorScript.DimCursor();
				}
			}
			
		} else
		{
			if (cursorScript)
			{
				cursorScript.ResetCursorSize();
				cursorScript.DimCursor();
			}
		}
	}
}
