using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBasic : IEnemy
{

    public string Title { get; } = "Monster";
    public int HPmax { get; } = 60;

    public ICard CardToBePlayed { get; set; } = new CardScratch();

    public Action PlayCard()
    {
        switch (CardToBePlayed)
        {
            case CardScratch:
                return PlayScratch;
            default:
                return PlayScratch;
        }
    }

    public void DetermineNextCard()
    {
        CardToBePlayed = CardGameManager.Instance.CardTypeList.SelectRandom();
    }

    // Targeting logic

    private void PlayScratch()
    {
        UnitController target = BattleManager.Instance.HeroList.SelectRandom();
        (CardToBePlayed as ICardTargetOpponentSingle).OnTargetOpponentSingle(target);
    }

}
