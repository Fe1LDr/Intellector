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

            Debug.Log("����������� � �������");
            using (StreamWriter LogStream = new StreamWriter(LogFilePath)) { LogStream.WriteLine("����������� � �������"); }
            stream.Write(SentByClientBytes, 0, 1);

            Debug.Log("����� ����");
            using (StreamWriter LogStream = new StreamWriter(LogFilePath, true)) { LogStream.WriteLine("����� ����"); }
 
            byte[] GetFromServerBytes = new byte[1];
            stream.Read(GetFromServerBytes, 0, 1);
       
            if (GetFromServerBytes[0] == 0 || GetFromServerBytes[0] == 1)
            {
                Debug.Log("���� �������");
                using (StreamWriter LogStream = new StreamWriter(LogFilePath, true)) { LogStream.WriteLine("���� �������"); }

                board.PlayerTeam = Convert.ToBoolean(GetFromServerBytes[0]);
                Debug.Log($"����������� ����: {(board.PlayerTeam?"������":"�����")}");
                using (StreamWriter LogStream = new StreamWriter(LogFilePath, true)) { LogStream.WriteLine($"����������� ����: {(board.PlayerTeam ? "������" : "�����")}"); }

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
        Debug.Log($"������� ���: {start} ; {end}");
        using (StreamWriter LogStream = new StreamWriter(LogFilePath, true))
        {
            LogStream.WriteLine($"������� ���: {start} ; {end}");
            board.MovePiece(start, end, true);
        }  
    }

    void MoveEventHandler(Vector2Int start, Vector2Int end)
    {
        byte[] MoveBytes = new byte[4] { (byte)start.x, (byte)start.y, (byte)end.x, (byte)end.y };
        stream.Write(MoveBytes, 0, 4);
        Debug.Log($"�������� ���� {start} ; {end}");
        using (StreamWriter LogStream = new StreamWriter(LogFilePath, true)) { LogStream.WriteLine($"�������� ���� {start} ; {end}"); }
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
