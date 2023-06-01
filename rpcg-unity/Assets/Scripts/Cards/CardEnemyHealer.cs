using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardEnemyHealer : ICardTargetAllySingle
{

    private const int HEALING = 10;

    public string Title { get; } = "First Aid";
    public string Description { get; } = $"Heals your opponent by {HEALING} HP.";
    public int Cost { get; } = 2;
    public List<CardGame.CardTag> CardTag { get; } = new List<CardGame.CardTag>() { };

    public void OnTargetAllySingle(UnitController target)
    {
        target.TakeDamage(-HEALING);
    }

}
