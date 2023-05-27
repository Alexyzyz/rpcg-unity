using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleManager : MonoBehaviour
{

    public static BattleManager Instance;

    [Header("Components")]
    [SerializeField] private CardController prefabCard;
    public RectTransform DrawPileContainer;
    public RectTransform DiscardPileContainer;

    public List<Card> DrawList { get; } = new List<Card>();
    public List<Card> DiscardList { get; } = new List<Card>();

    public float CardPrefabWidth => prefabCard.CardWidth;

    /// <summary>
    /// Attempts to draw a card from the draw pile and instantiates it.
    /// </summary>
    public void DrawCard()
    {
        if (DrawList.Count == 0) return;

        CardController newCard = Instantiate(
            prefabCard,
            DrawPileContainer.position,
            Quaternion.identity,
            CardHandManager.Instance.transform);

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
            Card newCard = new Card();
            DrawList.Add(newCard);
        }
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
    }

}
