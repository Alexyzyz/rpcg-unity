using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitController : MonoBehaviour
{

	[Header("Components")]
	[SerializeField] private UnitSelectedHighlightController controllerSelectedHighlight;

    [Header("Prefabs")]
    [SerializeField] private UnitOverheadController prefabOverhead;

    public IUnit Model { get; private set; }

	private int _hp;
	public int HP {
		get { return _hp; }
		set
		{
			_hp = Mathf.Max(0, value);
            OverheadController.HP = _hp;
		}
	}

	private UnitOverheadController _overheadController;
	public UnitOverheadController OverheadController
	{
		get
		{
			if (_overheadController == null)
			{
				_overheadController = Instantiate(
					prefabOverhead,
					Vector3.zero,
					Quaternion.identity,
					BattleManager.Instance.UnitOverheadParent);
			}
			return _overheadController;
		}
	}

	public bool IsHoverable { get; set; } = false;
	public bool IsBeingHovered { get; set; }
	
	public void Bind(IUnit model)
	{
		Model = model;
        OverheadController.Bind(this);

		HP = model.HPmax;
	}

	public void TakeDamage(int damage)
	{
		HP -= damage;
	}

	private void HandleOnLeftClick()
	{
		if (!Input.GetMouseButtonDown(0)) return;
		if (!IsBeingHovered) return;

        IsHoverable = false;
		IsBeingHovered = false;

		BattleManager.Instance.EndSelectingUnit(this);
	}

	private void SetHoveredState(bool isHovered)
	{
        IsBeingHovered = isHovered;
        controllerSelectedHighlight.ToggleVisible(isHovered);
    }

	private void DebugPosition()
	{
        float xAxis = (Input.GetKey(KeyCode.A) ? 1 : 0) - (Input.GetKey(KeyCode.D) ? 1 : 0);
        float zAxis = (Input.GetKey(KeyCode.W) ? 1 : 0) - (Input.GetKey(KeyCode.S) ? 1 : 0);
        Vector3 normalizedDir = new Vector3(xAxis, 0, zAxis).normalized;
        transform.position += 5f * normalizedDir * Time.deltaTime;
    }

    private void OnMouseOver()
    {
		if (!IsHoverable) return;
		if (IsBeingHovered) return;
		SetHoveredState(true);
    }

    private void OnMouseExit()
    {
		SetHoveredState(false);
    }

    private void Update()
    {
		HandleOnLeftClick();
    }

}
