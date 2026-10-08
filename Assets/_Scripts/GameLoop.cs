using UnityEngine;

public class GameLoop : MonoBehaviour
{
    private void Awake()
    {
        if (!GameController.IsInitialized)
        {
            GameController.Init();
        }
    }

    private void Update()
    {
        GameController.UpdateTimer(Time.deltaTime);
    }
}
