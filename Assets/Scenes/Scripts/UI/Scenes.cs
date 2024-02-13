using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Scenes: MonoBehaviour
{
    // Start is called before the first frame update
    public void ChangeScenes(int numberScrenes)
    {
        SceneManager.LoadScene(numberScrenes);
    }
    public void Exit()
    {
        Application.Quit();
    }

    public void LocalGame() => StartGame(false, false);
    public void NetworkGame() => StartGame(true, false);
    public void AIGame() => StartGame(false, true);
    private void StartGame(bool network, bool AI)
    {
        Settings settings = Settings.Load();
        settings.NetworkGame = network;
        settings.AIGame = AI;
        settings.Save();
        ChangeScenes(1);
    }

    
}
