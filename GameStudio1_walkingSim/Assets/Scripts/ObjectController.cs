using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectController : MonoBehaviour
{

	public Vector3 startingPos;

	public bool inspectMode = false;
	
//	void OnMouseDown()
//	{
//		Debug.Log("player clicked " + gameObject.name);
//	}
	
	// Use this for initialization
	void Start ()
	{
		startingPos = transform.position;
	}
	
	// Update is called once per frame
	void Update () {
		if (inspectMode)
		{
			
		}
	}

	public void ResetObject()
	{
		
	}
}
