using System.Collections;
using System.Collections.Generic;
using System.Net.Sockets;
using UnityEngine;
using static Networking;


public class ServerConnection
{
    public TcpClient Client { get; private set; }

    public const string password = "a3P1>8]Ы-/йЧяЭ975?:$qcDыФ9&e@1a<c{a/";
    private static string server_ip = "194.87.235.152";
    private static string local_ip = "192.168.1.6";
    public static int server_port = 7002;

    private static ServerConnection Instance;

    private ServerConnection(TcpClient client)
    {
        this.Client = client;
    }

    public void Close() => Client.Close();

    public static ServerConnection GetInstance()
    {
        if (Instance == null || !Instance.Client.Connected) Instance = new ServerConnection(ConnectToServer());
        return Instance;
    }

    private static TcpClient ConnectToServer()
    {
        TcpClient client = new TcpClient(local_ip, server_port);

        SendString(password, client.GetStream());
        CheckVersion(client.GetStream());

        return client;
    }

    private static void CheckVersion(NetworkStream stream)
    {
        SendInt(Settings.version, stream);
        int server_version = RecvInt(stream);
        if (Settings.version != server_version)
        {
            throw new VersionException(
             $"\"Неподходящая версия\\n" +
             $"Версия сервера - {VerToStr(server_version)}\\n" +
             $"Используемая версия клиента - {VerToStr(Settings.version)}\\n\""
             );
        }

        string VerToStr(int ver) => $"{ver / 10}.{ver % 10}";
    }
}
