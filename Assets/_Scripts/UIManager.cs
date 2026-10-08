using UnityEngine;

public class UIManager : MonoBehaviour
{
    [SerializeField] private GameObject endGamePanel;

    private void OnEnable()
    {
        GameController.GameOver += ShowEndGamePanel;
        SetEndGamePanel(GameController.IsGameOver());
    }

    private void OnDisable()
    {
        GameController.GameOver -= ShowEndGamePanel;
    }

    private void ShowEndGamePanel()
    {
        SetEndGamePanel(true);
        Debug.Log("Game Over!");
    }

    private void SetEndGamePanel(bool visible)
    {
        endGamePanel.SetActive(visible);
    }
}
