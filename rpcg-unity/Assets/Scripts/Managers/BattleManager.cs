using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BattleManager : MonoBehaviour
{

    public static BattleManager Instance;

    [Header("World Components")]
    public GameObject FieldHero;
    public GameObject FieldEnemy;

    [Header("Canvas Components")]
    public RectTransform UnitOverheadParent;
    public RectTransform DrawPileContainer;
    public RectTransform DiscardPileContainer;

    [Header("Prefabs")]
    public CardController PrefabCard;
    public UnitController PrefabUnit;

    public List<ICard> DrawList { get; } = new List<ICard>();
    public List<ICard> DiscardList { get; } = new List<ICard>();

    public List<CardController> CardsToBeKept{ get; } = new List<CardController>();

    public List<UnitController> HeroList { get; } = new List<UnitController>();
    public List<UnitController> EnemyList { get; } = new List<UnitController>();

    public UnitController MainHero { get; private set; }

    public float CardPrefabWidth => PrefabCard.CardWidth;

    private int _mana;
    private int _maxMana;

    public int Mana {
        get { return _mana; }
        set
        {
            _mana = value;
            EventManager.Instance.OnManaChanged?.Invoke();
        }
    }
    
    public int MaxMana
    {
        get { return _maxMana; }
        set
        {
            _maxMana = value;
            EventManager.Instance.OnManaChanged?.Invoke();
        }
    }

    #region Public methods

    /// <summary>
    /// Attempt to draw a card from the draw pile and instantiate it.
    /// </summary>
    public void DrawCard()
    {
        if (DrawList.Count == 0) return;

        CardController newCard = Instantiate(
            PrefabCard,
            DrawPileContainer.position,
            Quaternion.identity,
            CardHandManager.Instance.transform);

        ICard newCardModel = DrawList[0];
        DrawList.RemoveAt(0);
        newCard.Bind(newCardModel);

        EventManager.Instance.OnCardDrawn.Invoke(newCard);
    }

    /// <summary>
    /// Create a card and place it in your hand.
    /// </summary>
    public void CreateCard(ICard cardModel)
    {
        CardController newCard = Instantiate(
            PrefabCard,
            DrawPileContainer.position,
            Quaternion.identity,
            CardHandManager.Instance.transform);

        newCard.Bind(cardModel);

        EventManager.Instance.OnCardDrawn.Invoke(newCard);
    }

    /// <summary>
    /// Handle when a card is selected to be kept for the next turn.
    /// </summary>
    public void SelectCardToBeKept(CardController card)
    {
        if (CardsToBeKept.Contains(card))
        {
            card.SetSelectedToBeKept(false);
            CardsToBeKept.Remove(card);
            EventManager.Instance.OnCardKeepPressed?.Invoke(card);
            return;
        }

        if (CardsToBeKept.Count >= CardGameManager.MAX_KEEPABLE_CARDS) return;

        CardsToBeKept.Add(card);
        card.SetSelectedToBeKept(true);
        EventManager.Instance.OnCardKeepPressed?.Invoke(card);
    }

    /// <summary>
    /// End your turn.
    /// </summary>
    public void EndTurn()
    {
        int amountToDraw = CardGameManager.MAX_CARDS_IN_HAND;
        foreach (CardController cardToDiscard in CardHandManager.Instance.CardControllerList)
        {
            if (CardsToBeKept.Contains(cardToDiscard))
            {
                cardToDiscard.SetSelectedToBeKept(false);
                amountToDraw--;
                continue;
            }
            BattleEventManager.Instance.AddEvent(() => CardHandManager.Instance.Discard(cardToDiscard), 0.1f);
        }

        CardsToBeKept.Clear();

        // Refill your hand
        for (int i = 0; i < amountToDraw; i++)
        {
            BattleEventManager.Instance.AddEvent(DrawCard, 0.1f);
        }

        // Refill your mana
        Mana = MaxMana;

        EventManager.Instance.OnTurnEnded?.Invoke();
    }

    #endregion

    #region Private

    private void DrawInitialHand()
    {
        for (int i = 0; i < 6; i++)
        {
            BattleEventManager.Instance.AddEvent(DrawCard, 0.1f);
        }
    }

    private void PopulateDeck()
    {
        for (int i = 0; i < 30; i++)
        {
            DrawList.Add(CardGameManager.Instance.CardTypeList.SelectRandom());
        }
    }

    private void PopulateUnitField()
    {
        MainHero = AddUnit(new UnitStarter(), true);
        AddUnit(new UnitStarter(), false);
    }

    private UnitController AddUnit(IUnit unitModel, bool isHero)
    {
        UnitController newUnit = Instantiate(
            PrefabUnit,
            Vector3.zero,
            Quaternion.identity,
            isHero ? FieldHero.transform : FieldEnemy.transform);
        newUnit.transform.localPosition = Vector3.zero;
        newUnit.Bind(unitModel);

        if (isHero)
        {
            HeroList.Add(newUnit);
        } else
        {
            EnemyList.Add(newUnit);
        }
        return newUnit;
    }

    #endregion

    #region Unity methods

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        } else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        PopulateDeck();
        PopulateUnitField();

        Mana = 7;
        MaxMana = 7;

        DrawInitialHand();
    }

    #endregion

}
