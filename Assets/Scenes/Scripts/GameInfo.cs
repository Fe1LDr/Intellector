using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

[Serializable()]
public class GameInfo 
{   
    public uint ID { get; set; }
    public string Name { get; set; }
    public TimeContol TimeContol { get; set; }  
    public ColorChoice Color { get; set; }

    public bool Team { get; set; }

    [field: NonSerialized]
    public static string file_path = "game_info.bin";

    public GameInfo() { }
    public void Save()
    {
        using (FileStream stream = new FileStream(file_path, FileMode.OpenOrCreate))
        {
            var binaryFormatter = new BinaryFormatter();
            binaryFormatter.Serialize(stream, this);
        }
    }
    public static GameInfo Load()
    {
        if (File.Exists(file_path))
        {
            var binaryFormatter = new BinaryFormatter();
            using (FileStream stream = new FileStream(file_path, FileMode.Open))
            {
                return (GameInfo)binaryFormatter.Deserialize(stream);
            }
        }
        else
        {
            throw new FileNotFoundException("Файл настроек игры не найден");
        }
    }
}
