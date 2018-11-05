using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class IntroController : MonoBehaviour
{

	public FadeController fadeScript;
	public float fadeDelay = 4f;
	public float bgSoundFadeDelay = 7f;
	public float fadeDuration = 2f;
	public Color fadeInColor;

	public float timeToUnfreeze = 4f;
	
	public FirstPersonDrifter fpController;
	public MouseLook mouseLookX;
	public MouseLook mouseLookY;
	public AudioSource flatlineAudio;
	public AudioSource backgroundHumAudio;

	private float bgVolume;
	
	// Use this for initialization
	void Start () {
		fpController.FreezeMovement();
		mouseLookX.SetSensitivity(0f);
		mouseLookY.SetSensitivity(0f);
		fadeScript.fadeImage.color = fadeInColor;

		if (backgroundHumAudio)
		{
			bgVolume = backgroundHumAudio.volume;
			backgroundHumAudio.volume = 0f;
		}
		
		Invoke("BeginFade", fadeDelay);
		Invoke("FadeInBGSound", bgSoundFadeDelay);
		Invoke("Unfreeze", timeToUnfreeze);
	}

	void BeginFade()
	{
		fadeScript.FadeToTransparent(fadeDuration);
		flatlineAudio.DOFade(0f, fadeDuration);
	}

	void FadeInBGSound()
	{
		backgroundHumAudio.DOFade(bgVolume, fadeDuration);
	}
	
	void Unfreeze()
	{
		//Debug.Log("unfreezing");
		fpController.UnfreezeMovement();
		mouseLookX.ResetSensitivity();
		mouseLookY.ResetSensitivity();
	}
	
}
