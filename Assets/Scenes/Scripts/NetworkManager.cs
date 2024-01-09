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

    NetworkStream stream;
    Thread ServerCommunicator;

    bool still_waiting;
    bool ready_for_rematch = false;

    public event Action ExitEvent;
    public event Action RematchEvent;
    

    void Start()
    {
        Settings settings = Settings.Load();
        if (settings.NetworkGame)
        {
            TcpClient client = new TcpClient(settings.ServerIP, 7001);
            stream = client.GetStream();

            WriteLog("Подключение к серверу");
            SendString(password, stream);

            byte wanted_id = Convert.ToByte(settings.Game_ID_To_Connect);
            Networking.SendMessage(wanted_id, stream);

            if (wanted_id == 0)
            {
                SendString(settings.UserName, stream);
                WaitScreen.SetActive(true);

                ServerCommunicator = new Thread(WaitForStart);
                ServerCommunicator.Start();
            }
            else
            {
                TryConnect();
            }
        }
    }

    bool TryConnect()
    {
        const byte no_such_game_ans = 99;
        const byte white_team = 0;
        const byte black_team = 1;

        byte server_ans = RecvMessage(stream);
        WriteLog($"Получено сообщение: {server_ans}");
        if(server_ans == no_such_game_ans)
        {
            WriteLog($"Игра уже не существует");
            stream.Close();
            SceneManager.LoadScene(2);
        }
        if (server_ans == white_team || server_ans == black_team)
        {
            WriteLog($"Противник найден");
            StartGame(server_ans);
            return true;
        }
        return false;
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
        SendMove(new byte[5] { (byte)start.x, (byte)start.y, (byte)end.x, (byte)end.y, (byte)transform_info }, stream);
        WriteLog($"Отправка хода: {start} ; {end} ; {transform_info}");
    }

    void ListenToServer()
    {
        const byte exit_code = 111;
        const byte rematch_code = 222;
        while (true)
        {
            byte[] move = RecvMove(stream);
            WriteLog($"Получен ход:   ({move[0]}, {move[1]}) ; ({move[2]}, {move[3]}) ; {move[4]} ");
            if (move[0] == exit_code)
            {
                ExitReceived();
                return;
            }
            if (move[0] == rematch_code)
            {
                RematchReceived();
                continue;
            }
            ExecuteReceivedMove(move);
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

    void WaitForStart()
    {
        const byte till_waiting_ans = 1;
        const byte cancel_waiting_ans = 0;

        still_waiting = true;
        do
        {
            bool connect = TryConnect();
            if (connect) return;
            else Networking.SendMessage((still_waiting) ? till_waiting_ans : cancel_waiting_ans, stream);
        } while (still_waiting);
    }

    void StartGame(byte ans)
    {
        board.PlayerTeam = Convert.ToBoolean(ans);
        WriteLog($"Назначенный цвет: {(board.PlayerTeam ? "чёрные" : "белые")}");

        board.MoveEvent += MoveEventHandler;
        Thread ServerListener = new Thread(() => ListenToServer());
        ServerListener.Start();
        WaitScreen.SetActive(false);
        return;
    }

    public void CanselWaiting()
    {
        still_waiting = false;
        ServerCommunicator.Join();
        stream.Close();
        SceneManager.LoadScene(2);
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
                board.Restart();
                ready_for_rematch = false;
                return;
            }
        }
    }

    public void SendExit()
    {
        const byte exit_code = 111;
        SendMessageInGame(exit_code, stream);
        stream.Close();
    }
    public void SendRematch()
    {
        const byte rematch_code = 222;
        SendMessageInGame(rematch_code, stream);
    }
}
