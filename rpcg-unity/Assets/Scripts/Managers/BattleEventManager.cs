using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleEventManager : MonoBehaviour
{
	
    public static BattleEventManager Instance;

	private Queue<BattleEvent> eventQueue = new();
    private float previousEventPause = 0f;

	public void AddEvent(Action newEvent, float postEventPause = 0f)
	{
		eventQueue.Enqueue(new BattleEvent(newEvent, postEventPause));

        if (eventQueue.Count == 1) HandleNextEvent();
    }

    private void OnGameEventFinished() => HandleNextEvent();

    private void HandleNextEvent()
    {
        if (eventQueue.Count == 0) return;
        
        BattleEvent nextEvent = eventQueue.Peek();
        this.CreateTimerRoutine(previousEventPause, OnEventFinished);
        previousEventPause = nextEvent.PostEventPause;

        void OnEventFinished()
        {
            nextEvent.MyEvent?.Invoke();
            eventQueue.Dequeue();
            HandleNextEvent();
        }
    }

    private void Subscribe()
    {
        EventManager.Instance.OnGameEventFinished += OnGameEventFinished;
    }

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

    private void Start()
    {
        Subscribe();
    }

    class BattleEvent
    {

        public Action MyEvent;
        public float PostEventPause;

        public BattleEvent(Action myEvent, float postEventPause)
        {
            MyEvent = myEvent;
            PostEventPause = postEventPause;
        }

    }

}
