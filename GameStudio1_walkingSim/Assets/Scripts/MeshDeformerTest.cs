using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeshDeformerTest : MonoBehaviour {

	public float scaleX = 1f;
	public float scaleY = 1f;
 	
	Vector3[] vertPositions;

	public float triangleGapProb = 0.1f;
	private float timeToNextGapCalc = -1;

	private int[] baseTriList;
	private List<int> modifiedTriList;
	void Start()
	{
		vertPositions = GetComponent<MeshFilter> ().mesh.vertices;
		baseTriList = GetComponent<MeshFilter>().mesh.triangles;
	}
	
	void Update()
	{
		if (Time.timeSinceLevelLoad > timeToNextGapCalc)
		{
			//timeToNextGapCalc += Random.Range(0.001f, 0.075f);
			timeToNextGapCalc += 1f;
			
			Mesh mesh = GetComponent<MeshFilter> ().mesh;

//			modifiedTriList = new List<int>();
//			for (int i = 0; i < baseTriList.Length; i += 3)
//			{
//				if (Random.Range(0f, 1f) > triangleGapProb)
//				{
//					modifiedTriList.Add(baseTriList[i]);
//					modifiedTriList.Add(baseTriList[i + 1]);
//					modifiedTriList.Add(baseTriList[i + 2]);
//				}
//			}


			modifiedTriList = new List<int>(baseTriList);
			for (int i = 0; i < modifiedTriList.Count; i++) {
				if (Random.Range(0f, 1f) > triangleGapProb)
				{
					int temp = modifiedTriList[i];
					int randomIndex = Random.Range(i, modifiedTriList.Count);
					modifiedTriList[i] = modifiedTriList[randomIndex];
					modifiedTriList[randomIndex] = temp;
				}
			}
			
//		Vector3[] vertices = mesh.vertices;
// 
//		float invertscaleX =1-(scaleX-1);
//		float invertscaleY =1-(scaleY-1);
// 
// 
//		
//		for (int i=0; i<vertices.Length; i++)
//		{
//			vertices[i].x = vertPositions[i].x - Mathf.PerlinNoise(Time.deltaTime + vertPositions[i].x * scaleX, Time.deltaTime + vertPositions[i].y * scaleY);
//			vertices[i].y = vertPositions[i].y + Mathf.PerlinNoise(Time.deltaTime + vertPositions[i].x * scaleX, Time.deltaTime + vertPositions[i].y * scaleY);
//			vertices[i].z = vertPositions[i].z + Mathf.PerlinNoise(Time.deltaTime + vertPositions[i].x * scaleX, Time.deltaTime + vertPositions[i].y * scaleY);
//
//		}
		
			mesh.triangles = modifiedTriList.ToArray();
			mesh.RecalculateBounds ();
			mesh.RecalculateNormals ();
		}

	}
}
