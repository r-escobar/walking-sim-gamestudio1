using System.Collections;
using System.Collections.Generic;
using System.Net.Mime;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class CursorDotController : MonoBehaviour
{
	private float cursorSize;
	private RectTransform rTransform;
	private Image cursorImage;
	
	private Vector2 defaultSize;
	private Color defaultColor;

	public float maxSize = 15f;
	public Color hoverColor;
	public float growDuration = 0.5f;
	public Ease easeType;

	private bool enlarged = false;
	
	// Use this for initialization
	void Start ()
	{
		rTransform = GetComponent<RectTransform>();
		defaultSize = rTransform.sizeDelta;

		cursorImage = GetComponent<Image>();
		defaultColor = cursorImage.color;
	}
	public void EnlargeCursor()
	{
		if (enlarged)
			return;
		enlarged = true;
		rTransform.DOSizeDelta(new Vector2(maxSize, maxSize), growDuration, false).SetEase(easeType);
		cursorImage.DOColor(hoverColor, growDuration).SetEase(easeType);
	}

	public void ResetCursorSize()
	{
		if (!enlarged)
			return;
		enlarged = false;
		rTransform.DOSizeDelta(defaultSize, growDuration, false).SetEase(easeType);
		cursorImage.DOColor(defaultColor, growDuration).SetEase(easeType);
	}
}
