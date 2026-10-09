using UnityEngine;
using TMPro;

public class UIManager : MonoBehaviour
{
    [SerializeField] private GameObject endGamePanel;
    [SerializeField] private TMP_Text gameOverTitle;

    private void OnEnable()
    {
        GameController.GameOver += ShowEndGamePanel;
        if (GameController.IsInitialized && GameController.IsGameOver())
        {
            ShowEndGamePanel(GameController.GetGameOverReason());
        }
        else
        {
            SetEndGamePanel(false);
        }
    }

    private void OnDisable()
    {
        GameController.GameOver -= ShowEndGamePanel;
    }

    private void ShowEndGamePanel(string reason)
    {
        if (gameOverTitle != null)
        {
            gameOverTitle.text = $"Game over\nReason: {reason}";
        }

        SetEndGamePanel(true);
        Debug.Log($"Game Over: {reason}");
    }

    private void SetEndGamePanel(bool visible)
    {
        endGamePanel.SetActive(visible);
    }
}
