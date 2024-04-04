using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaitingGameInfo 
{
    public uint WaitingGameId { get; set; }
    public string Name { get; set; }
    public bool? ColorChoice { get; set; }
    public uint? MaxTime { get; set; }
    public uint? AddedTime { get; set; }
    public DateTime RefreshTime { get; set; }

    public GameInfo ToGameInfo()
    {
        ColorChoice color;
        switch (this.ColorChoice)
        {
            case true: color = global::ColorChoice.black; break;
            case false: color = global::ColorChoice.white; break;
            case null: color = global::ColorChoice.random; break;
        }
        return new GameInfo()
        {
            ID = WaitingGameId,
            Name = Name,
            Color = color,
            TimeContol = new((int)MaxTime, (int)AddedTime)
        };
    }
}
