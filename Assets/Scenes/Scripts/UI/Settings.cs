using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using UnityEngine;

[Serializable()]
public class Settings
{
    [field: NonSerialized]
    private static string config_file_path = "config.bin";
    private static string server_ip = "194.87.235.152";
    private static string local_ip = "192.168.1.5";
    public static int server_port = 7002;
    public static int version = 14;

    public string ServerIP { get; set; }
    public uint Game_ID_To_Connect { get; set; }
    public bool NetworkGame { get; set; }
    public bool AIGame { get; set; }
    public string UserName { get; set; }
    public AvaibleMaterials Material { get; set; }

    public Settings()
    {
        ServerIP = server_ip;
        Game_ID_To_Connect = 0;
        NetworkGame = false;
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
