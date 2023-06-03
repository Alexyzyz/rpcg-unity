using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UnitOverheadController : MonoBehaviour
{

	[Header("Components")]
	[SerializeField] private Image hpBar;
    [SerializeField] private Image hpLagBar;
    [SerializeField] private TextMeshProUGUI hpText;

    private int hpMax = 1;
	private UnitController unitController;

    private int _hp = 1;
	public int HP {
        set
        {
			int oldHP = _hp;
			_hp = value;
			UpdateDisplay(oldHP, _hp);
        }
	}

	private Coroutine animateLagBarCoroutine;

	public void Bind(UnitController unit)
	{
		unitController = unit;
        hpMax = unit.Model.HPmax;
		HP = hpMax;
    }

	private void UpdateDisplay(int oldHP, int newHP)
	{
		float startValue = (float)oldHP / hpMax;
		float endValue = (float)newHP / hpMax;

		hpBar.fillAmount = endValue;
        hpText.text = $"{newHP}/{hpMax}";

		this.EnsureCoroutineStopped(ref animateLagBarCoroutine);
        animateLagBarCoroutine = this.CreateAnimationRoutine(1f, TransFunction);

		void TransFunction(float t)
		{
            hpLagBar.fillAmount = Mathf.Lerp(startValue, endValue, t);
        }
    }

	private void UpdateCanvasPosition()
	{
		if (unitController == null) return;
		Vector3 worldPos = unitController.transform.position + new Vector3(0, unitController.SpriteBoundsHeight, 0);
		Vector2 canvasPos = Camera.main.WorldToScreenPoint(worldPos);
		transform.position = canvasPos;
	}

    private void Update()
    {
		UpdateCanvasPosition();
    }

}
