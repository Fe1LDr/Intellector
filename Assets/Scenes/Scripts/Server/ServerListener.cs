using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using static Networking;


namespace Assets.Scenes.Scripts.Server
{
    public class ServerListener : IServerListener
    {  
        private List<IServerListenerObserver> observers = new List<IServerListenerObserver>();
        public void RegisterObserver(IServerListenerObserver observer)
        {
            observers.Add(observer);
        }
        public void UnregisterObserver(IServerListenerObserver observer)
        {
            observers.Remove(observer);
        }

        public void ListenServer()
        {
            const byte move_code = 10;
            const byte time_code = 20;
            const byte white_time_out_code = 30;
            const byte black_time_out_code = 31;
            const byte exit_code = 111;
            const byte rematch_code = 222;

            var server_stream = ServerConnection.GetConnection().Client.GetStream();
            while (true)
            {
                byte code = RecvCode(server_stream);
                switch (code)
                {
                    case move_code:
                        byte[] move = RecvMove(server_stream);
                        Vector2Int start = new Vector2Int(move[0], move[1]);
                        Vector2Int end = new Vector2Int(move[2], move[3]);
                        int transform_info = move[4];
                        MoveReceived(start, end, transform_info);
                        break;
                    case time_code:
                        int time = RecvInt(server_stream);
                        TimeReceived(time);
                        break;
                    case exit_code:
                        ExitReceived();
                        break;
                    case white_time_out_code:
                        TimeOutReceived(true);
                        break;
                    case black_time_out_code:
                        TimeOutReceived(false);
                        break;
                    case rematch_code:
                        RematchReceived();
                        break;
                }
            }
        }
        public void ExitReceived()
        {
            foreach(var observer in observers) observer.OnExitReceived();
        }
        public void MoveReceived(Vector2Int start, Vector2Int end, int transform_info)
        {
            foreach (var observer in observers) observer.OnMoveReceived(start, end, transform_info);
        }
        public void RematchReceived()
        {
            foreach (var observer in observers) observer.OnRematchReceived();
        }
        public void TimeOutReceived(bool exit_team)
        {
            foreach (var observer in observers) observer.OnTimeOutReceived(exit_team);
        }
        public void TimeReceived(int time)
        {
            foreach (var observer in observers) observer.OnTimeReceived(time);
        }
    }
}
