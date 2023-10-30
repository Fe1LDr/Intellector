using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Scenes : MonoBehaviour
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
}
