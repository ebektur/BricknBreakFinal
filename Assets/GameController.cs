using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    public static GameController instance;
    public static Dictionary<int, PlayerManager> players = new Dictionary<int, PlayerManager>();

    //check if necessary
    //public GameObject localPlayerPrefab;
    public GameObject OpponentPrefab;
    public GameObject localUserPrefab;

  //  public Text LocalUserName;
 //   public Text OpponentUserName;

    //Game States
    private bool gameOver = false;
    private bool gameStarted = false;
    private bool gameFinished = false;

    //public LevelController levelController;
    public Ball ball;
    public Player player;
    public Timer timer;
    public Bricks bricks;
    public CentralEventManager centralEventManager;
    public LevelController levelController;
    public CanvasButtons canvasButtons;
    public GameObject RoundTransitionCanvas;

    public delegate void OnNewRoundStartDelegate();
    public event OnNewRoundStartDelegate newRoundStart;

    public delegate void PleaseWaitDelegate(string notifText);
    public event PleaseWaitDelegate PleaseWait;

    public delegate void StopMovementDelegate();
    public event StopMovementDelegate StopMovement;

    public delegate void CanvasButtonsDelegate(string scoreText, string totalScoreText, string gameStatusText);
    public event CanvasButtonsDelegate SetCanvasButton;

    public delegate void CanvasButtonsMultiplayerDelegate(Dictionary<int, PlayerManager> players);
    public event CanvasButtonsMultiplayerDelegate SetCanvasButtonMultiplayer;

    public int totalScore;
    public float currentLevel; //is game manager informed about levels with this system
    public float TotalLevel;

    //TODO:set player object according to practise-real game
    public bool isPractise = false;
    public bool allowNextLevel = false;
    GameObject _player;


    private void Awake()
    {
        if(instance == null)
        {
            instance = this;

        }
        else if(instance != this)
        {
            Debug.Log("Instance already exists, destroying the instance");
            Destroy(instance);
        }
         currentLevel = player.GetComponent<PlayerManager>().currentLevel; //is game manager informed about levels with this system
         TotalLevel = player.GetComponent<PlayerManager>().totalLevel;
    }

    public void InitializeNewRoundMessage()
    {
        if(isPractise == false)
        {
            if(allowNextLevel == true){
                newRoundStart();
                player.enableMotion();
                allowNextLevel = false;
                ClientSend.SetMove(false); //SETMOVELARI KONTROL ET
                
            }
        }
        else{
             StartCoroutine(RemoveAfterSeconds(1, RoundTransitionCanvas));
             //starts new round from there
             newRoundStart();
             player.enableMotion();
        }
    }

    public IEnumerator RemoveAfterSeconds(int seconds, GameObject obj)
    {
        obj.SetActive(true);
        yield return new WaitForSeconds(seconds);
        obj.SetActive(false);
    }

    public void InitalizeMultiplayer() //tek tek gerekli değişiklikler için fonksiyonları yaz
    {
        canvasButtons.InitializeMultiplayer(centralEventManager);
       // RoundTransitionCanvas.SetActive(false); //çıkmıyor
        levelController.InitializeMultiplayer(centralEventManager);
        player.Initialize(centralEventManager);
        timer.Initialize(centralEventManager); 
        ball.Initialize(centralEventManager); 
        bricks.Initialize(centralEventManager); 
        timer.EndofTime += PlayersTimeEnd;
       // canvasButtons.PressedMoveMulti += checkAllPlayersMove;
        levelController.ARoundCompleted += SetNewRoundCondition; //burda kaldım 16 ağustos
        InitGameStates();
        centralEventManager.SendStartMultiplayerGameMsg();
    }
    
    public void Initialize()
    {
       // setPlayer();
        //currentLevel = 1f; //sil
        RoundTransitionCanvas.SetActive(false); //çıkmıyor
        levelController.Initialize(centralEventManager);
        player.Initialize(centralEventManager);
        timer.Initialize(centralEventManager);
        ball.Initialize(centralEventManager);
        bricks.Initialize(centralEventManager);
        //levelController.RoundsCompleted += AllRoundsCompleted;
        levelController.ARoundCompleted += SetNewRoundCondition;
        canvasButtons.Initialize(centralEventManager);
        InitGameStates();
        centralEventManager.SendStartGameMessage();
    }

    public void InitGameStates() 
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
    }

    public void PlayersTimeEnd()
    {
        Debug.Log("Playerstimeend gamecontroller.cs");
        levelController.LevelFinishedDueToDurationMultiplayer(players);
    }

    public void SetNewRoundCondition() //in this part, round transition canvas should be displayed
    {
        if(isPractise == true)
        {
            currentLevel ++;
            player.GetComponent<PlayerManager>().currentLevel = currentLevel;

            if(currentLevel == TotalLevel) //game over stuff
            {
            gameStarted = false;
            gameOver = true;
            Debug.Log("Game over");
            //Display Game Finished Canvas
            }

            else //next level 
            {
                Debug.Log("Gamcont setcanvas button");
                SetCanvasButton(levelController.scoreText, levelController.totalScoreText, levelController.gameStatusText);
                StopMovement();
            }
        }

        else{ //Online Game, send current level to server, wait until everyone is on same round together
            
            if(currentLevel == TotalLevel) //game over stuff
            {
            gameStarted = false;
            gameOver = true;
            Debug.Log("Game over");
            //Display Game Finished Canvas
            }

            else //next level 
            {
            Debug.Log("Gamcont setcanvas button");
            SetCanvasButtonMultiplayer(players);
            StopMovement();
            }
        }
    }


    public int PlayerPositioninDict(int _id) //currentScore
    {
        //Debug.Log("gamecontroller.cs currentscore with players count" + players.Count.ToString());
        for(int i=1; i<=players.Count; i++)
        {
            //Debug.Log(players[1].username);
            if(players[i].id == _id) 
            {
                Debug.Log(players[i].username + " is found in gamecontroller.cs");
                return i;
            }
        }

        return -1;
    }

    public void checkAllPlayersMove() //delete this carefully
    {
        int count = 0;
        for(int i=1; i<=players.Count; i++)
        {
            if(players[i].isMovePressed == true) 
            {
                count++;
            }
        }

        if(count == players.Count){ //asll players pressed move, set the new round
           PleaseWait("Get Ready for the Next Round!");
           //canvasButtons.GetComponent<CanvasButtons>().CanvasButtonsOptions.SetActive(false);
           //InitializeNewRoundMessage();
        }

        else
        {
            PleaseWait("Please Wait for the other Player..");
            checkAllPlayersMove(); 
            //IF OTHER PLAYER NEVER PRESSES MOVE, WHAT WILL HAPPEN 31.08
        }
    }

    public void currentScore(int pos, int _currentScore)
    {
        players[pos].currentScore = _currentScore;
    }

    public void currentTime(int pos, int _timeLeft){
  
        players[pos].playerDurationLeftForRound = _timeLeft;
    }

    public void moveResponse(int pos, bool _moveResponse)
    {
        players[pos].isMovePressed = _moveResponse;
       // Debug.Log("Game controller.cs move response is " + _moveResponse.ToString());
        if(players[pos].isMovePressed == true){
            StartCoroutine(checkAllPlayers());
            }
    }

    public IEnumerator checkAllPlayers() //all players move condition
    {
        int count = 0;
        for(int i=1; i<=players.Count; i++)
        {
            if(players[i].isMovePressed == true) 
            {
                count++;
            }
        }

         if(count == players.Count){
            PleaseWait("Get Ready for the Next Round!");
            allowNextLevel = true;
            //canvasButtons.PressMove();
         }
         else{ //allownextlevel = false
            PleaseWait("Please Wait for the other Player..");
            //checkAllPlayers();
         }
         canvasButtons.PressMove();
        
        yield return null;

    }

  public void SpawnPlayer(int _id, string _username, int _currScore, int _totalScore, bool _isMovePressed) //both will be unaware of each other.
    {
        if (_id == ClientOps.instance.myId) //if it is me, set me
        {
            _player = localUserPrefab;
           // LocalUserName.GetComponent<Text>().text = _username;
           // Debug.Log("Gc.cs spawn player if condition" + _username);
        }
        else
        {
            //Instantiate(OpponentPrefab);
            OpponentPrefab.SetActive(true);
            _player = OpponentPrefab;
           // OpponentUserName.GetComponent<Text>().text = _username;
           // Debug.Log("Gc.cs spawn player else condition" + _username);
        }

        _player.GetComponent<PlayerManager>().id = _id; 
        _player.GetComponent<PlayerManager>().username = _username;
        _player.GetComponent<PlayerManager>().currentScore = _currScore;
        _player.GetComponent<PlayerManager>().totalScore = _totalScore;
        _player.GetComponent<PlayerManager>().isMovePressed = _isMovePressed;

        players.Add(_id, _player.GetComponent<PlayerManager>());

        Debug.Log("gamecontroller spawnplayer is called with username: " + _username );
    }
}

