using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardPortableStove : ICardTargetNone
{

    public string Title { get; } = "Portable Stove";
    public string Description { get; } = $"Creates a random Food card in your hand.";
    public int Cost { get; } = 2;

    public List<CardGame.CardTag> CardTag { get; } = new() { };

    public void OnPlayed()
    {
        ICard randomFoodCard = CardGameManager.Instance.AllFoodCards.SelectRandom();
        BattleManager.Instance.CreateCard(randomFoodCard);
    }

}


public class CardCarrot : ICardTargetAllySingle
{

    private const int HEALING = 10;

    public string Title { get; } = "Carrot";
    public string Description { get; } = $"Heals {HEALING} HP to an ally.";
    public int Cost { get; } = 0;

    public List<CardGame.CardTag> CardTag { get; } = new() {
        CardGame.CardTag.Food
    };

    public void OnTargetAllySingle(UnitController target)
    {
        target.TakeDamage(-HEALING);
    }

}

public class CardDurian : ICardTargetOpponentSingle
{

    private const int DAMAGE = 8;

    public string Title { get; } = "Throwing Durian";
    public string Description { get; } = $"Deals {DAMAGE} DMG to an opponent.";
    public int Cost { get; } = 0;
    public List<CardGame.CardTag> CardTag { get; } = new List<CardGame.CardTag>() {
        CardGame.CardTag.Food
    };

    public void OnTargetOpponentSingle(UnitController target)
    {
        target.TakeDamage(DAMAGE);
    }

}
