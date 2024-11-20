using UnityEngine;
using UnityEngine.SceneManagement;

public class Scenes: MonoBehaviour
{
    public void ChangeScenes(int numberScrenes)
    {
        SceneManager.LoadScene(numberScrenes);
    }

    public void LocalGame()
    {
        var settings = Settings.Load();
        settings.GameMode = GameMode.Local;
        settings.Save();
        ChangeScenes(1);
    }

    public void AIGame()
    {
        var settings = Settings.Load();
        settings.GameMode = GameMode.AI;
        settings.Save();
        ChangeScenes(1);
    }

    public void Exit()
    {
        Application.Quit();
    }
}
