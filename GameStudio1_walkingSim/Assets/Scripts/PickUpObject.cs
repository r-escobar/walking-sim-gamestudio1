using System.Collections;
using System.Collections.Generic;
using System.Net.Mime;
using UnityEngine;
using UnityEngine.UI;

public class PickUpObject : MonoBehaviour
{
	
	public Camera fpCamera;
	public Image cursorImage;
	public MouseLook mouseLookX;
	public MouseLook mouseLookY;
	public FirstPersonDrifter fpController;
	public BlurFader blurFader;

	public float pickupRange = 1f;
	public Vector3 inspectPos = new Vector3(0f, 0f, 2f);
	public float minInspectDuration = 0.25f;
	
	// Update is called once per frame
	void Update () {
				
		if (Input.GetMouseButtonDown(0))
		{
			Vector3 rayOrigin = fpCamera.ViewportToWorldPoint(new Vector3(0.5f, 0.5f, 0f));
			RaycastHit hit;
			if (Physics.Raycast(rayOrigin, fpCamera.transform.forward, out hit, pickupRange))
			{	
				if (hit.collider.tag == "CanPickup")
				{
					ObjectController objScript = hit.collider.gameObject.GetComponent<ObjectController>();
					PulsatingGlow glowScript = hit.collider.gameObject.GetComponent<PulsatingGlow>();

					if (objScript)
					{
						if (!objScript.inspectMode)
						{
							//Debug.Log("picking up " + objScript.gameObject.name);
							objScript.StartInspectingObject(inspectPos, transform);
							mouseLookX.SetSensitivity(0f);
							mouseLookY.SetSensitivity(0f);
							fpController.FreezeMovement();
							blurFader.BlurFadeIn();
							if(cursorImage)
								cursorImage.enabled = false;

							if (glowScript)
							{
								glowScript.StopGlowing();
							}
						}
						else if(objScript.inspectTimer >= minInspectDuration)
						{
							//Debug.Log("putting down " + objScript.gameObject.name);
							objScript.StopInspectingObject();
							mouseLookX.ResetSensitivity();
							mouseLookY.ResetSensitivity();
							fpController.UnfreezeMovement();
							blurFader.BlurFadeOut();
							if(cursorImage)
								cursorImage.enabled = true;
						}
					}
					else
					{
						Debug.LogError("object does not have an attached ObjectController!");
					}
				}
			}
		}
	}
}
