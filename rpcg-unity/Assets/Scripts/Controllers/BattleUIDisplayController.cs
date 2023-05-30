using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BattleUIDisplayController : MonoBehaviour
{

    public static BattleUIDisplayController Instance;

    [Header("Mana Display Components")]
    [SerializeField] private TextMeshProUGUI manaText;
    [SerializeField] private TextMeshProUGUI maxManaText;

    [Header("Keep Cards Reminder Component")]
	[SerializeField] private TextMeshProUGUI textReminder;

    [Header("End Turn Component")]
    [SerializeField] private Button buttonEndTurn;

	private const string KEEP_CARDS_REMINDER = "You may select {0} card(s) to keep for your next turn.";

    public void OnEndTurnButtonPressed()
    {
		BattleManager.Instance.EndTurn();
    }

    private void UpdateManaDisplay()
    {
        manaText.text = "" + BattleManager.Instance.Mana;
        maxManaText.text = "" + BattleManager.Instance.MaxMana;
    }

    private void UpdateKeepCardsReminderDisplay()
	{
		int selectableCardsAmount = CardGameManager.MAX_KEEPABLE_CARDS - BattleManager.Instance.CardsToBeKept.Count;
		textReminder.text = string.Format(KEEP_CARDS_REMINDER, selectableCardsAmount);
    }

	private void Subscribe()
	{
        EventManager.Instance.OnManaChanged += UpdateManaDisplay;

        EventManager.Instance.OnCardKeepPressed += (CardController card) => UpdateKeepCardsReminderDisplay();
        EventManager.Instance.OnTurnEnded += UpdateKeepCardsReminderDisplay;
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

}
