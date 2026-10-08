using UnityEngine;

public static class GameController
{
    private static int coinCount;
    private static int timer; // In seconds
    private static float timerAccumulator;

    public static void Init()
    {
        coinCount = 0;
        timer = 10;
        timerAccumulator = 0f;
        Debug.Log("GameController initialized. Coin count set to 0.");
    }

    public static void AddCoin()
    {
        coinCount++;
        Debug.Log("Coins collected: " + coinCount);
    }

    public static void ResetCoinCount()
    {
        coinCount = 0;
        Debug.Log("Coin count reset.");
    }

    public static int GetCoinCount()
    {
        return coinCount;
    }

    public static bool gameOver()
    {
        return coinCount >= 3 || timer <= 0;
    }

    public static void DecreaseTimer()
    {
        timer = Mathf.Max(0, timer - 1);
        Debug.Log("Timer decreased. Remaining time: " + timer);
    }

    public static void UpdateTimer(float deltaTime)
    {
        if (timer <= 0 || gameOver())
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
}
