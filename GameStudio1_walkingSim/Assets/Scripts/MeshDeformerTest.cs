using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeshDeformerTest : MonoBehaviour {

	public float scaleX = 1f;
	public float scaleY = 1f;
 	
	Vector3[] vertPositions;

	[Range(0f,1f)]
	public float triangleGapProb = 0.1f;
	[Range(0f,1f)]
	public float triangleDeformProb = 0.1f;
	[Range(0f,1f)]
	public float quickDeformChance = 0.5f;

	public bool triangleGapDeformation = true;
	public bool triangleReassignment = false;

	public float deformModifier = 1f;

	public float quickDeformDelayMin = 0.001f;
	public float quickDeformDelayMax = 0.05f;
	public float longDeformDelayMin = 0.75f;
	public float longDeformDelayMax = 1.5f;
	
	private float timeToNextGapCalc = -1;
	private int[] baseTriList;
	private List<int> modifiedTriList;

	public float stopGazingDelay = 0.5f;
	private float stopGazingTime;
	private bool isBeingGazedAt = false;

	public float deformSmoothing = 10f;
	public float undeformSmoothing = 100f;
	public float deformSpeed = 5f;		
	
	void Start()
	{
		vertPositions = GetComponent<MeshFilter> ().mesh.vertices;
		baseTriList = GetComponent<MeshFilter>().mesh.triangles;
	}
	
	public void GazeAtObject()
	{
		stopGazingTime = Time.timeSinceLevelLoad + stopGazingDelay;
		isBeingGazedAt = true;

		//Debug.Log("gazing at " + gameObject.name);
	}
	
	void Update()
	{
		if (Time.timeSinceLevelLoad > stopGazingTime)
			isBeingGazedAt = false;

		if (isBeingGazedAt)
		{
			deformModifier -= deformModifier * Time.deltaTime * undeformSmoothing;

			if (deformModifier < 0f)
				deformModifier = 0f;
		}
		else
		{
			//deformModifier += (1f - deformModifier) * Time.deltaTime * deformSmoothing;
			deformModifier += Time.deltaTime * deformSpeed;
			if (deformModifier > 1f)
				deformModifier = 1f;
		}
		
		if (Time.timeSinceLevelLoad > timeToNextGapCalc)
		{
			if (Random.value <= (quickDeformChance))
			{
				timeToNextGapCalc += Random.Range(quickDeformDelayMin, quickDeformDelayMax);
			}
			else
			{
				timeToNextGapCalc += Random.Range(longDeformDelayMin, longDeformDelayMax);
			}
			
			Mesh mesh = GetComponent<MeshFilter> ().mesh;

			modifiedTriList = new List<int>();

			if (triangleGapDeformation)
			{
				for (int i = 0; i < baseTriList.Length; i += 3)
				{
					if (Random.value > (triangleGapProb * deformModifier))
					{
						modifiedTriList.Add(baseTriList[i]);
						modifiedTriList.Add(baseTriList[i + 1]);
						modifiedTriList.Add(baseTriList[i + 2]);
					}
				}
			}


			if (triangleReassignment)
			{
				if(!triangleGapDeformation)
					modifiedTriList = new List<int>(baseTriList);
				
				for (int i = 0; i < modifiedTriList.Count; i++) {
					if (Random.value <= (triangleDeformProb * deformModifier))
					{
						int temp = modifiedTriList[i];
						int randomIndex = Random.Range(i, modifiedTriList.Count);
						modifiedTriList[i] = modifiedTriList[randomIndex];
						modifiedTriList[randomIndex] = temp;
					}
				}
			}
			
		
			mesh.triangles = modifiedTriList.ToArray();
			mesh.RecalculateBounds ();
			mesh.RecalculateNormals ();
		}

	}
}
