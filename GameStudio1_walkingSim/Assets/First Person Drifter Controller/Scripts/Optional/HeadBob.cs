// original by Mr. Animator
// adapted to C# by @torahhorse
// http://wiki.unity3d.com/index.php/Headbobber

using UnityEngine;
using System.Collections;

public class HeadBob : MonoBehaviour
{	
	public float bobbingSpeed = 0.25f; 
	public float bobbingAmount = 0.05f; 
	public float  midpoint = 0.6f; 
	
	public float timer = 0.0f;
	public float waveslice;
 
	void Update ()
	{ 
	    waveslice = 0.0f; 
	    float horizontal = Input.GetAxis("Horizontal"); 
	    float vertical = Input.GetAxis("Vertical"); 
	    
	    if (Mathf.Abs(horizontal) == 0f && Mathf.Abs(vertical) == 0f)
	    { 
	       //timer = 0.0f; 
	    } 
	    else
	    { 
	       waveslice = Mathf.Sin(timer); 
	       timer = timer + bobbingSpeed * 100f * Time.deltaTime; 
	       if (timer > Mathf.PI * 2f)
	       { 
	          timer = timer - (Mathf.PI * 2f); 
	       }

		    if (Mathf.Abs(timer - Mathf.PI * 1.5f) <= 0.02f)
		    {
			    StartFootStep();
		    }
	    } 
	    if (waveslice != 0f)
	    { 
	       float translateChange = waveslice * bobbingAmount; 
	       float totalAxes = Mathf.Abs(horizontal) + Mathf.Abs(vertical); 
		   totalAxes = Mathf.Clamp (totalAxes, 0.0f, 1.0f); 
	       translateChange = totalAxes * translateChange;
	       
	       Vector3 localPos = transform.localPosition;
	       localPos.y = midpoint + translateChange * Time.timeScale; 
	       transform.localPosition = localPos;
	    } 
	    else
	    { 
//	    	Vector3 localPos = transform.localPosition;
//	    	localPos.y = midpoint; 
//	    	transform.localPosition = localPos;
	    } 
	}

	void StartFootStep()
	{
		if(GetComponentInParent<FootstepParticleControllerTammy> () != null)
			GetComponentInParent<FootstepParticleControllerTammy> ().StartStep();
	}
}
