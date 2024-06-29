// Using DotTween and building on: https://github.com/Noixelfer/FadeInFadeOut/blob/main/Assets/Scripts/FadingPanel.cs

using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FadingPanel : MonoBehaviour
{
	[SerializeField] private CanvasGroup canvasGroup;
    [SerializeField] private float duration = 2.0f;
    [SerializeField] private bool fadeIn = true;
	private Tween fadeTween;

	// Start is called before the first frame update
	void Start()
	{
        if (fadeIn == true)
        {
		StartCoroutine(CreateFadeIn());
        }
        else
        {
        StartCoroutine(CreateFadeOut());
        }
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
		if (fadeTween != null)
		{
			fadeTween.Kill(false);
		}

		fadeTween = canvasGroup.DOFade(endValue, duration);
		fadeTween.onComplete += onEnd;
	}

	private IEnumerator CreateFadeIn()
	{
		// yield return new WaitForSeconds(2f);
		// FadeOut(1f);
		yield return new WaitForSeconds(3f);
		FadeIn(duration: duration);
	}
    private IEnumerator CreateFadeOut()
	{
		yield return new WaitForSeconds(2f);
		FadeOut(duration);
		// yield return new WaitForSeconds(3f);
		// FadeIn(1f);
	}
}