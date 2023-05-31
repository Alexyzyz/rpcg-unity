using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBasic : IEnemy
{

    public string Title { get; } = "The Chef";
    public int HPmax { get; } = 100;

    public ICard CardToBePlayed { get; set; } = new CardCarrot();

    public void DetermineNextMove()
    {
        CardToBePlayed = CardGameManager.Instance.CardTypeList.SelectRandom();
    }

}
