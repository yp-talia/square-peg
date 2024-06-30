// Using DotTween and building on: https://github.com/Noixelfer/FadeInFadeOut/blob/main/Assets/Scripts/FadingPanel.cs

using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FadingPanel : MonoBehaviour
{
	DataManager dataManager;
	private enum FadeDirection {In,Out, InThenOut,OutThenIn };
	[SerializeField] private CanvasGroup canvasGroup;
    [SerializeField] private float duration = 5.0f;
    [SerializeField] private FadeDirection fadeDirection;
	private Tween fadeTween;

	// Start is called before the first frame update
	void Start()
	{
		dataManager = DataManager.Instance;

        if (dataManager.debugOnInfo == true)
        {
            Debug.Log("Fade Start Begun"); // As this Class is largely coroutines putting this up front.
        }
        switch (fadeDirection)
		{
			case FadeDirection.In:
			{
				FadeIn( duration);
				break;
			}
			case FadeDirection.Out:
			{
				FadeOut( duration);
				break;
			}
			case FadeDirection.InThenOut:
			{
				StartCoroutine(InThenOutCoroutine());
				break;
			}
			case FadeDirection.OutThenIn:
			{
				StartCoroutine(OutThenInCoroutine());
				break;
			}
		}
	}
    private IEnumerator InThenOutCoroutine()
    {
		FadeOut( duration/2);
		yield return new WaitForSeconds(duration/2);
		FadeIn( duration/2);
		yield return new WaitForSeconds(duration/2);
    }

    private IEnumerator OutThenInCoroutine()
    {
		FadeIn( duration/2);
		yield return new WaitForSeconds(duration/2);
		FadeOut( duration/2);
		yield return new WaitForSeconds(duration/2);
    }
    public void FadeIn(float duration)
	{
		Fade(1f, duration, () =>
		{
			canvasGroup.interactable = true;
			canvasGroup.blocksRaycasts = true;
		});
	}

	public void FadeOut(float duration)
	{
		Fade(0f, duration, () =>
		{
			canvasGroup.interactable = false;
			canvasGroup.blocksRaycasts = false;
		});
	}

	private void Fade(float endValue, float duration, TweenCallback onEnd)
	{
        if (dataManager.debugOnInfo == true)
        {
            Debug.Log("Fading over" + duration + " to an end value of" + endValue);
        }
		if (fadeTween != null)
		{
			fadeTween.Kill(false);
		}

		fadeTween = canvasGroup.DOFade(endValue, duration);
		fadeTween.onComplete += onEnd;
		
		if (dataManager.debugOnInfo == true)
        {
            Debug.Log("Fade begun");
        }
	}
}