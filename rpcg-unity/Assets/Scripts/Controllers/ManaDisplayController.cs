using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ManaDisplayController : MonoBehaviour
{

	[Header("Components")]
	[SerializeField] private TextMeshProUGUI manaText;
	[SerializeField] private TextMeshProUGUI maxManaText;

    private void UpdateDisplay()
    {
        manaText.text = "" + BattleManager.Instance.Mana;
        maxManaText.text = "" + BattleManager.Instance.MaxMana;
    }
    
    private void Subscribe()
    {
        EventManager.Instance.OnManaChanged += UpdateDisplay;
    }

    private void Start()
    {
        Subscribe();
    }

}
