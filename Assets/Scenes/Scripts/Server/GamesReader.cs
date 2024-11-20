using System;
using System.Collections.Generic;
using System.Net.Sockets;
using static Networking;

public interface IGamesReader
{
    public List<GameInfo> ReadGames();
}

public class GamesReader : IGamesReader
{
    public List<GameInfo> ReadGames()
    {
        const byte games_list_request = 100;

        TcpClient server = ServerConnection.GetConnection().Client;
        NetworkStream stream = server.GetStream();

        SendCode(games_list_request, stream);
        int GameCount = RecvInt(stream);

        List<GameInfo> games = new List<GameInfo>();
        for (int i = 0; i < GameCount; i++)
        {
            games.Add(RecvGameInfo(stream));
        }
        return games;
    }
}

public class VersionException : Exception
{
    public VersionException(string message) : base(message) { }
}
