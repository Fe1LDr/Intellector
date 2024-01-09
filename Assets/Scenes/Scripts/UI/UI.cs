using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UI : MonoBehaviour
{
    [SerializeField] NetworkManager networkManager;
    [SerializeField] Board board;
    public void Exit()
    {
        if (board.NetworkGame)
            networkManager.SendExit();
        SceneManager.LoadScene(0);
    }
}
