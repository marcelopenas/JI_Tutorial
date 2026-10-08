using UnityEngine;
using TMPro;

public class HUDManager : MonoBehaviour
{
    public GameObject timerText;
    public GameObject coinCountText;

    private TMP_Text timerTmpText;
    private TMP_Text coinTmpText;

    private void Awake()
    {
        timerTmpText = timerText.GetComponent<TMP_Text>();
        coinTmpText = coinCountText.GetComponent<TMP_Text>();
    }

    private void FixedUpdate()
    {
        GameController.UpdateTimer(Time.fixedDeltaTime);

        int coinCount = GameController.GetCoinCount();
        int remainingTime = GameController.GetTimer();

        timerTmpText.text = "Time: " + remainingTime.ToString("D3");
        coinTmpText.text = "Coins: " + coinCount.ToString("D3");
    }
}
