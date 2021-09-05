using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CentralEventManager : MonoBehaviour //central event managerın çocuğu -> game controller + login
{
    public FirebaseManager firebaseManager;
    public MainPage mainPage;
    public delegate void OnGameStartDelegate();
    public event OnGameStartDelegate onGameStart;

    public delegate void OnGameStartMultiplayerDelegate();
    public event OnGameStartMultiplayerDelegate onGameStartMultiplayer;

    public void Activate() //after login, activate game options
    {
        firebaseManager.LoginSuccessful += SetPersonalizedGame;
    }

    public void SetPersonalizedGame()
    {
        //Display main menu
        mainPage.activateMainPage();
    }

    public void SendStartGameMessage()
    {
        if(onGameStart != null)
        {
            onGameStart();
        }
    }

    public void SendStartMultiplayerGameMsg()
    {
        if(onGameStartMultiplayer != null)
        {
            onGameStartMultiplayer();
        }
    }
}
