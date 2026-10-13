using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class MenuSantaRunner : MonoBehaviour
{
    [SerializeField] private float speed = 180f;
    [SerializeField] private float waitTime = 3f;

    [Header("Sprites & Animation")]
    [SerializeField] private Sprite runningSprite;
    [SerializeField] private float spriteSwitchInterval = 0.25f;

    private Sprite idleSprite;
    private Image santaImage;
    private RectTransform santa;
    private RectTransform canvasRect;

    private void Awake()
    {
        santa = (RectTransform)transform;
        canvasRect = GetComponentInParent<Canvas>().GetComponent<RectTransform>();
        santaImage = GetComponent<Image>();

        if (santaImage != null)
        {
            idleSprite = santaImage.sprite;
        }

        SetFacing(1f);
        santa.anchoredPosition = new Vector2(GetOffscreenX(-1f), santa.anchoredPosition.y);
    }

    private IEnumerator Start()
    {
        while (true)
        {
            if (santaImage != null && idleSprite != null) santaImage.sprite = idleSprite;
            yield return RunAcrossScreen(1f);
            
            if (santaImage != null && idleSprite != null) santaImage.sprite = idleSprite;
            yield return new WaitForSeconds(waitTime);

            SetFacing(-1f);
            yield return RunAcrossScreen(-1f);
            
            if (santaImage != null && idleSprite != null) santaImage.sprite = idleSprite;
            yield return new WaitForSeconds(waitTime);

            SetFacing(1f);
        }
    }

    private IEnumerator RunAcrossScreen(float direction)
    {
        float targetX = GetOffscreenX(direction);
        float spriteSwitchTimer = 0f;
        bool useRunningSprite = true;

        while (!Mathf.Approximately(santa.anchoredPosition.x, targetX))
        {
            Vector2 position = santa.anchoredPosition;
            position.x = Mathf.MoveTowards(position.x, targetX, speed * Time.deltaTime);
            santa.anchoredPosition = position;

            // Toggle between running and idle sprites on a timer while moving
            if (runningSprite != null && idleSprite != null && santaImage != null)
            {
                spriteSwitchTimer += Time.deltaTime;
                if (spriteSwitchTimer >= spriteSwitchInterval)
                {
                    spriteSwitchTimer = 0f;
                    useRunningSprite = !useRunningSprite;
                }
                santaImage.sprite = useRunningSprite ? runningSprite : idleSprite;
            }

            yield return null;
        }

        // Ensure it reverts to idle when movement finishes
        if (santaImage != null && idleSprite != null)
        {
            santaImage.sprite = idleSprite;
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