using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CardSelectArrowManager : MonoBehaviour
{

    [Header("Components")]
    [SerializeField] private CanvasGroup canvasGroup;

    [Header("Prefabs")]
    [SerializeField] private CardSelectArrowController prefabArrow;

    [Header("Colors")]
    [SerializeField] private Color colorArrowNoTarget;
    [SerializeField] private Color colorArrowValidTarget;
    [SerializeField] private Color colorArrowInvalidTarget;

	private CardController preparedCard;
    private UnitController hoveredUnit;
    private BezierCurve curve = new();

    private const int ARROW_COUNT = 64;
    private const int DISTANCE_PER_ARROW = 48;
    private const float CURVINESS = 64;
    private const float ANIMATION_SPEED = 1.8f;

    private CardSelectArrowController[] controllerArrowArray = new CardSelectArrowController[ARROW_COUNT];
    private CardSelectArrowController controllerCursorArrow;

    private float arrowAnimationLerpValue;

    /// <summary>
    /// Show the arrow curve that appears when selecting a card's target.
    /// </summary>
    public void TurnOn(CardController card)
    {
        preparedCard = card;
        canvasGroup.alpha = 1;
    }

    /// <summary>
    /// Hide the arrow curve when the player isn't currently selecting a card's target.
    /// </summary>
    public void TurnOff()
    {
        preparedCard = null;
        canvasGroup.alpha = 0;
    }

    private void UpdateCurve()
    {

        if (!(hoveredUnit != null && !hoveredUnit.IsSelectable))
        {
            // Only stop animating when the hovered target is invalid
            arrowAnimationLerpValue = (arrowAnimationLerpValue + ANIMATION_SPEED * Time.deltaTime) % 1;
        }

        if (preparedCard == null) return;
        if (controllerArrowArray[0] == null) return;

        float canvasScaler = BattleManager.Instance.Canvas.localScale.x;

        Vector2 startPos = preparedCard.Position;
        Vector2 endPos = hoveredUnit == null ? Input.mousePosition : hoveredUnit.TargetSelectArrowPosition;
        Vector2 firstControl = Vector2.Lerp(startPos, endPos, 1 / 3f);
        Vector2 secondControl = Vector2.Lerp(startPos, endPos, 3 / 4f);

        Vector2 startEndUnit = (endPos - startPos).normalized;
        Vector2 startEndRight = UtilMath.RotateVector(startEndUnit, Mathf.PI / 2);

        bool isOffsetToTheRight = endPos.x >= preparedCard.Position.x;
        float offsetSign = isOffsetToTheRight ? 1 : -1;
        float scaledCurviness = canvasScaler * CURVINESS;

        curve.P0 = startPos;
        curve.P1 = firstControl + offsetSign * scaledCurviness * startEndRight;
        curve.P2 = secondControl + offsetSign * scaledCurviness * startEndRight;
        curve.P3 = endPos;

        // Now we try to populate the curve with arrows
        float arcLength = curve.GetLength(8);

        float scaledDistancePerArrow = canvasScaler * DISTANCE_PER_ARROW;
        int fittableArrowCount = Mathf.Min(ARROW_COUNT - 1, (int)(arcLength / scaledDistancePerArrow));
        float fittableLength = fittableArrowCount * scaledDistancePerArrow;

        float maxLerpValue = fittableLength / arcLength;
        float stepValue = maxLerpValue / fittableArrowCount;

        float animationLerpValueOffset = Mathf.Lerp(0, stepValue, arrowAnimationLerpValue);

        Color arrowColor;
        if (hoveredUnit)
        {
            arrowColor = hoveredUnit.IsSelectable ? colorArrowValidTarget : colorArrowInvalidTarget;
        } else
        {
            arrowColor = colorArrowNoTarget;
        }

        CardSelectArrowController controllerArrow;
        float finalLerpValue;

        // Show the arrows that can fit in the curve
        for (int i = 0; i < fittableArrowCount; i++)
        {
            controllerArrow = controllerArrowArray[i];
            finalLerpValue = i * stepValue + animationLerpValueOffset;

            controllerArrow.gameObject.SetActive(true);
            controllerArrow.Image.color = arrowColor;
            controllerArrow.RectTransform.position = curve.GetPositionOnCurve(finalLerpValue);
            controllerArrow.RectTransform.up = curve.GetVelocityOnCurve(finalLerpValue);
        }

        // Special case for the final arrow
        controllerArrow = controllerArrowArray[fittableArrowCount];
        finalLerpValue = fittableArrowCount * stepValue + animationLerpValueOffset;

        if (finalLerpValue <= 1)
        {
            controllerArrow.gameObject.SetActive(true);
            controllerArrow.Image.color = arrowColor;
            controllerArrow.RectTransform.position = curve.GetPositionOnCurve(finalLerpValue);
            controllerArrow.RectTransform.up = curve.GetVelocityOnCurve(finalLerpValue);
        } else
        {
            controllerArrow.gameObject.SetActive(false);
        }

        // Hide all the other arrows
        for (int i = fittableArrowCount + 1; i < ARROW_COUNT; i++)
        {
            controllerArrow = controllerArrowArray[i];
            controllerArrow.gameObject.SetActive(false);
        }

        // This arrow is always shown and positioned right on top of the cursor
        controllerCursorArrow.Image.color = arrowColor;
        controllerCursorArrow.RectTransform.position = curve.GetPositionOnCurve(1);
        controllerCursorArrow.RectTransform.up = curve.GetVelocityOnCurve(1);
    }

    private void PopulateArrowArray()
    {
        int i = 0;
        while (i < ARROW_COUNT)
        {
            controllerArrowArray[i++] = Instantiate(prefabArrow, Vector2.zero, Quaternion.identity, transform);
        }
        controllerCursorArrow = Instantiate(prefabArrow, Vector2.zero, Quaternion.identity, transform);
    }

    private void OnUnitHovered(UnitController unit) => hoveredUnit = unit;
    private void OnUnitUnhovered(UnitController unit) => hoveredUnit = null;

    private void Subscribe()
    {
        EventManager.Instance.OnUnitHovered += OnUnitHovered;
        EventManager.Instance.OnUnitUnhovered += OnUnitUnhovered;
    }

    private void Awake()
    {
        PopulateArrowArray();
    }

    private void Start()
    {
        Subscribe();
    }

    private void Update()
    {
        UpdateCurve();
    }

}
