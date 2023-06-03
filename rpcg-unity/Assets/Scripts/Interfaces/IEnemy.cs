using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IEnemy : IUnit
{
	
	/// <summary>
	/// The card this enemy will play on their next turn.
	/// </summary>
	public ICard CardToBePlayed { get; set; }

    /// <summary>
    /// Play the card being queued.
    /// </summary>
    public Action PlayCard();

    /// <summary>
    /// Update which card this enemy should play on their next turn.
    /// </summary>
    public void DetermineNextCard();

}
