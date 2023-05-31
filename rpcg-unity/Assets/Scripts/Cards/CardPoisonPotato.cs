using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardPoisonPotato : ICardTargetNone
{

    private const int POISON_DAMAGE = 3;

    public string Title { get; } = "Poisonous Potato";
    public string Description { get; } = $"Deals {POISON_DAMAGE} damage to yourself.";
    public int Cost { get; } = 0;
    public List<CardGame.CardTags> CardTags { get; } = new List<CardGame.CardTags>() {
        CardGame.CardTags.Food
    };

    public void OnPlayed()
    {
        UnitController player = BattleManager.Instance.MainHero;
        player.TakeDamage(POISON_DAMAGE);
    }

}
