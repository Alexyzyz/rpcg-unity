using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventManager : MonoBehaviour
{

	public static EventManager Instance;

    public delegate void Notifier();
    public Notifier OnGameEventFinished { get; set; }
    public Notifier OnManaChanged { get; set; }
    public Notifier OnTurnEnded { get; set; }

    public delegate void CardControllerNotifier(CardController cardController);
    public CardControllerNotifier OnCardDrawn { get; set; }
    public CardControllerNotifier OnCardPlayed { get; set; }
    /// <summary>
    /// Called when a card is pressed to toggle keeping it. Right mouse button by default.
    /// </summary>
    public CardControllerNotifier OnCardKeepPressed { get; set; }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        } else
        {
            Destroy(gameObject);
        }
    }

}
