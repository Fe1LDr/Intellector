using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JoinInfo
{
        public uint WaitingGamesCodeId { get; set; }
        public WaitingGameInfo WaitingGame { get; set; }
        public int JoinCode { get; set; }
        public int Port { get; set; }
        public bool Team { get; set; }
}
