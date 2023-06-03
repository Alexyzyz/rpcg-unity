using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitController : MonoBehaviour
{

	[Header("Components")]
	public SpriteRenderer SpriteRendererRig;

    [Header("Prefabs")]
    [SerializeField] private UnitOverheadController prefabOverhead;

	[Header("Colors")]
	[SerializeField] private Color colorHovered;
	[SerializeField] private Color colorUnhovered;

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

	public Vector2 TargetSelectArrowPosition
	{
		get
		{
			Sprite mySprite = SpriteRendererRig.sprite;
			Vector3 worldPos = transform.position + new Vector3(0, mySprite.bounds.size.y / 2, 0);
			Vector2 canvasPos = Camera.main.WorldToScreenPoint(worldPos);
			return canvasPos;
		}
	}

	public bool IsSelectable { get; set; } = false;
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

	private const string MATERIAL_OUTLINE_PROPNAME_COLOR = "_SolidOutline";

    private void HandleOnLeftClick()
	{
		if (!Input.GetMouseButtonDown(0)) return;
		if (!IsBeingHovered) return;
		if (!IsSelectable) return;

		BattleManager.Instance.EndSelectingUnit(this);
	}

	private void SetHoveredState(bool isHovered)
	{
        IsBeingHovered = isHovered;
        SpriteRendererRig.material.SetColor(MATERIAL_OUTLINE_PROPNAME_COLOR, isHovered ? colorHovered : colorUnhovered);

        if (isHovered)
		{
			EventManager.Instance.OnUnitHovered(this);
		} else
		{
			EventManager.Instance.OnUnitUnhovered(this);
		}
    }

	private void DebugPosition()
	{
        float xAxis = (Input.GetKey(KeyCode.A) ? 1 : 0) - (Input.GetKey(KeyCode.D) ? 1 : 0);
        float zAxis = (Input.GetKey(KeyCode.W) ? 1 : 0) - (Input.GetKey(KeyCode.S) ? 1 : 0);
        Vector3 normalizedDir = new Vector3(xAxis, 0, zAxis).normalized;
        transform.position += 5f * Time.deltaTime * normalizedDir;
    }

    private void OnMouseOver()
    {
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
