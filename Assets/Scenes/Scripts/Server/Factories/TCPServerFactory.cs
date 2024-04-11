using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TCPServerFactory : IServerFactory
{
    public IGameCreator MakeGameCreator()
    {
        return new GameCreator();
    }

    public IGameJoiner MakeGameJoiner()
    {
        return new GameJoiner();
    }

    public IGamesReader MakeGamesReader()
    {
        return new GamesReader();
    }
}
