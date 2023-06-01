using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CardSelectArrowController : MonoBehaviour
{

    [Header("Components")]
    [SerializeField] private CanvasGroup canvasGroup;

    [Header("Prefabs")]
    [SerializeField] private RectTransform prefabArrow;

	private CardController preparedCard;
    private BezierCurve curve = new();

    private const int ARROW_COUNT = 64;
    private const int DISTANCE_PER_ARROW = 24;
    private const float CURVINESS = 64;

    private RectTransform[] rectTransArrowArray = new RectTransform[ARROW_COUNT];
    private RectTransform rectTransCursorArrow;

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
    /// Turn off the arrow curve when the player isn't currently selecting a card's target.
    /// </summary>
    public void TurnOff()
    {
        preparedCard = null;
        canvasGroup.alpha = 0;
    }

    private void UpdateArrow()
    {
        arrowAnimationLerpValue = (arrowAnimationLerpValue + Time.deltaTime) % 1;

        if (preparedCard == null) return;
        if (rectTransArrowArray[0] == null) return;

        Vector2 startPos = preparedCard.Position;
        Vector2 endPos = Input.mousePosition;
        Vector2 firstControl = Vector2.Lerp(startPos, endPos, 1 / 3f);
        Vector2 secondControl = Vector2.Lerp(startPos, endPos, 3 / 4f);

        Vector2 startEndUnit = (endPos - startPos).normalized;
        Vector2 startEndRight = UtilMath.RotateVector(startEndUnit, Mathf.PI / 2);

        bool isOffsetToTheRight = endPos.x >= preparedCard.Position.x;
        float offsetSign = isOffsetToTheRight ? 1 : -1;

        curve.P0 = startPos;
        curve.P1 = firstControl + offsetSign * CURVINESS * startEndRight;
        curve.P2 = secondControl + offsetSign * CURVINESS * startEndRight;
        curve.P3 = endPos;

        // Now we try to populate the arrows
        float arcLength = curve.GetLength();

        int fittableArrowCount = (int)(arcLength / DISTANCE_PER_ARROW);
        float fittableLength = fittableArrowCount * DISTANCE_PER_ARROW;

        float maxLerpValue = fittableLength / arcLength;
        float stepValue = maxLerpValue / fittableArrowCount;

        float animationLerpValueOffset = Mathf.Lerp(0, stepValue, arrowAnimationLerpValue);

        RectTransform arrowRectTrans;
        float finalLerpValue;

        // Show the arrows that can fit in the curve
        for (int i = 0; i < fittableArrowCount; i++)
        {
            arrowRectTrans = rectTransArrowArray[i];
            finalLerpValue = i * stepValue + animationLerpValueOffset;

            arrowRectTrans.gameObject.SetActive(true);
            arrowRectTrans.position = curve.GetPositionOnCurve(finalLerpValue);
            arrowRectTrans.up = curve.GetVelocityOnCurve(finalLerpValue);
        }

        // Special case for the final arrow
        arrowRectTrans = rectTransArrowArray[fittableArrowCount];
        finalLerpValue = fittableArrowCount * stepValue + animationLerpValueOffset;

        if (finalLerpValue <= 1)
        {
            arrowRectTrans.gameObject.SetActive(true);
            arrowRectTrans.position = curve.GetPositionOnCurve(finalLerpValue);
            arrowRectTrans.up = curve.GetVelocityOnCurve(finalLerpValue);
        } else
        {
            arrowRectTrans.gameObject.SetActive(false);
        }

        // Hide all the other arrows
        for (int i = fittableArrowCount + 1; i < ARROW_COUNT; i++)
        {
            arrowRectTrans = rectTransArrowArray[i];
            arrowRectTrans.gameObject.SetActive(false);
        }

        // This arrow is always shown and positioned right on top of the cursor
        rectTransCursorArrow.position = curve.GetPositionOnCurve(1);
        rectTransCursorArrow.up = curve.GetVelocityOnCurve(1);
    }

    private void PopulateArrowArray()
    {
        int i = 0;
        while (i < ARROW_COUNT)
        {
            rectTransArrowArray[i++] = Instantiate(prefabArrow, Vector2.zero, Quaternion.identity, transform);
        }
        rectTransCursorArrow = Instantiate(prefabArrow, Vector2.zero, Quaternion.identity, transform);
    }

    private void Awake()
    {
        PopulateArrowArray();
    }

    private void Update()
    {
        UpdateArrow();
    }

}
