using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardScratch : ICardTargetOpponentSingle
{

    private const int DAMAGE = 20;

    public string Title { get; } = "Scratch";
    public string Description { get; } = $"Deals {DAMAGE} damage.";
    public int Cost { get; } = 1;

    public List<CardGame.CardTag> CardTag { get; } = new() { };
    
    public void OnTargetOpponentSingle(UnitController target)
    {
        target.TakeDamage(DAMAGE);
    }

}
