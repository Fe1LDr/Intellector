using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameInfo 
{   
    public uint ID { get; set; }
    public string Name { get; set; }

    public GameInfo(uint iD, string name)
    {
        ID = iD;
        Name = name;
    }
}
