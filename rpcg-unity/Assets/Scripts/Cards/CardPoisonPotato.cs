using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardPoisonPotato : ICardTargetAllySingle
{

    private const int POISON_DAMAGE = 3;

    public string Title { get; } = "Poisonous Potato";
    public string Description { get; } = $"Deals {POISON_DAMAGE} damage to yourself.";
    public int Cost { get; } = 0;
    public List<CardGame.CardTag> CardTag { get; } = new List<CardGame.CardTag>() {
        CardGame.CardTag.Food
    };

    public void OnTargetAllySingle(UnitController target)
    {
        target.TakeDamage(POISON_DAMAGE);
    }

}
