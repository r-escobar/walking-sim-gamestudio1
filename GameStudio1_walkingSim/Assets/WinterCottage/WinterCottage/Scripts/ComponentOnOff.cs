using UnityEngine; 
using System.Collections.Generic; 

public class ComponentOnOff : MonoBehaviour { 
	public List<Light> ComponentList = new List<Light>(); 
	public AnimationClip SwitchLightOff; 
	public AnimationClip SwitchLightOn; 
	private enum State { 
		on, 
		off 
	} 

	private State state; 

	void Start() { 
		state = ComponentOnOff.State.on; 

	} 

	public void OnMouseUp() { 
		if (state == ComponentOnOff.State.on) 
			TurnOff(); 
		else 
			TurnOn(); 
	} 

	private void TurnOn() { 
		GetComponent<Animation>().Play(SwitchLightOn.name); 
		state = ComponentOnOff.State.on; 
		for (int i = 0; i < ComponentList.Count; i++) { 
			ComponentList[i].enabled = true; 
		} 
	} 

	private void TurnOff() { 
		GetComponent<Animation>().Play(SwitchLightOff.name); 
		state = ComponentOnOff.State.off; 
		for (int i = 0; i < ComponentList.Count; i++) { 
			ComponentList[i].enabled = false; 
		} 

	} 
}