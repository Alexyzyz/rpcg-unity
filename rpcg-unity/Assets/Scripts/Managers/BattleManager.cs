using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Experimental.GlobalIllumination;

public class BattleManager : MonoBehaviour
{

    public static BattleManager Instance;

    [Header("World Components")]
    public Light LightMain;
    public GameObject FieldHero;
    public GameObject FieldEnemy;

    [Header("Canvas Components")]
    public RectTransform Canvas;
    public RectTransform UnitOverheadParent;
    public RectTransform DrawPileContainer;
    public RectTransform DiscardPileContainer;
    public CardSelectArrowManager CardSelectArrowManager;

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

    public bool IsHeroTurn { get; private set; } = true;

    public bool IsSelectingUnit { get; private set; } = false;
    public CardController PreparedCard { get; private set; }

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
    /// Begin selecting a unit.
    /// </summary>
    public void BeginSelectingUnit(CardController preparedCard)
    {
        PreparedCard = preparedCard;

        IsSelectingUnit = true;
        LightMain.intensity = 0.5f;

        // Show the select arrow
        CardSelectArrowManager.TurnOn(preparedCard);
        
        // Enable targetable units to be hoverable
        CardGame.CardTargetType targetType = preparedCard.TargetType;
        if (targetType == CardGame.CardTargetType.OpponentSingle)
        {
            foreach (UnitController unit in EnemyList) {
                unit.IsSelectable = true;
            }
        } else
        if (targetType == CardGame.CardTargetType.AllySingle)
        {
            foreach (UnitController unit in HeroList) {
                unit.IsSelectable = true;
            }
        }

        // Disable other cards to not be hoverable
        foreach (CardController card in CardHandManager.Instance.CardControllerList)
        {
            card.IsHoverable = false;
        }
        PreparedCard.IsHoverable = true;
    }

    /// <summary>
    /// Cancel selecting a unit. This is done by clicking the card again.
    /// </summary>
    public void CancelSelectingUnit()
    {
        FinishSelectingUnit();
    }

    /// <summary>
    /// Target unit has been selected.
    /// </summary>
    public void EndSelectingUnit(UnitController target)
    {
        PreparedCard.Play(target);
        FinishSelectingUnit();
    }

    /// <summary>
    /// End your turn.
    /// </summary>
    public void EndTurn()
    {
        // CardHandManager.Instance.gameObject.SetActive(false);

        foreach (UnitController enemy in EnemyList)
        {
            BattleEventManager.Instance.AddEvent((enemy.Model as IEnemy).PlayCard());
        }
        BattleEventManager.Instance.AddEvent(StartTurn);
    }

    /// <summary>
    /// Start your turn.
    /// </summary>
    public void StartTurn() {
        // CardHandManager.Instance.gameObject.SetActive(true);

        int amountToDraw = CardGameManager.MAX_CARDS_IN_HAND - CardsToBeKept.Count;

        foreach (CardController cardToDiscard in CardHandManager.Instance.CardControllerList)
        {
            if (CardsToBeKept.Contains(cardToDiscard))
            {
                cardToDiscard.SetSelectedToBeKept(false);
                continue;
            }
            BattleEventManager.Instance.AddEvent(() =>
            {
                CardHandManager.Instance.Discard(cardToDiscard);
                DrawCard();
            }, 0.1f);
            amountToDraw--;
        }

        CardsToBeKept.Clear();

        // Refill your hand
        for (int i = 0; i < amountToDraw; i++)
        {
            BattleEventManager.Instance.AddEvent(DrawCard, 0.1f);
        }

        // Refill your mana
        Mana = MaxMana;

        IsHeroTurn = !IsHeroTurn;

        EventManager.Instance.OnTurnEnded?.Invoke();
    }

    #endregion

    #region Private

    private void StartBattle()
    {
        PopulateDeck();
        PopulateUnitField();

        Mana = CardGameManager.MAX_MANA;
        MaxMana = CardGameManager.MAX_MANA;

        DrawInitialHand();
    }

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
            DrawList.Add(
                CardGameManager.Instance.CardTypeList
                .Where(item => !item.CardTag.Contains(CardGame.CardTag.Food))
                .ToList().SelectRandom());
        }
    }

    private void PopulateUnitField()
    {
        MainHero = AddHero(new HeroStarter());
        AddEnemy(new EnemyBasic());
    }

    private void FinishSelectingUnit()
    {
        PreparedCard = null;
        CardSelectArrowManager.TurnOff();

        IsSelectingUnit = false;
        LightMain.intensity = 1f;

        // Disable every unit from being hoverable
        foreach (UnitController unit in EnemyList)
        {
            unit.IsSelectable = false;
        }
        foreach (UnitController unit in HeroList)
        {
            unit.IsSelectable = false;
        }

        // Enable every card from being hoverable
        foreach (CardController card in CardHandManager.Instance.CardControllerList)
        {
            card.IsHoverable = true;
        }
    }

    private UnitController AddHero(IHero heroModel)
    {
        UnitController newHero = Instantiate(
            PrefabUnit,
            Vector3.zero,
            Quaternion.identity,
            FieldHero.transform);
        newHero.transform.localPosition = Vector3.zero;
        newHero.Bind(heroModel);
        HeroList.Add(newHero);
        return newHero;
    }

    private UnitController AddEnemy(IEnemy enemyModel)
    {
        UnitController newEnemy = Instantiate(
            PrefabUnit,
            Vector3.zero,
            Quaternion.identity,
            FieldEnemy.transform);
        newEnemy.transform.localPosition = Vector3.zero;
        newEnemy.transform.eulerAngles = new Vector3(0, 180, 0);
        newEnemy.Bind(enemyModel);
        EnemyList.Add(newEnemy);
        return newEnemy;
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
        StartBattle();
    }

    #endregion

}
