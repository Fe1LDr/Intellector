using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ServerManager 
{
    public ServerConnection Connection { get => ServerConnection.GetConnection(); }

    private IGamesReader gamesReader;
    private IGameJoiner gameJoiner;
    private IGameCreator gameCreator;

    private static ServerManager instance;

    private ServerManager()
    {
        gameCreator = Settings.ServerFactory.MakeGameCreator();
        gameJoiner = Settings.ServerFactory.MakeGameJoiner();
        gamesReader = Settings.ServerFactory.MakeGamesReader();
    }
    public static ServerManager GetInstance()
    {
        return instance ?? new ServerManager();       
    }

    public void CreateGame(GameInfo gameInfo, Action onConnect)
    {
        gameCreator.CreateGame(gameInfo, onConnect);
    }
    public void CancelGameCreate()
    {
        gameCreator.CancelGameCreate();
    }
    public (bool, GameInfo) JoinGame(uint game_id)
    {
        return gameJoiner.JoinGame(game_id); 
    }
    public List<GameInfo> ReadGames()
    {
        return gamesReader.ReadGames();
    }

}
