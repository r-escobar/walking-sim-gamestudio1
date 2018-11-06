using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeshDeformerTest : MonoBehaviour {

	public float scaleX = 1f;
	public float scaleY = 1f;
 	
	//Vector3[] vertPositions;

	[Range(0f,1f)]
	public float triangleGapProb = 0.1f;
	[Range(0f,1f)]
	public float triangleDeformProb = 0.1f;
	[Range(0f,1f)]
	public float quickDeformChance = 0.5f;

	public bool triangleGapDeformation = true;
	public bool triangleReassignment = false;

	[Range(0f,1f)]
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

	public bool stayUndeformed = true;
	public bool changeColorOnGaze = true;
	public float colorMod = 0.5f;

	private MeshRenderer mRend;
	private float startingColorVal;
	//private float curColorVal;
	public float colorSmoothing = 0.5f;


	public AudioSource glitchAudio;
	public bool randomizeVolume = true;
	public  float volumeControl = 1f;
	public float volumeAdjustSpeed = 0.002f;
	public float maxVolume = 1f;
	public float volumeDisturbanceAmount = 0.1f;

	public MeshDeformerTest pairedMeshScript;
	public GameObject undeformActionTarget;

	public int numDependentObjects;
	public MeshDeformerTest giantUnderformTarget;
	private bool hasBeenUndeformed = false;
	private int maxObjects;

	public bool highPerformanceMode = false;
	public int highPerformanceNumTris = 300;
	
	void Start()
	{
		//vertPositions = GetComponent<MeshFilter> ().mesh.vertices;
		baseTriList = GetComponent<MeshFilter>().mesh.triangles;
		mRend = GetComponent<MeshRenderer>();
//		if(mRend.material.HasProperty("_yPosHigh"))
//			startingColorVal = mRend.material.GetFloat("_yPosHigh");
		//curColorVal = startingColorVal;

		maxObjects = numDependentObjects;
		
		if (!glitchAudio)
			glitchAudio = GetComponent<AudioSource>();
	}
	
	public void GazeAtObject()
	{
		stopGazingTime = Time.timeSinceLevelLoad + stopGazingDelay;
		isBeingGazedAt = true;
	}
	
	
	void Update()
	{

		if (deformModifier == 0f && stayUndeformed && hasBeenUndeformed)
			return;
		
		if (!hasBeenUndeformed && deformModifier == 0f)
		{
			if(undeformActionTarget)
				undeformActionTarget.SendMessage("PerformUndeformAction");
			if (giantUnderformTarget)
				giantUnderformTarget.numDependentObjects--;
						
			hasBeenUndeformed = true;
		}
		
		if (maxObjects == 0)
		{
			volumeControl = Mathf.Clamp(volumeControl, 0f, maxVolume);
			if (glitchAudio)
			{
				if(randomizeVolume)
					glitchAudio.volume = Random.value * volumeControl;
				else
					glitchAudio.volume = volumeControl;
			}
		
			if (Time.timeSinceLevelLoad > stopGazingTime)
				isBeingGazedAt = false;

			if (isBeingGazedAt)
			{
				deformModifier -= deformModifier * Time.deltaTime * undeformSmoothing;
			
				if(volumeControl > 0f)
					volumeControl -= Time.deltaTime * volumeAdjustSpeed * maxVolume;

				if (deformModifier <= 0.001f)
				{
					deformModifier = 0f;
					if (!hasBeenUndeformed)
					{
						if(undeformActionTarget)
							undeformActionTarget.SendMessage("PerformUndeformAction");
						if (giantUnderformTarget)
							giantUnderformTarget.numDependentObjects--;
						
						hasBeenUndeformed = true;
					}
					//curColorVal += (startingColorVal - curColorVal) * Time.deltaTime * colorSmoothing;
					
				}
				else
				{
					//curColorVal += ((startingColorVal * colorMod) - curColorVal) * Time.deltaTime * colorSmoothing;
				}
			}
			else 
			{
				//curColorVal += (startingColorVal - curColorVal) * Time.deltaTime * colorSmoothing;
				if (!stayUndeformed)
				{
					//deformModifier += (1f - deformModifier) * Time.deltaTime * deformSmoothing;
					if(volumeControl < maxVolume)
						volumeControl += Time.deltaTime * volumeAdjustSpeed * maxVolume;
				
					deformModifier += Time.deltaTime * deformSpeed;
					if (deformModifier > 1f)
						deformModifier = 1f;
				}
			}
		}
		else
		{
			deformModifier = numDependentObjects * 1.0f / maxObjects;
		}
		

		if (pairedMeshScript)
			pairedMeshScript.deformModifier = deformModifier;
		
//		if (curColorVal < startingColorVal * colorMod)
//			curColorVal = startingColorVal * colorMod;
//		else if (curColorVal > startingColorVal)
//			curColorVal = startingColorVal;
			              
		//mRend.material.SetFloat("_yPosHigh", curColorVal);
		
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

				int size = modifiedTriList.Count;
				if (highPerformanceMode)
					size = Mathf.Min(highPerformanceNumTris, size);
				
				for (int i = 0; i < size; i++) {
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
