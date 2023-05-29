using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardScratch : ICard
{

    private const int DAMAGE = 20;

    public string Title { get; } = "Scratch";
    public string Description { get; } = $"Deals {DAMAGE} damage.";
    public int Cost { get; } = 1;

    public List<CardGame.CardTags> CardTags { get; } = new() { };

    public void OnPlayed()
    {
        List<UnitController> enemyList = BattleManager.Instance.EnemyList;

        if (enemyList.Count == 0) return;
        UnitController targetUnit = enemyList[0];
        targetUnit.TakeDamage(DAMAGE);
    }

}
