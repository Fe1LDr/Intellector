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
    private static GameInfo saved_instance;

    public GameInfo() { }
    public void Save()
    {
        saved_instance = this;
    }
    public static GameInfo Load()
    {
        return saved_instance ?? new();
    }
}
