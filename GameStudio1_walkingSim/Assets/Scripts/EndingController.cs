using System.Collections;
using System.Collections.Generic;
using System.Net.Mime;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class EndingController : MonoBehaviour {

	public FirstPersonDrifter fpController;
	public MouseLook mouseLookX;
	public MouseLook mouseLookY;
	
	public AudioSource backgroundHumAudio;

	public float audioFadeDuration = 2f;

	public float textFadeDuration = 1f;

	public Image cursorDot;

	public Text titleText;
	public Text creditsText;
	public Text exitText;

	public AudioSource endingMusic;


	private bool canExit = false;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if (canExit)
		{
			if(Input.GetKey(KeyCode.Escape))
			{
				Application.Quit();
			}
		}
	}

	private void OnTriggerEnter(Collider other)
	{
		if (other.gameObject.tag == "Player")
		{
			fpController.FreezeMovement();
			mouseLookX.SetSensitivity(0f);
			mouseLookY.SetSensitivity(0f);
			FadeOutBGSound();
		}
	}
	
	void FadeOutBGSound()
	{
		cursorDot.DOFade(0f, audioFadeDuration);
		backgroundHumAudio.DOFade(0f, audioFadeDuration).OnComplete(StartCredits);
	}

	public void StartCredits()
	{
		endingMusic.Play();
		titleText.DOFade(1f, textFadeDuration).SetDelay(.75f);
		creditsText.DOFade(1f, textFadeDuration).SetDelay(3.85f).OnComplete(EnableEscape);
		exitText.DOFade(1f, textFadeDuration).SetDelay(7.3f);

	}

	public void EnableEscape()
	{
		canExit = true;
	}
}
