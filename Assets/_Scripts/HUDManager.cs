using System;
using System.Linq;
using UnityEngine;
using TMPro;

public class HUDManager : MonoBehaviour
{
    [SerializeField] private TMP_Text timerText;
    [SerializeField] private TMP_Text coinCountText;
    [SerializeField] private GameObject[] healthImages;

    private void OnEnable()
    {
        GameController.TimerChanged += UpdateTimerText;
        GameController.CoinCountChanged += UpdateCoinText;
        GameController.PlayerLivesChanged += UpdateHealthText;

        UpdateTimerText(GameController.GetTimer());
        UpdateCoinText(GameController.GetCoinCount());
        FindHealthImagesIfNeeded();
        UpdateHealthText(GameController.GetPlayerLives());
    }

    private void OnDisable()
    {
        GameController.TimerChanged -= UpdateTimerText;
        GameController.CoinCountChanged -= UpdateCoinText;
        GameController.PlayerLivesChanged -= UpdateHealthText;
    }

    private void UpdateTimerText(int value)
    {
        timerText.text = "Time: " + value.ToString("D3");
    }

    private void UpdateCoinText(int value)
    {
        coinCountText.text = "Coins: " + value.ToString("D3");
    }

    private void UpdateHealthText(int value)
    {
        FindHealthImagesIfNeeded();

        int visibleHealth = Mathf.Clamp(value, 0, healthImages.Length);
        for (int i = 0; i < healthImages.Length; i++)
        {
            if (healthImages[i] != null)
            {
                healthImages[i].SetActive(i < visibleHealth);
            }
        }
    }

    private void FindHealthImagesIfNeeded()
    {
        if (healthImages != null && healthImages.Length > 0)
        {
            return;
        }

        healthImages = GameObject.FindGameObjectsWithTag("HUDHealth")
            .OrderBy(healthImage => healthImage.name, StringComparer.Ordinal)
            .ToArray();
    }
}
