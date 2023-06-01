using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CardGameManager : MonoBehaviour
{

    public static CardGameManager Instance;

    /// <summary>
    /// The game will ask you to discard your hand until you only have this amount before ending your turn.
    /// </summary>
    public const int MAX_CARDS_IN_HAND = 6;

    /// <summary>
    /// You may keep up to this amount of cards to carry over to your next turn.
    /// </summary>
    public const int MAX_KEEPABLE_CARDS = 2;

    /// <summary>
    /// A list of all cards available in the game.
    /// </summary>
    public List<ICard> CardTypeList = new()
    {
        new CardScratch(),
        new CardEnemyHealer(),
        new CardPortableStove(),
        new CardCarrot(),
        new CardPoisonPotato(),
    };

    /// <summary>
    /// A list of all Food cards available in the game.
    /// </summary>
    public List<ICard> AllFoodCards => CardTypeList.Where(item => item.CardTag.Contains(CardGame.CardTag.Food)).ToList();

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

}

namespace CardGame
{
    public enum CardTag
    {
        Food,
        Ally
    }

    public enum CardTargetType
    {
        None,
        AllySingle,
        OpponentSingle
    }
}
