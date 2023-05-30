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

    [Header("Prefabs")]
    [SerializeField] private CardHintController prefabCardHint;

    [Header("Components")]
    [SerializeField] private RectTransform container;
    [SerializeField] private CanvasGroup canvasGroup;
    [SerializeField] private RectTransform hintParent;
    [SerializeField] private Image keptHighlight;
	[SerializeField] private Image cardImage;
    [SerializeField] private TextMeshProUGUI cardTitle;
    [SerializeField] private TextMeshProUGUI cardCost;

	public ICard Model { get; private set; }

    public Vector2 Position
    {
        get { return transform.position; }
        set { transform.position = value; }
    }

    public Vector2 AnchoredPosition {
        get { return RectTransform.anchoredPosition; }
        set { RectTransform.anchoredPosition = value; }
    }

    public bool IsPlayable => manaCostSatisfied && !isSelectedToBeKept;

    public float CardWidth => RectTransform.rect.width;

    private RectTransform _rectTransform;
    private RectTransform RectTransform
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

    private CardHintController _cardHint;
    private CardHintController CardHint
    {
        get
        {
            if (_cardHint == null)
            {
                _cardHint = Instantiate(prefabCardHint, Vector2.zero, Quaternion.identity, hintParent);
                _cardHint.Bind(this);
            }
            return _cardHint;
        }
    }

    #region Constants
    private const float CARD_HOVER_DISTANCE = 20f;
    private const float CARD_HOVER_DURATION = 0.1f;
    #endregion

    #region Coroutines
    private Coroutine positionCoroutine;
    private Coroutine hoveredCoroutine;
    #endregion

    private bool isBeingHovered;
    private bool isSelectedToBeKept => BattleManager.Instance.CardsToBeKept.Contains(this);
    private bool manaCostSatisfied
    {
        get
        {
            if (Model == null) return false;
            return BattleManager.Instance.Mana >= Model.Cost;
        }
    }

    public void Bind(ICard model)
	{
		Model = model;

		cardTitle.text = model.Title;
		cardCost.text = "" + model.Cost;

        OnManaChanged();
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

    /// <summary>
    /// Toggle whether this card is being kept for the next turn or not.
    /// </summary>
    /// <param name="isBeingKept"></param>
    public void SetSelectedToBeKept(bool isBeingKept)
    {
        keptHighlight.gameObject.SetActive(isBeingKept);
    }

    /// <summary>
    /// Animate this card being discarded.
    /// </summary>
    public void AnimateDiscard()
    {
        SetPosition(BattleManager.Instance.DiscardPileContainer.position, 0.4f, OnReachDiscardPile);
        void OnReachDiscardPile()
        {
            Destroy(gameObject);
        }
    }

    /// <summary>
    /// Play this card.
    /// </summary>
    private void Play()
    {
        if (!IsPlayable) return;

        BattleManager.Instance.Mana -= Model.Cost;
        Model.OnPlayed();

        CardHandManager.Instance.Discard(this);
        EventManager.Instance.OnCardPlayed?.Invoke(this);
    }

    private void OnManaChanged()
    {
        canvasGroup.alpha = manaCostSatisfied ? 1 : 0.5f;
    }

    private void Subscribe()
    {
        EventManager.Instance.OnManaChanged += OnManaChanged;
    }

    private void Unsubscribe()
    {
        EventManager.Instance.OnManaChanged -= OnManaChanged;
    }

    #region Unity methods

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

        CardHint.ShowHint();

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

        CardHint.HideHint();

        isBeingHovered = false;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (!isBeingHovered) return;

        switch (eventData.button)
        {
            case PointerEventData.InputButton.Left:
                Play();
                break;
            case PointerEventData.InputButton.Right:
                BattleManager.Instance.SelectCardToBeKept(this);
                break;
            default:
                break;
        }

    }

    private void Start()
    {
        Subscribe();
    }

    private void OnDestroy()
    {
        Unsubscribe();
    }

    #endregion

}
