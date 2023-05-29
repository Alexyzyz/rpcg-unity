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

    public List<UnitController> HeroList { get; } = new List<UnitController>();
    public List<UnitController> EnemyList { get; } = new List<UnitController>();

    public UnitController MainHero { get; private set; }

    public float CardPrefabWidth => PrefabCard.CardWidth;

    /// <summary>
    /// Attempts to draw a card from the draw pile and instantiate it.
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
    /// Creates a card and places it in your hand.
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
    /// Discard a card.
    /// </summary>
    public void DiscardCard(CardController card)
    {
        EventManager.Instance.OnCardDiscarded.Invoke(card);
    }

    private void PopulateDeck()
    {
        for (int i = 0; i < 30; i++)
        {
            ICard[] cardTypeList = CardGameManager.Instance.CardTypeList;
            int randIndex = Random.Range(0, cardTypeList.Length);
            DrawList.Add(cardTypeList[randIndex]);
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

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        } else
        {
            Destroy(gameObject);
        }
        
        PopulateDeck();
        PopulateUnitField();
    }

}
