using System.Collections;
using System.Collections.Generic;
using System.Net.Sockets;
using UnityEngine;
using static Networking;

public interface IGameJoiner
{
    public (bool, GameInfo) JoinGame(uint game_id);
}
public class GameJoiner : IGameJoiner
{
    public (bool, GameInfo) JoinGame(uint game_id)
    {
        const byte join_game_code = 30;
        const byte no_such_game_ans = 99;

        TcpClient server = ServerConnection.GetConnection().Client;
        NetworkStream stream = server.GetStream();

        SendCode(join_game_code, stream);
        SendCode((byte)game_id, stream);

        GameInfo game_info = RecvGameInfo(stream);

        byte ans = RecvCode(stream);
        if (ans == no_such_game_ans) return (false, null);

        bool team = ans != 0;
        game_info.Team = team;

        return (true, game_info);
    }
}
