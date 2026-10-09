using System;
using UnityEngine;

public static class GameController
{
    private static int coinCount;
    private static int timer; // In seconds
    private static int playerLives; // Number of lives the player has
    private static float timerAccumulator;
    private static float playerInvulnerabilityRemaining;
    private static bool initialized;
    private static bool gameOverRaised;

    public static event Action<int> TimerChanged;
    public static event Action<int> CoinCountChanged;
    public static event Action<int> PlayerLivesChanged;
    public static event Action<float> PlayerDamaged;
    public static event Action GameOver;

    public const float PlayerInvulnerabilityDuration = 3f;
    public static bool IsInitialized => initialized;
    public static bool IsPlayerInvulnerable => playerInvulnerabilityRemaining > 0f;

    public static void Init()
    {
        coinCount = 0;
        playerLives = 3;
        timer = 100;
        timerAccumulator = 0f;
        playerInvulnerabilityRemaining = 0f;
        initialized = true;
        gameOverRaised = false;

        TimerChanged?.Invoke(timer);
        CoinCountChanged?.Invoke(coinCount);
        PlayerLivesChanged?.Invoke(playerLives);
        Debug.Log("GameController initialized. Coin count set to 0.");
    }

    public static void AddCoin()
    {
        coinCount++;
        CoinCountChanged?.Invoke(coinCount);
        Debug.Log("Coins collected: " + coinCount);

        RaiseGameOverIfNeeded();
    }

    public static void ResetCoinCount()
    {
        coinCount = 0;
        CoinCountChanged?.Invoke(coinCount);
        Debug.Log("Coin count reset.");
    }

    public static int GetCoinCount()
    {
        return coinCount;
    }

    public static bool IsGameOver()
    {
        return coinCount >= 3 || timer <= 0 || playerLives <= 0;
    }

    public static void DecreaseTimer()
    {
        timer = Mathf.Max(0, timer - 1);
        TimerChanged?.Invoke(timer);
        Debug.Log("Timer decreased. Remaining time: " + timer);

        RaiseGameOverIfNeeded();
    }

    public static void UpdateTimer(float deltaTime)
    {
        if (!initialized)
        {
            return;
        }

        playerInvulnerabilityRemaining = Mathf.Max(0f, playerInvulnerabilityRemaining - deltaTime);

        if (IsGameOver())
        {
            return;
        }

        timerAccumulator += deltaTime;

        while (timerAccumulator >= 1f && timer > 0)
        {
            timerAccumulator -= 1f;
            DecreaseTimer();
        }
    }

    public static int GetTimer()
    {
        return timer;
    }

    public static void DamagePlayer()
    {
        if (!initialized || IsGameOver() || IsPlayerInvulnerable)
        {
            return;
        }

        playerLives = Mathf.Max(0, playerLives - 1);
        playerInvulnerabilityRemaining = PlayerInvulnerabilityDuration;
        PlayerDamaged?.Invoke(PlayerInvulnerabilityDuration);
        PlayerLivesChanged?.Invoke(playerLives);
        Debug.Log("Player damaged! Remaining lives: " + playerLives);
        RaiseGameOverIfNeeded();
    }

    public static int GetPlayerLives()
    {
        return playerLives;
    }

    private static void RaiseGameOverIfNeeded()
    {
        if (IsGameOver() && !gameOverRaised)
        {
            gameOverRaised = true;
            GameOver?.Invoke();
        }
    }

}
