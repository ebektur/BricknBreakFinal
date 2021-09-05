using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Net;

public class ClientHandler : MonoBehaviour
{
     public static void Welcome(Packet _packet)
    {
        string _msg = _packet.ReadString();
        int _myId = _packet.ReadInt();

        Debug.Log($"Message from server: {_msg}");
        ClientOps.instance.myId = _myId;
        ClientSend.WelcomeReceived();
    }


    public static void SpawnPlayer(Packet _packet)
    {
        int _id = _packet.ReadInt();
        string _username = _packet.ReadString();
        int _currentScore = _packet.ReadInt();
        int _totalScore = _packet.ReadInt();
        bool _isMovePressed = _packet.ReadBool();

       // Debug.Log(_username + " is arrived");
        GameController.instance.SpawnPlayer(_id, _username, _currentScore, _totalScore, _isMovePressed); 
        Debug.Log("clienthandler spawnplayer is called with score: " + _currentScore );
    }

    public static void currentScore(Packet _packet) //burada
    {
        int _id = _packet.ReadInt();
        int _currentScore = _packet.ReadInt();
        int pos = GameController.instance.PlayerPositioninDict(_id);
        GameController.instance.currentScore(pos, _currentScore);

    }

    public static void currentTime(Packet _packet)
    {
        int _id = _packet.ReadInt();
        int _timeLeft = _packet.ReadInt();
        int pos = GameController.instance.PlayerPositioninDict(_id);
       //GameController.instance.players[_id].playerDurationLeftForRound = _timeLeft; //test this 31.08
        GameController.instance.currentTime(pos, _timeLeft);
    }

    public static void moveResponse(Packet _packet) //start from this place and check why move response is not affected by  
    {
        int _id = _packet.ReadInt();
        bool _moveResponse = _packet.ReadBool();
        int pos = GameController.instance.PlayerPositioninDict(_id);
        GameController.instance.moveResponse(pos, _moveResponse);

    }

}
