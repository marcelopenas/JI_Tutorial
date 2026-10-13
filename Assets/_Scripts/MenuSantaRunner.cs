using System.Collections;
using UnityEngine;

public class MenuSantaRunner : MonoBehaviour
{
    [SerializeField] private float speed = 180f;
    [SerializeField] private float waitTime = 3f;

    private RectTransform santa;
    private RectTransform canvasRect;

    private void Awake()
    {
        santa = (RectTransform)transform;
        canvasRect = GetComponentInParent<Canvas>().GetComponent<RectTransform>();

        SetFacing(1f);
        santa.anchoredPosition = new Vector2(GetOffscreenX(-1f), santa.anchoredPosition.y);
    }

    private IEnumerator Start()
    {
        while (true)
        {
            yield return RunAcrossScreen(1f);
            yield return new WaitForSeconds(waitTime);

            SetFacing(-1f);
            yield return RunAcrossScreen(-1f);
            yield return new WaitForSeconds(waitTime);

            SetFacing(1f);
        }
    }

    private IEnumerator RunAcrossScreen(float direction)
    {
        float targetX = GetOffscreenX(direction);

        while (!Mathf.Approximately(santa.anchoredPosition.x, targetX))
        {
            Vector2 position = santa.anchoredPosition;
            position.x = Mathf.MoveTowards(position.x, targetX, speed * Time.deltaTime);
            santa.anchoredPosition = position;
            yield return null;
        }
    }

    private float GetOffscreenX(float direction)
    {
        float canvasHalfWidth = canvasRect.rect.width * 0.5f;
        float santaHalfWidth = santa.rect.width * Mathf.Abs(santa.localScale.x) * 0.5f;
        return direction * (canvasHalfWidth + santaHalfWidth);
    }

    private void SetFacing(float direction)
    {
        Vector3 scale = santa.localScale;
        scale.x = Mathf.Abs(scale.x) * direction;
        santa.localScale = scale;
    }
}
