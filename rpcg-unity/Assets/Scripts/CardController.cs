using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CardController :
	MonoBehaviour,
	IPointerEnterHandler,
	IPointerExitHandler,
    IPointerClickHandler
{

    [Header("Components")]
    [SerializeField] private RectTransform container; // We manipulate this transform instead
	[SerializeField] private Image cardImage;
    [SerializeField] private TextMeshProUGUI cardTitle;
    [SerializeField] private TextMeshProUGUI cardCost;

	public Card Model { get; private set; }

    public Vector2 Position
    {
        get { return transform.position; }
        set { transform.position = value; }
    }

    public Vector2 AnchoredPosition {
        get { return RectTrans.anchoredPosition; }
        set { RectTrans.anchoredPosition = value; }
    }

    public float CardWidth => RectTrans.rect.width;

    private RectTransform _rectTrans;
    private RectTransform RectTrans
    {
        get
        {
            if (_rectTrans == null)
            {
                _rectTrans = GetComponent<RectTransform>();
            }
            return _rectTrans;
        }
    }

    #region ——— Constants ———
    private const float CARD_HOVER_DISTANCE = 20f;
    private const float CARD_HOVER_DURATION = 0.1f;
    #endregion

    #region ——— Coroutines ———
    private Coroutine positionCoroutine;
    private Coroutine hoveredCoroutine;
    #endregion

    private bool isBeingHovered;

    public void Bind(Card model)
	{
		Model = model;

		cardTitle.text = model.Title;
		cardCost.text = "" + model.Cost;
	}

    /// <summary>
    /// Perfect if you're moving this card to something else that's not its current parent.
    /// </summary>
    public void SetPosition(Vector2 position, float moveDuration = 0.8f, Action onComplete = null)
    {
        Vector2 startPos = Position;
        Vector2 endPos = position;

        this.EnsureCoroutineStopped(ref positionCoroutine);
        positionCoroutine = this.CreateAnimationRoutine(moveDuration, TransFunction, onComplete);

        void TransFunction(float _t)
        {
            float t = UtilEasing.EaseOutCubic(0, 1, _t);
            Position = Vector2.Lerp(startPos, endPos, t);
        }
    }

    /// <summary>
    /// Perfect if you're moving this card within its current parent's anchors.
    /// </summary>
    public void SetAnchoredPosition(Vector2 anchoredPosition, float moveDuration = 0.8f, Action onComplete = null)
    {
        Vector2 startPos = AnchoredPosition;
        Vector2 endPos = anchoredPosition;

        this.EnsureCoroutineStopped(ref positionCoroutine);
        positionCoroutine = this.CreateAnimationRoutine(moveDuration, TransFunction, onComplete);

        void TransFunction(float _t)
        {
            float t = UtilEasing.EaseOutCubic(0, 1, _t);
            AnchoredPosition = Vector2.Lerp(startPos, endPos, t);
        }
    }

    public void Discard()
    {
        BattleManager.Instance.DiscardCard(this);
        SetPosition(BattleManager.Instance.DiscardPileContainer.position, 0.4f, OnReachDiscardPile);

        void OnReachDiscardPile()
        {
            Destroy(gameObject);
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        Vector2 startPos = container.anchoredPosition;
		Vector2 endPos = Vector2.up * CARD_HOVER_DISTANCE;

		this.EnsureCoroutineStopped(ref hoveredCoroutine);
        hoveredCoroutine = this.CreateAnimationRoutine(CARD_HOVER_DURATION, TransFunction);

		void TransFunction(float t)
		{
            container.anchoredPosition = Vector2.Lerp(startPos, endPos, t);
		}

        isBeingHovered = true;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        Vector2 startPos = container.anchoredPosition;
        Vector2 endPos = Vector2.zero;

        this.EnsureCoroutineStopped(ref hoveredCoroutine);
        hoveredCoroutine = this.CreateAnimationRoutine(CARD_HOVER_DURATION, TransFunction);

        void TransFunction(float t)
        {
            container.anchoredPosition = Vector2.Lerp(startPos, endPos, t);
        }

        isBeingHovered = false;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (!isBeingHovered) return;

        Discard();

    }

    private void Update()
    {
        
    }

}
