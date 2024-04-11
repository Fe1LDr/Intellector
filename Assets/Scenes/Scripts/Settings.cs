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
    private static string config_file_path = "config.bin";
    private static string server_connection_file_path = "connection.json";
    public static int version = 15;
    public static IServerFactory ServerFactory { get; private set; }
    public static ServerConnectionSettings ServerConnection { get; private set; }
    static Settings()
    {
        ServerFactory = new TCPServerFactory();
        ServerConnection = JsonUtility.FromJson<ServerConnectionSettings>(File.ReadAllText(server_connection_file_path));
    }

    public GameMode GameMode { get; set; }
    public string UserName { get; set; }
    public AvaibleMaterials Material { get; set; }

    private Settings()
    {
        UserName = String.Empty;
        Material = AvaibleMaterials.Standard;
    }
    public void Save()
    {
        using (FileStream stream = new FileStream(config_file_path, FileMode.OpenOrCreate))
        {
            var binaryFormatter = new BinaryFormatter();
            binaryFormatter.Serialize(stream, this);
        }
    }

    public static Settings Load()
    {
        if (File.Exists(config_file_path))
        {
            var binaryFormatter = new BinaryFormatter();
            using (FileStream stream = new FileStream(config_file_path, FileMode.Open))
            {
                return (Settings)binaryFormatter.Deserialize(stream);
            }
        }
        else
        {
            return new Settings();
        }
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
