using System.Collections;
using System.Collections.Generic;
using System.Net.Sockets;
using UnityEngine;
using System.IO;
using System.Threading;
using System;

public class NetworkManager : MonoBehaviour
{
    [SerializeField] private Board board;
    TcpClient client;
    NetworkStream stream;


    byte[] ReceivedMoveBytes = new byte[4];
    const string LogFilePath = "log.txt";

    void Start()
    {
        if (board.NetworkGame)
        {
            client = new TcpClient("192.168.1.6", 7000);
            byte[] SentByClientBytes = new byte[1] { 1 };
            stream = client.GetStream();

            Debug.Log("Подключение к серверу");
            using (StreamWriter LogStream = new StreamWriter(LogFilePath)) { LogStream.WriteLine("Подключение к серверу"); }
            stream.Write(SentByClientBytes, 0, 1);

            Debug.Log("Поиск игры");
            using (StreamWriter LogStream = new StreamWriter(LogFilePath, true)) { LogStream.WriteLine("Поиск игры"); }
 
            byte[] GetFromServerBytes = new byte[1];
            stream.Read(GetFromServerBytes, 0, 1);
       
            if (GetFromServerBytes[0] == 0 || GetFromServerBytes[0] == 1)
            {
                Debug.Log("Игра найдена");
                using (StreamWriter LogStream = new StreamWriter(LogFilePath, true)) { LogStream.WriteLine("Игра найдена"); }

                board.PlayerTeam = Convert.ToBoolean(GetFromServerBytes[0]);
                Debug.Log($"Назначенный цвет: {(board.PlayerTeam?"чёрные":"белые")}");
                using (StreamWriter LogStream = new StreamWriter(LogFilePath, true)) { LogStream.WriteLine($"Назначенный цвет: {(board.PlayerTeam ? "чёрные" : "белые")}"); }

            }

       
            board.MoveEvent += MoveEventHandler;
            Thread ServerListener = new Thread(() => ListenToServer());
            ServerListener.Start();
        }
    }


    public void ReceiveMove()
    {
        Vector2Int start = new Vector2Int(ReceivedMoveBytes[0], ReceivedMoveBytes[1]);
        Vector2Int end = new Vector2Int(ReceivedMoveBytes[2], ReceivedMoveBytes[3]);
        Debug.Log($"Получен ход: {start} ; {end}");
        using (StreamWriter LogStream = new StreamWriter(LogFilePath, true))
        {
            LogStream.WriteLine($"Получен ход: {start} ; {end}");
            board.MovePiece(start, end, true);
        }  
    }

    void MoveEventHandler(Vector2Int start, Vector2Int end)
    {
        byte[] MoveBytes = new byte[4] { (byte)start.x, (byte)start.y, (byte)end.x, (byte)end.y };
        stream.Write(MoveBytes, 0, 4);
        Debug.Log($"Отправка хода {start} ; {end}");
        using (StreamWriter LogStream = new StreamWriter(LogFilePath, true)) { LogStream.WriteLine($"Отправка хода {start} ; {end}"); }
        //ReceiveMove();
    }

    void ListenToServer()
    {
        while (true)
        {
            stream.Read(ReceivedMoveBytes, 0, 4);
            ReceiveMove();
        }      
    }
}
