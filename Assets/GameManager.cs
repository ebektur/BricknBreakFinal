using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;


public class GameManager : MonoBehaviour
{
    public static GameManager instance = null; //singleton
    public Ball score;
    public Bricks bricks;
    private bool isSuccess;


    //pause the game when it is finished
    public GameObject RoundTransitionCanvas;
    public GameObject GameEndedCanvas;
    public GameObject PlayerObject;
    public GameObject BricksObject;
    public GameObject BallObject;
    public GameObject FrontWallObject;
    public GameObject WallObject;
    public GameObject TimerObject;
    public GameObject SliderObject;
    public GameObject DemoObject;
    public GameObject BlockObject;



    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI TotalscoreText;
    public TextMeshProUGUI gameStatusText;
    public int currentScore;
    public int totalScore;
    public float currentLevel = 0f;
    public int unlockedLevel;
    public float TotalLevel = 3f;
    public float totalDuration = 0f;

    //Slider
    public float sliderPercentage;
    private Slider sliderObj;
    private Slider sli;

    //Game State
    private bool gameOver = false;
    private bool gameStarted = false;
    private bool gameFinished = false;

    // void Awake()
    // {
    //     if (instance == null)
    //     { //check if an instance of Game Manager is created
    //         instance = this;    //if not create one
    //         Debug.Log("Game Manager is created");
    //     }
    //     else if (instance != this)
    //     {
    //         Destroy(gameObject);    //if already exists destroy the new one trying to be created
    //     }

    //     DontDestroyOnLoad(gameObject);  //Unity function allows a game object to persist between scenes
    // }

    public bool GameStarted
    {
        get { return gameStarted; }
    }

    public bool GameOver
    {
        get { return gameOver; }
    }

    public void StartGame()
    {

        if (!gameStarted)
        {
            gameStarted = true;
        }
        else
        {
            gameStarted = false;
            gameOver = true;
        }

        Debug.Log("StartGame");
    }


    //must create start game to retry game
    //oyun bittikten sonra hareket ediyor

    void Start(){
        isSuccess = false;
        sliderPercentage = currentLevel/TotalLevel * 100.0f;
        SliderObject.GetComponent<ProgressBar>().UpdateProgress();
        RoundTransitionCanvas.SetActive(false);
    }



    void StopGame(){
        Debug.Log("Game enters stop game");
        PlayerObject.GetComponent<Player>().enabled = false;
        BricksObject.GetComponent<Bricks>().enabled = false;
        BallObject.GetComponent<Ball>().enabled = false;
        FrontWallObject.GetComponent<FrontWall>().enabled = false;
        WallObject.GetComponent<Wall>().enabled = false;
        TimerObject.GetComponent<Canvas>().enabled = false; //oluyor
    }

    // Start is called before the first frame update
    public void LevelFinishedDueToSuccess()
    {
        //all items are hit by the user
        gameStatusText.text = "Congratulations!";
        GameObject BallObject = GameObject.Find("Ball");
        totalScore = totalScore + BallObject.GetComponent<Ball>().count;
        scoreText.text = "Round Score: " + BallObject.GetComponent<Ball>().count.ToString();
        TotalscoreText.text = "Total Score: " + totalScore.ToString();
        isSuccess = true;
        DemoObject.GetComponent<Demo>().ChangeDuration(0); //buna kızıyo
        OnRoundComplete();
        //move to next level with move button
    }

    public void LevelFinishedDueToDuration()
    {
        //time is up
        if(isSuccess == false)
        {
            Debug.Log("GameManager.cs returns time finished");
            GameObject BallObject = GameObject.Find("Ball");
            totalScore = totalScore + BallObject.GetComponent<Ball>().count;
            scoreText.text = "Round Score: " + BallObject.GetComponent<Ball>().count.ToString();
            TotalscoreText.text = "Total Score: " + totalScore.ToString();
            gameStatusText.text = "Time Out";
            OnRoundComplete();
        }
        //try again 
    }


    void OnRoundComplete(){
        //Call the slider to fill up!
        StopGame();
        currentLevel++;
        sliderPercentage = currentLevel/TotalLevel * 100.0f; //evaluate this
        //sliderObj.value = sliderPercentage; //use of this?
        Debug.Log("Round Complete");
        GameEndedCanvas.SetActive(true);
        SliderObject.GetComponent<ProgressBar>().UpdateProgress(); 
        //timer should be zeroed
    }
    public IEnumerator RemoveAfterSeconds(int seconds, GameObject obj)
    {
        obj.SetActive(true);
        yield return new WaitForSeconds(seconds);
        obj.SetActive(false);
    }
    public void initiateNewRound()
    {
        StartCoroutine(RemoveAfterSeconds(1, RoundTransitionCanvas));

        //set objects to new round
        PlayerObject.GetComponent<Player>().enabled = true;
        BricksObject.GetComponent<Bricks>().enabled = true;
        BallObject.GetComponent<Ball>().enabled = true;
        FrontWallObject.GetComponent<FrontWall>().enabled = true;
        WallObject.GetComponent<Wall>().enabled = true;
        TimerObject.GetComponent<Canvas>().enabled = true; //oluyor

        // //canvas kapanmalı
        // GameEndedCanvas.SetActive(false);
        // FrontWallObject.GetComponent<FrontWall>().Start();
        // PlayerObject.GetComponent<PlayerController>().Start();
        // BallObject.GetComponent<Ball>().Start();
        // DemoObject.GetComponent<Demo>().Start();
    }
}
