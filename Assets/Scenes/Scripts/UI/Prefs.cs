using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

[Serializable()]
public class Settings
{
    [field: NonSerialized]
    public static string config_file_path = "config.bin";

    public string ServerIP { get; set; }
    public bool NetworkGame { get; set; }
    public string Material { get; set; }

    public Settings()
    {
        ServerIP = "194.87.235.152";
        NetworkGame = false;
        Material = "";
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