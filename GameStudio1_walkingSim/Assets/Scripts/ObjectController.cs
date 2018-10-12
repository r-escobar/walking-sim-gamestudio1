using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectController : MonoBehaviour
{

	public Vector3 startingPos;
	public Quaternion startingRot;
	public Transform startingParent;

	public bool inspectMode = false;

	public float rotateSpeed = 0.1f;
	public float curRotateSpeed = 0f;
	public float tgtRotateSpeed;
	public float rotationSmoothing = 100f;

	private int defaultLayer;
	public int unblurredLayer = 9;
	
	private Vector2 axis2;

	public float inspectTimer = 0f;
	
	// Use this for initialization
	void Start ()
	{
		startingPos = transform.localPosition;
		startingRot = transform.localRotation;
		if (transform.parent)
			startingParent = transform.parent;

		defaultLayer = gameObject.layer;
	}
	
	// Update is called once per frame
	void Update () {
		if (inspectMode)
		{
			inspectTimer += Time.deltaTime;
			
			Vector2 mouseDir = new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));
			
			if (mouseDir.magnitude != 0f)
				axis2 = Vector2.Perpendicular((mouseDir));

			if (mouseDir.magnitude == 0)
				tgtRotateSpeed = 0f;
			else
				tgtRotateSpeed = rotateSpeed;
			
			curRotateSpeed += (tgtRotateSpeed - curRotateSpeed) * 0.1f * Time.deltaTime * rotationSmoothing;

			transform.RotateAround((axis2), Time.deltaTime * curRotateSpeed);
		}
	}

	public void StartInspectingObject(Vector3 inspectPos, Transform newParent)
	{
		SetLayer(gameObject, unblurredLayer);
		inspectMode = true;
		transform.parent = newParent;
		transform.localRotation = Quaternion.identity;
		transform.localPosition = inspectPos;
	}
	
	public void StopInspectingObject()
	{
		SetLayer(gameObject, 0);
		inspectMode = false;
		transform.parent = startingParent;
		transform.localRotation = startingRot;
		transform.localPosition = startingPos;
		inspectTimer = 0f;
	}
	
	public void SetLayer(GameObject go, int layerNumber)
	{
		go.layer = layerNumber;
		foreach (Transform trans in go.GetComponentsInChildren<Transform>(true))
		{
			trans.gameObject.layer = layerNumber;
		}
	}
}
