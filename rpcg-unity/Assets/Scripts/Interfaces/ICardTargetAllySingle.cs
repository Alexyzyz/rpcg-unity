using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ICardTargetAllySingle : ICard
{

    /// <summary>
    /// When this card is played on an ally.
    /// </summary>
    public void OnTargetAllySingle(UnitController target);

}
