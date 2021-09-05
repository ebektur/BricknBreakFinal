using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClientSend : MonoBehaviour
{
    private static void SendTCPData(Packet _packet)
    {
        _packet.WriteLength();
        ClientOps.instance.tcp.SendData(_packet);
    }

    private static void SendUDPData(Packet _packet)
    {
        _packet.WriteLength();
        ClientOps.instance.udp.SendData(_packet);
    }

    #region Packets
    public static void WelcomeReceived()
    {
        using (Packet _packet = new Packet((int)ClientPackets.welcomeReceived))
        {
            _packet.Write(ClientOps.instance.myId);
            _packet.Write(MainPage.instance.usernameField.text);

            SendTCPData(_packet);
        }
    }

    public static void SetScore(int _currentScore) //playermovement
    {
        using (Packet _packet = new Packet((int)ClientPackets.setScore))
         {
             _packet.Write(_currentScore);
             SendTCPData(_packet);
         }
    }

    public static void SetTime(int remainingDuration){
        using(Packet _packet = new Packet((int)ClientPackets.setTime))
        {
            _packet.Write(remainingDuration);
            SendTCPData(_packet);
        }
    }

    public static void SetMove(int _toClient, bool _moveResponse)
    {
        using(Packet _packet = new Packet((int)ClientPackets.moveResponse))
        {
            _packet.Write(_toClient);
            _packet.Write(_moveResponse);
            SendTCPData(_packet);
        }
    }

    #endregion
}