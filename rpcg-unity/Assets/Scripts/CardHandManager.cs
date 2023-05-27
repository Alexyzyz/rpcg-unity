using System.Collections;
using System.Collections.Generic;
using UnityEditorInternal;
using UnityEngine;

public class CardHandManager : MonoBehaviour
{

	public static CardHandManager Instance;

	public List<CardController> CardControllerList { get; } = new List<CardController>();

	private const float CARD_SPACING = 5f;

	public void OnDrawCard(CardController card)
	{
		CardControllerList.Add(card);
		UpdateCardPositions();
    }

    public void OnDiscardCard(CardController card)
    {
        CardControllerList.Remove(card);
        UpdateCardPositions();
    }

    private void UpdateCardPositions()
	{
		float cardWidth = BattleManager.Instance.CardPrefabWidth;
		float cardSpace = cardWidth + CARD_SPACING;

		float startX = -cardSpace * CardControllerList.Count / 2 + cardSpace / 2;

		int i = 0;
		foreach (CardController card in CardControllerList)
		{
			card.SetAnchoredPosition(new Vector2(startX + i * cardSpace, 0));
			i++;
		}
	}

	private void SubscribeToBroadcasters()
	{
		EventManager.Instance.OnCardDrawn += OnDrawCard;
		EventManager.Instance.OnCardDiscarded += OnDiscardCard;
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

		SubscribeToBroadcasters();
    }

}
