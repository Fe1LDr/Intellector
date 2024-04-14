using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using static Networking;
using static Board;

namespace Assets.Scenes.Scripts.Server
{
    public interface INetworkGameManager
    {
        
        public void SendMove(Vector2Int start, Vector2Int end, int transform_info);
        public void SendExit();
        public void SendRematch();
    }

    public class NetworkGameManager: INetworkGameManager
    {
        public void SendMove(Vector2Int start, Vector2Int end, int transform_info)
        {
            Networking.SendMove(
                new byte[5] { (byte)start.x, (byte)start.y, (byte)end.x, (byte)end.y, (byte)transform_info },
                ServerConnection.GetConnection().Client.GetStream());
        }
        public void SendExit()
        {
            const byte exit_code = 111;
            SendCode(exit_code, ServerConnection.GetConnection().Client.GetStream());
            ServerConnection.GetConnection().Close();
        }
        public void SendRematch()
        {
            const byte rematch_code = 222;
            SendCode(rematch_code, ServerConnection.GetConnection().Client.GetStream());
        }
    }
}
