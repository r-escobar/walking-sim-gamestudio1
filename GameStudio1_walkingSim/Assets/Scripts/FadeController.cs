using System.Collections;
using System.Collections.Generic;
using System.Net.Mime;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class FadeController : MonoBehaviour
{

	public Image fadeImage;

	public void FadeToTransparent(float time)
	{
		fadeImage.DOFade(0f, time);
	}

	public void FadeToSolid(float time)
	{
		fadeImage.DOFade(1f, time);
	}
}
