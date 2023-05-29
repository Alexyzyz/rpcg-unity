using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventManager : MonoBehaviour
{

	public static EventManager Instance;

    public delegate void CardControllerNotifier(CardController cardController);
    public CardControllerNotifier OnCardDrawn;
    public CardControllerNotifier OnCardDiscarded;

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
