using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CardHintController : MonoBehaviour
{

	[Header("Components")]
	public TextMeshProUGUI cardNameText;
	public TextMeshProUGUI cardDescriptionText;
	public CanvasGroup canvasGroup;

	private CardController cardController;

	private Coroutine hintShowCoroutine;

	private RectTransform _rectTransform;
	public RectTransform RectTransform
	{
		get
		{
			if (_rectTransform == null)
			{
				_rectTransform = GetComponent<RectTransform>();
			}
			return _rectTransform;
		}
	}

    #region Constants
    private const float HINT_Y_POS = 230f;
    private const float HOVER_HEIGHT = 20f;
	private const float HOVER_ANIMATION_DURATION = 0.1f;
    #endregion

    public void Bind(CardController cardController)
	{
		this.cardController = cardController;
		RectTransform.anchoredPosition = new(0, HINT_Y_POS);

        cardNameText.text = cardController.Model.Title;
		cardDescriptionText.text = cardController.Model.Description;

		HideHint();
    }

	public void ShowHint()
	{
		Vector2 startPos = RectTransform.anchoredPosition;
		Vector2 endPos = new(0, HINT_Y_POS + HOVER_HEIGHT);

		float startAlpha = 0;
		float endAlpha = 1;

		this.EnsureCoroutineStopped(ref hintShowCoroutine);
		hintShowCoroutine = this.CreateAnimationRoutine(HOVER_ANIMATION_DURATION, TransFunction);

		void TransFunction(float t)
		{
			RectTransform.anchoredPosition = Vector2.Lerp(startPos, endPos, t);
			canvasGroup.alpha = Mathf.Lerp(startAlpha, endAlpha, t);
        }
	}

    public void HideHint()
    {
        Vector2 startPos = RectTransform.anchoredPosition;
        Vector2 endPos = new(0, HINT_Y_POS);

        float startAlpha = 1;
        float endAlpha = 0;

        this.EnsureCoroutineStopped(ref hintShowCoroutine);
        hintShowCoroutine = this.CreateAnimationRoutine(HOVER_ANIMATION_DURATION, TransFunction);

        void TransFunction(float t)
        {
            RectTransform.anchoredPosition = Vector2.Lerp(startPos, endPos, t);
            canvasGroup.alpha = Mathf.Lerp(startAlpha, endAlpha, t);
        }
    }

}
