using System.Net.Sockets;
using UnityEngine;
using System.IO;
using System.Threading;
using System;
using System.Text;
using UnityEngine.SceneManagement;

public class NetworkManager : MonoBehaviour
{
    [SerializeField] private Board board;
    [SerializeField] private GameObject WaitScreen;
    TcpClient client;
    NetworkStream stream;
    Thread ServerCommunicator;

    byte[] TillWaitingAnswer = new byte[1] { 1 };
    byte[] ReceivedMoveBytes = new byte[5];

    private const string password = "a3P1>8]Ы-/йЧяЭ975?:$qcDыФ9&e@1a<c{a/";
    const string LogFilePath = "log.txt";

    void Start()
    {
        Settings settings = Settings.Load();
        if (settings.NetworkGame)
        {
            client = new TcpClient(settings.ServerIP, 7001);
            stream = client.GetStream();

            WriteLog("Подключение к серверу");

            byte[] password_bytes = Encoding.Default.GetBytes(password);
            stream.Write(password_bytes, 0, password_bytes.Length);

            byte[] SentIdBytes = new byte[1];
            SentIdBytes[0] = Convert.ToByte(settings.Game_ID_To_Connect);
            stream.Write(SentIdBytes, 0, 1);

            if (settings.Game_ID_To_Connect == 0)
            {
                byte[] name_bytes = Encoding.Default.GetBytes(settings.UserName);
                stream.Write(name_bytes, 0, name_bytes.Length);
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
        byte[] GetFromServerBytes = new byte[1];
        stream.Read(GetFromServerBytes, 0, 1);

        byte ans = GetFromServerBytes[0];
        WriteLog($"Получено сообщение: {GetFromServerBytes[0]}");
        if(ans == 99)
        {
            WriteLog($"Игра уже не существует");

            client.Close();
            SceneManager.LoadScene(2);
        }
        if (ans == 0 || ans == 1)
        {
            WriteLog($"Противник найден");
            StartGame(ans);
            return true;
        }
        return false;
    }

    public void ReceiveMove()
    {
        Vector2Int start = new Vector2Int(ReceivedMoveBytes[0], ReceivedMoveBytes[1]);
        Vector2Int end = new Vector2Int(ReceivedMoveBytes[2], ReceivedMoveBytes[3]);
        int transform_info = ReceivedMoveBytes[4];
        board.MovePiece(start, end, true, transform_info);
    }

    void MoveEventHandler(Vector2Int start, Vector2Int end, int transform_info)
    {
        byte[] MoveBytes = new byte[5] { (byte)start.x, (byte)start.y, (byte)end.x, (byte)end.y, (byte)transform_info };
        stream.Write(MoveBytes, 0, 5);
        WriteLog($"Отправка хода: {start} ; {end} ; {transform_info}");
    }

    void ListenToServer()
    {
        while (true)
        {
            stream.Read(ReceivedMoveBytes, 0, 5);
            WriteLog($"Получен ход:   ({ReceivedMoveBytes[0]}, {ReceivedMoveBytes[1]}) ; ({ReceivedMoveBytes[2]}, {ReceivedMoveBytes[3]}) ; {ReceivedMoveBytes[4]} ");
            if (ReceivedMoveBytes[0] == 111)
            {
                WriteLog($"Противник вышел :(");
                board.GameOver(board.PlayerTeam, true);
                return;
            }
            ReceiveMove();
        }
    }

    void WaitForStart()
    {
        do
        {
            bool connect = TryConnect();
            if (connect) return;
            else stream.Write(TillWaitingAnswer, 0, 1);
        } while (TillWaitingAnswer[0] == 1);
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

    void WriteLog(string messeage)
    {
        try
        {
            using (StreamWriter LogStream = new StreamWriter(LogFilePath, true))
            {
                LogStream.WriteLine(messeage);
            }
        }
        catch (Exception) { }
    }

    public void CanselWaiting()
    {
        TillWaitingAnswer[0] = 0;
        ServerCommunicator.Join();
        stream.Close();
        SceneManager.LoadScene(2);
    }

    public void SendExit()
    {
        byte[] exit = new byte[5] { 111, 0, 0, 0, 0 };
        stream.Write(exit, 0, 5);
        stream.Close();
    }
}
