using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameStarter 
{
    public static void StartGame(GameInfo gameInfo, JoinInfo joinInfo)
    {
        switch (joinInfo.Team)
        {
            case true: gameInfo.Color = ColorChoice.white; break;
            case false: gameInfo.Color = ColorChoice.black; break;
        }

        gameInfo.Port = joinInfo.Port;
        gameInfo.JoinCode = joinInfo.JoinCode;
        gameInfo.Save();
        SceneManager.LoadScene(1);
    }
}
