using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

[Serializable()]
public class Settings
{
    [field: NonSerialized]
    public static string config_file_path = "config.bin";
    private static string server_ip = "194.87.235.152";
    private static string local_ip = "192.168.1.5";

    public string ServerIP { get; set; }
    public uint Game_ID_To_Connect { get; set; }
    public bool NetworkGame { get; set; }
    
    public string UserName { get; set; }
    public string Material { get; set; }

    public Settings()
    {
        ServerIP = server_ip;
        Game_ID_To_Connect = 0;
        NetworkGame = false;
        UserName = String.Empty;
        Material = String.Empty;
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
}
