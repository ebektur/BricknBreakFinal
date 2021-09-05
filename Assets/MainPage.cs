using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainPage : MonoBehaviour
{
    //public Client client;
    public static MainPage instance;
    public GameController gameController;
    public GameObject featuresBar;
    public InputField usernameField; //put this later to profile page

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
        else if(instance != this)
        {
            Debug.Log("Instance already exists, destroying object");
            gameController.isPractise = false;
            Destroy(this);
        }
    }


    public void activateMainPage()
    {
        gameObject.SetActive(true);
    }

    public void PressedPractiseGame() //dont forget to change onclick func
    {
        GameController.instance.isPractise = true;
        gameController.Initialize();
        gameObject.SetActive(false);
        featuresBar.SetActive(true);
    }

    public void PressedPlayGame() //Connect to Server
    {
        //UI manager
        gameObject.SetActive(false);
        ClientOps.instance.ConnectToServer();
        usernameField.interactable = false; //put it later to profile
        gameController.InitalizeMultiplayer();

    }


}
