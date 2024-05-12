using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scenes.Scripts.Server
{
    public interface IServerListenerObservable
    {
        public void RegisterObserver(IServerListenerMoveObserver observer);
        public void RegisterObserver(IServerListenerTimeObserver observer);
        public void RegisterObserver(IServerListenerExitObserver observer);
        public void RegisterObserver(IServerListenerRematchObserver observer);
        public void RegisterObserver(IServerListenerTimeOutObserver observer);
    }
    public interface IServerListener
    {
        public void ListenServer();
        public void MoveReceived(Vector2Int start, Vector2Int end, int transform_info);
        public void TimeReceived(int time);
        public void ExitReceived();
        public void RematchReceived();
        public void TimeOutReceived(bool exit_team);
    }
}
