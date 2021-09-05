using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class LevelController : MonoBehaviour //rounds, scores
{
    public TextMeshProUGUI countText;
    public int currentScore = 0;
    public int totalScore = 0;
    //public float currentLevel = 0f; //is game manager informed about levels with this system
    //public float TotalLevel = 3f;
    public CentralEventManager centralEventManager;
    public GameController gameController;
    public Player playerManager;
    private bool isSuccess;
    //public GameObject GameOptionsCanvas;

    public string scoreText;
    public string totalScoreText;
    public string gameStatusText;

    //public Brick brick;
    public Timer timer;
    public Ball ball;
    public Bricks bricks;

    public GameObject BallObject;

    public delegate void RoundsCompletedDelegate();
    public event RoundsCompletedDelegate RoundsCompleted;

    //public delegate void CanvasButtonsDelegate(string scoreText, string totalScoreText, string gameStatusText);
    //public event CanvasButtonsDelegate SetCanvasButton;
    
    public delegate void ARoundCompletedDelegate();
    public event ARoundCompletedDelegate ARoundCompleted;

    public delegate void ObjectsSetNewRoundDelegate();
    public event ObjectsSetNewRoundDelegate ObjectsSetNewRound;

    public void Initialize(CentralEventManager centralEventManager) //initialize multiplayer
    {
        centralEventManager.onGameStart += OnGameStart;
        timer.EndofTime += LevelFinishedDueToDuration;
        ball.SetCount += SetCountText;
        ball.AllBricksHit += LevelFinishedDueToSuccess;
        gameController.newRoundStart += initiateNewRound;
    }

    public void InitializeMultiplayer(CentralEventManager centralEventManager)
    {
        centralEventManager.onGameStartMultiplayer += OnGameStart;
        //timer.EndofTime += InitializeRoundCompleted;
        ball.SetCount += SetCountText;
        ball.AllBricksHit += LevelFinishedDueToSuccess;
        gameController.newRoundStart += initiateNewRound;
    }

    public void SendInputToServer()
    {
        currentScore = BallObject.GetComponent<Ball>().count; 
        ClientSend.SetScore(currentScore); //setscore = playermovement
    }

    public void OnGameStart()
    {
        isSuccess = false;
        currentScore = 0;
        ball.enableMotion();
    }

    public void OnRoundComplete(){
        ARoundCompleted();
    }

    public void LevelFinishedDueToDurationMultiplayer(Dictionary<int, PlayerManager> players)
    {       //online game
            Debug.Log("Lc.cs levelfinishedduetodurationmultiplayer");
            int countUser = 0;
            for(int i=1; i<= players.Count; i++)
            {
                if(players[i].playerDurationLeftForRound == 0)
                {
                    countUser++;
                }
            }

            if(countUser == players.Count)
            {
                Debug.Log("All Users finished the round");
                if(isSuccess == false) //Set new round
                {
                     OnRoundComplete();
                }
            }
            else{
                Debug.Log("Levelfduetodur multiplayer waited for one sec"); //BURAYA GELİYOR
                StartCoroutine(WaitOneSecond());
            }
    }

    void LevelFinishedDueToDuration()
    {
        Debug.Log("LevelController.cs returns time finished");
        if(gameController.GetComponent<GameController>().isPractise == true)
        {
            if(isSuccess == false)
            {
            //GameObject BallObject = GameObject.Find("Ball");
            totalScore = totalScore + BallObject.GetComponent<Ball>().count;
            scoreText = "Round Score: " + BallObject.GetComponent<Ball>().count.ToString();
            //currentScore = BallObject.GetComponent<Ball>().count;
            totalScoreText= "Total Score: " + totalScore.ToString();
            gameStatusText = "Time Out";
            //ReceiveOpponentLevelStatus();
            OnRoundComplete();
            }
        }
    }
    

    IEnumerator WaitOneSecond()
    {
         yield return new WaitForSeconds(1);
         gameController.PlayersTimeEnd();
         Debug.Log("lc.cs wait one second");
    }

    public void LevelFinishedDueToSuccess()
    {
        gameStatusText = "Congratulations!";
        GameObject BallObject = GameObject.Find("Ball");
        currentScore = BallObject.GetComponent<Ball>().count;
        totalScore = totalScore + BallObject.GetComponent<Ball>().count;
        scoreText = "Round Score: " + BallObject.GetComponent<Ball>().count.ToString();
        totalScoreText = "Total Score: " + totalScore.ToString();
        isSuccess = true;

        //SetCanvasButton(scoreText, totalScoreText, gameStatusText);
        OnRoundComplete();
    }

    public void SetCountText(){  //needs parameter, buraya dön
        countText.text = "Count: " +  playerManager.GetComponent<PlayerManager>().currentScore.ToString();//count.ToString();
    }


    public void SetCountMultiplayer()
    {
        Debug.Log("lc.cs setcountmultiplayer");
        SendInputToServer();
    }



    public void initiateNewRound()
    {
       Debug.Log("levelCont initiate new round");
       ObjectsSetNewRound();
       OnGameStart();
    }

    public void OnDestroy()
    {
        if(gameController.isPractise == true){ centralEventManager.onGameStart -= OnGameStart;
        ball.SetCount -= SetCountText;}
        else{
            centralEventManager.onGameStartMultiplayer -= OnGameStart;
        }

       // centralEventManager.onGameStart -= OnGameStart;
        timer.EndofTime -= LevelFinishedDueToDuration;
        ball.AllBricksHit -= LevelFinishedDueToSuccess;
        gameController.newRoundStart -= initiateNewRound;

    }
}
