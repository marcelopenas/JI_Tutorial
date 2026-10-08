using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuActions : MonoBehaviour
{
    private const string GameScene = "Game";
    private const string MainMenuScene = "MainMenu";

    public void StartGame()
    {
        GameController.Init();
        SceneManager.LoadScene(GameScene);
    }

    public void Menu()
    {
        SceneManager.LoadScene(MainMenuScene);
    }

    public void Restart()
    {
        GameController.Init();
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
