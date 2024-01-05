using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Text;
using UnityEngine;

public class Networking 
{
    public const string password = "a3P1>8]Û-/é×ÿÝ975?:$qcDûÔ9&e@1a<c{a/";
    static public void SendMessage(byte mes, NetworkStream stream)
    {
        byte[] send_bytes = new byte[1] { mes };
        stream.Write(send_bytes, 0, 1);
    }

    static public void SendString(string str, NetworkStream stream)
    {
        byte[] str_bytes = Encoding.Default.GetBytes(str);
        stream.Write(str_bytes, 0, str_bytes.Length);
    }

    static public GameInfo RecvGameInfo(NetworkStream stream)
    {
        byte[] id_bytes = new byte[4];
        byte[] name_bytes = new byte[20];
        stream.Read(id_bytes, 0, 4);
        stream.Read(name_bytes, 0, 20);
        uint id = BitConverter.ToUInt32(id_bytes);
        string name = Encoding.Default.GetString(name_bytes).TrimEnd('\0');
        return new GameInfo(id, name);
    }
    static public byte RecvMessage(NetworkStream stream)
    {
        byte[] recv_bytes = new byte[1];
        stream.Read(recv_bytes, 0, 1);
        return recv_bytes[0];
    }

    static public void SendMove(byte[] move_bytes, NetworkStream stream)
    {
        stream.Write(move_bytes, 0, 5);
    }

    static public byte[] RecvMove(NetworkStream stream)
    {
        byte[] move_bytes = new byte[5];
        stream.Read(move_bytes, 0, 5);
        return move_bytes;
    }

    static public void SendExit(NetworkStream stream)
    {
        byte[] exit = new byte[5] { 111, 0, 0, 0, 0 };
        stream.Write(exit, 0, 5);
    }

}
