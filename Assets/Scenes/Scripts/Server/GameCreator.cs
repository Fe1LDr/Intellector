using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Net.Sockets;
using System.Runtime.InteropServices.ComTypes;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;
using static Networking;


public interface IGameCreator
{
    public void CreateGame(GameInfo gameInfo, Action onConnect);
    public void CancelGameCreate();
}
public class GameCreator : IGameCreator
{
    private static bool still_waiting;
    private GameInfo gameInfo;
    private Task waiter;
    private bool connected;

    public async void CreateGame(GameInfo gameInfo, Action onConnect)
    {
        this.gameInfo = gameInfo;
        still_waiting = true;
        waiter = Task.Run(() => ConnectionWait());
        await waiter;
        if (connected)
        {
            onConnect();
        }
        else
        {
            ServerConnection.GetConnection().Close();
        }
    }

    public void CancelGameCreate()
    {
        still_waiting = false;
    }

    private void ConnectionWait()
    {
        const byte white_team_code = 0;
        const byte black_team_code = 1;
        const byte create_game_request_code = 40;
        const byte continue_waiting_code = 1;
        const byte stop_code = 0;

        TcpClient server = ServerConnection.GetConnection().Client;
        NetworkStream stream = server.GetStream();

        SendCode(create_game_request_code, stream);
        SendGameInfo(gameInfo, stream);
        do
        {
            byte server_ans = RecvCode(stream);
            if (server_ans == white_team_code || server_ans == black_team_code)
            {
                gameInfo.Team = false;
                gameInfo.Save();
                still_waiting = false;
                connected = true;
                return;
            }
            SendCode((still_waiting) ? continue_waiting_code : stop_code, stream);
        } while (still_waiting);         
    }
}
