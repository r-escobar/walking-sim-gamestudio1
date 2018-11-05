using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClumpyLeaves : MonoBehaviour {

    public List<float> scales;
    public List<GameObject> leafObjs;

	// Use this for initialization
	void Start () {
		foreach(GameObject leaf in leafObjs)
        {
            int scaleIndex = Random.Range(0, scales.Count);
            leaf.transform.localScale = Vector3.one * scales[scaleIndex];
            scales.RemoveAt(scaleIndex);
        }
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
