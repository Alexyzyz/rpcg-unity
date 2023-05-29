using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardPortableStove : ICard
{

    public string Title { get; } = "Portable Stove";
    public string Description { get; } = $"Creates a random Food card in your hand.";
    public int Cost { get; } = 2;
    
    public List<CardGame.CardTags> CardTags { get; } = new() { };

    public void OnPlayed()
    {
        ICard randomFoodCard = CardGameManager.Instance.AllFoodCards.SelectRandom();
        BattleManager.Instance.CreateCard(randomFoodCard);
    }

}
