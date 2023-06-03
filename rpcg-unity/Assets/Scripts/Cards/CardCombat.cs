using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardScratch : ICardTargetOpponentSingle
{

    private const int DAMAGE = 4;

    public string Title { get; } = "Scratch";
    public string Description { get; } = $"Deals {DAMAGE} DMG to an opponent.";
    public int Cost { get; } = 1;

    public List<CardGame.CardTag> CardTag { get; } = new() { };

    public void OnTargetOpponentSingle(UnitController target)
    {
        target.TakeDamage(DAMAGE);
    }

}

public class CardRecoil : ICardTargetOpponentSingle
{

    private const int DAMAGE = 12;
    private const int RECOIL_DAMAGE = 5;

    public string Title { get; } = "Recoil";
    public string Description { get; } = $"Deals {DAMAGE} DMG to an opponent and {RECOIL_DAMAGE} DMG to yourself.";
    public int Cost { get; } = 2;
    public List<CardGame.CardTag> CardTag { get; } = new List<CardGame.CardTag>() { };

    public void OnTargetOpponentSingle(UnitController target)
    {
        target.TakeDamage(DAMAGE);
        BattleManager.Instance.MainHero.TakeDamage(RECOIL_DAMAGE);
    }

}

