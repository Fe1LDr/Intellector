using Assets.Scenes.Scripts;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class ConnectionWaiter : MonoBehaviour
{
    [SerializeField] GameObject WaitingScreen;

    public bool still_waiting;

    private GameInfo gameInfo;
    private Thread waiter;
    private bool connect;
    private JoinInfo joinInfo;

    public void StartWaiting(GameInfo gameInfo)
    {
        this.gameInfo = gameInfo;

        WaitingScreen.SetActive(true);
        still_waiting = true;
        waiter = new Thread(WaitForConnect);
        waiter.Start();
        waiter.Join();
        if (connect)
        {
            WaitingScreen.SetActive(false);
            GameStarter.StartGame(gameInfo, joinInfo);
        }
    }
    public void CancelWaiting()
    {
        still_waiting = false;
    }

    private async void WaitForConnect(object obj)
    {
        while(still_waiting)
        {
            (connect,  joinInfo) = await ServerManager.PostRequest<(bool, JoinInfo)>($"Refresh/{gameInfo.ID}");
            if (connect) return;
        }
    }
}
