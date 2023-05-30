using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CardGameManager : MonoBehaviour
{

    public static CardGameManager Instance;

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
    public List<ICard> AllFoodCards => CardTypeList.Where(item => item.CardTags.Contains(CardGame.CardTags.Food)).ToList();

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
    public enum CardTags
    {
        Food,
        Ally
    }
}
