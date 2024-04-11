using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IServerFactory
{
    public IGameCreator MakeGameCreator();
    public IGameJoiner MakeGameJoiner();
    public IGamesReader MakeGamesReader();
}