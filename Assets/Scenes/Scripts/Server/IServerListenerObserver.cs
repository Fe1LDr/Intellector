using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scenes.Scripts.Server
{
    public interface IServerListenerObserver
    {
        public void OnMoveReceived(Vector2Int start, Vector2Int end, int transform_info);
        public void OnTimeReceived(int time);
        public void OnExitReceived();
        public void OnRematchReceived();
        public void OnTimeOutReceived(bool exit_team);
    }
}
