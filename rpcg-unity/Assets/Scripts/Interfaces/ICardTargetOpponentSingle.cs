using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ICardTargetOpponentSingle : ICard
{

	/// <summary>
	/// When this card is played on an opponent.
	/// </summary>
	public void OnTargetOpponentSingle(UnitController target);
	
}
