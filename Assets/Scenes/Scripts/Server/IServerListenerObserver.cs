using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scenes.Scripts.Server
{
    public interface IServerListenerMoveObserver
    {
        public void OnMoveReceived(Vector2Int start, Vector2Int end, int transform_info);
    }

    public interface IServerListenerTimeObserver
    {
        public void OnTimeReceived(int time);
    }

    public interface IServerListenerExitObserver
    {
        public void OnExitReceived();
    }

    public interface IServerListenerRematchObserver
    {
        public void OnRematchReceived();
    }

    public interface IServerListenerTimeOutObserver
    {
        public void OnTimeOutReceived(bool team);
    }
}
