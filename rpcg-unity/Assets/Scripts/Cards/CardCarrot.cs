using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardCarrot : ICardTargetAllySingle
{

    private const int HEALING = 20;

    public string Title { get; } = "Carrot";
    public string Description { get; } = $"Heals {HEALING} HP.";
    public int Cost { get; } = 0;

    public List<CardGame.CardTag> CardTag { get; } = new() {
        CardGame.CardTag.Food
    };

    public void OnTargetAllySingle(UnitController target)
    {
        target.TakeDamage(-HEALING);
    }

}
