using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUpObject : MonoBehaviour
{

	public Camera fpCamera;
	public float pickupRange = 1f;
	
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
				
		if (Input.GetMouseButtonDown(0))
		{
			Vector3 rayOrigin = fpCamera.ViewportToWorldPoint(new Vector3(0.5f, 0.5f, 0f));
			RaycastHit hit;
			if (Physics.Raycast(rayOrigin, fpCamera.transform.forward, out hit, pickupRange))
			{	
//				if(hit.collider != null)
//					Debug.Log("player clicked " + hit.collider.name);
				if (hit.collider.tag == "CanPickup")
				{
					Debug.Log("player is trying to pick up a " + hit.collider.name);
				}
			}

		}
	}
}
