using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using UnityEngine;

public enum GameMode { Local, Network, AI }

[Serializable]
public class ServerConnectionSettings
{
    public string Password; 
    public string ServerIP;
    public int Port;
}

[Serializable()]
public class Settings
{
    [field: NonSerialized]
    public static int version = 16;
    public static IServerFactory ServerFactory { get; private set; }
    public static ServerConnectionSettings ServerConnection { get; private set; }
    static Settings()
    {
        ServerFactory = new TCPServerFactory();
        ServerConnection = new ServerConnectionSettings()
        {
            Password = "kW2й@#daцкйw;!oAW}3Dф<вP)Idцк4awd345Aввas*da$F?dsfgУКчф6a>Wxa{",
            Port = 7003,
            ServerIP = "194.87.235.152"
        };
    }

    static Settings _instance = new Settings();
    public GameMode GameMode { get; set; }
    public string UserName { get; set; }
    public PieceMaterials Material { get; set; }

    private Settings()
    {
        UserName = String.Empty;
        Material = PieceMaterials.Standard;
    }
    public void Save()
    {
        _instance = this;
    }

    public static Settings Load()
    {
        return _instance;       
    }
    static public bool CheckName(string name, out string error_message)
    {
        if (String.IsNullOrEmpty(name))
        {
            error_message = "Имя не должно быть пустым";
            return false;
        }
        if (Encoding.Default.GetBytes(name).Length > 20)
        {
            error_message = "Имя не должно быть длинне 20 символов";
            return false;
        }
        error_message = null;
        return true;
    }
}
