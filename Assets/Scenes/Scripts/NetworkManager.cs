using System.Net.Sockets;
using UnityEngine;
using System.IO;
using System.Threading;
using System;
using System.Text;
using UnityEngine.SceneManagement;
using static Networking;
using static LogWriter;

public class NetworkManager : MonoBehaviour
{
    [SerializeField] private Board board;
    [SerializeField] private GameObject WaitScreen;

    NetworkStream server_stream;

    bool ready_for_rematch = false;

    public event Action ExitEvent;
    public event Action RematchEvent;
    public event Action GameStartEvent;

    public delegate void TimeReceived(int time);
    public event TimeReceived TimeEvent;
    

    void Start()
    {
        Settings settings = Settings.Load();
        if (settings.GameMode == GameMode.Network)
        {
            ServerConnection connection = ServerConnection.GetInstance();
            server_stream = connection.Client.GetStream();

            board.MoveEvent += MoveEventHandler;
            Thread ServerListener = new Thread(() => ListenToServer());
            ServerListener.Start();
            GameStartEvent?.Invoke();
        }
    }

    public void ExecuteReceivedMove(byte[] move)
    {
        Vector2Int start = new Vector2Int(move[0], move[1]);
        Vector2Int end = new Vector2Int(move[2], move[3]);
        int transform_info = move[4];
        try
        {
           board.MovePiece(start, end, true, transform_info);
        }
        catch (Exception e)
        {
            WriteLog(e.Message);
        }
    }

    void MoveEventHandler(Vector2Int start, Vector2Int end, int transform_info)
    {
        SendMove(new byte[5] { (byte)start.x, (byte)start.y, (byte)end.x, (byte)end.y, (byte)transform_info }, server_stream);
        WriteLog($"Отправка хода: {start} ; {end} ; {transform_info}");
    }

    void ListenToServer()
    {
        const byte move_code = 10;
        const byte time_code = 20;
        const byte white_time_out_code = 30;
        const byte black_time_out_code = 31;
        const byte exit_code = 111;
        const byte rematch_code = 222;
        while (true)
        {
            byte code = RecvCode(server_stream);
            switch (code)
            {
                case move_code:
                    byte[] move = RecvMove(server_stream);
                    WriteLog($"Получен ход:   ({move[0]}, {move[1]}) ; ({move[2]}, {move[3]}) ; {move[4]} ");
                    MainTasks.AddTask(() => ExecuteReceivedMove(move));
                    break;
                case time_code:
                    int time = RecvInt(server_stream);
                    WriteLog($"осталось времени: {time}");
                    MainTasks.AddTask(() => TimeEvent?.Invoke(time));
                    break;
                case exit_code:
                    MainTasks.AddTask(() => ExitReceived());
                    break;
                case white_time_out_code:
                    MainTasks.AddTask(() => board.GameOver(true));
                    break;
                case black_time_out_code:
                    MainTasks.AddTask(() => board.GameOver(false));
                    break;
                case rematch_code:
                    MainTasks.AddTask(() => RematchReceived());
                    break;
            }
        }

        void ExitReceived()
        {
            WriteLog($"Противник вышел");
            board.GameOver(board.PlayerTeam, true);
            ExitEvent?.Invoke();
        }
        void RematchReceived()
        {
            ready_for_rematch = true;
            RematchEvent?.Invoke();
        }
    }

    public void AskRematch()
    {
        if (board.NetworkGame)
        {
            SendRematch();
            Thread RematchWaiter = new Thread(WaitForRematch);
            RematchWaiter.Start();
        }
        else
        {
            board.Restart();
        }
    }

    private void WaitForRematch()
    {
        while (true)
        {
            if (ready_for_rematch)
            {
                MainTasks.AddTask(() => board.Restart());
                ready_for_rematch = false;
                return;
            }
        }
    }
    public void SendExit()
    {
        const byte exit_code = 111;
        SendCode(exit_code, server_stream);
        server_stream.Close();
    }
    public void SendRematch()
    {
        const byte rematch_code = 222;
        SendCode(rematch_code, server_stream);
    }
}
