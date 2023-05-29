using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardEnemyHealer : ICard
{

    private const int HEALING = 10;

    public string Title { get; } = "First Aid";
    public string Description { get; } = $"Heals your opponent by {HEALING} HP.";
    public int Cost { get; } = 2;
    public List<CardGame.CardTags> CardTags { get; } = new List<CardGame.CardTags>() { };

    public void OnPlayed()
    {
        List<UnitController> enemyList = BattleManager.Instance.EnemyList;

        if (enemyList.Count == 0) return;
        UnitController targetUnit = enemyList[0];
        targetUnit.TakeDamage(-HEALING);
    }

}
