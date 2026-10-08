using UnityEngine;

public class UIManager : MonoBehaviour
{
    public GameObject endGamePanel;

    void FixedUpdate()
    {
        if (GameController.gameOver())
        {
            Debug.Log("Game Over!");
            endGamePanel.SetActive(true);
        }
        else {
            endGamePanel.SetActive(false);
        }
    }
}
