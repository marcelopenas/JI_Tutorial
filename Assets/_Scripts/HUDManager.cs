using UnityEngine;
using TMPro;

public class HUDManager : MonoBehaviour
{
    [SerializeField] private TMP_Text timerText;
    [SerializeField] private TMP_Text coinCountText;

    private void OnEnable()
    {
        GameController.TimerChanged += UpdateTimerText;
        GameController.CoinCountChanged += UpdateCoinText;

        UpdateTimerText(GameController.GetTimer());
        UpdateCoinText(GameController.GetCoinCount());
    }

    private void OnDisable()
    {
        GameController.TimerChanged -= UpdateTimerText;
        GameController.CoinCountChanged -= UpdateCoinText;
    }

    private void UpdateTimerText(int value)
    {
        timerText.text = "Time: " + value.ToString("D3");
    }

    private void UpdateCoinText(int value)
    {
        coinCountText.text = "Coins: " + value.ToString("D3");
    }
}
