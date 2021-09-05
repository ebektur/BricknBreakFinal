using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    public int id;
    public string username;
    public int currentScore;
    public int totalScore;

    //buralar yok
    public float currentLevel = 0f; 
    public float totalLevel = 3f;
    public int playerDurationLeftForRound;
    public bool isMovePressed; //don't allow to move forward if they dont press move

   public PlayerManager(int _id, string _username, int _currentScore, int _totalScore, bool _isMovePressed)
    {
        id = _id;
        username = _username;
        currentScore = _currentScore;
        totalScore = _totalScore;
        isMovePressed = _isMovePressed;
    }

    //Msg1: Player_ finished all the bricks -> End game for all users
    //Msg2: Time is up-> Compare scores and msg who is winning
    //currentlevel
    //totalllevel
    //isSuccess

}
