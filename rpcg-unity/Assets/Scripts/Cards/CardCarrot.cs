using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardCarrot : ICard
{

    private const int HEALING = 20;

    public string Title { get; } = "Carrot";
    public string Description { get; } = $"Heals {HEALING} HP.";
    public int Cost { get; } = 0;

    public List<CardGame.CardTags> CardTags { get; } = new() {
        CardGame.CardTags.Food
    };

    public void OnPlayed()
    {
        UnitController player = BattleManager.Instance.MainHero;
        player.TakeDamage(-HEALING);
    }

}
