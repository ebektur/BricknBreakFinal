using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Ball : MonoBehaviour
{
    // Start is called before the first frame update

    Vector3 initialPosition;
    public TextMeshProUGUI countText;
    public Rigidbody rb;
    public GameObject ball;
    public Bricks brick;
    public int count = 0;
    public float total;
    public bool isAllSetFalse;
    public CentralEventManager centralEventManager;
    public GameController gameController;
    public LevelController levelController;
    public Player playerManager;

    public delegate void CountDelegate();
    public event CountDelegate SetCount;

    public delegate void AllBricksHitDelegate(); 
    public event AllBricksHitDelegate AllBricksHit;

    public delegate void ABrickHitDelegate(GameObject theBrick); 
    public event ABrickHitDelegate ABrickHit;

    public void Initialize(CentralEventManager centralEventManager)
    {
        rb = GetComponent<Rigidbody>();
        if(gameController.GetComponent<GameController>().isPractise == true){centralEventManager.onGameStart += OnGameStart;} //left is subscriber
        else{centralEventManager.onGameStartMultiplayer += OnGameStart; }

        //centralEventManager.onGameStart += OnGameStart;
        gameController.StopMovement += disableMotion;
        gameController.newRoundStart += SetNewRound;

        //levelController.ARoundCompleted += SetNewRound;
    }


    public void disableMotion()
    {
        Debug.Log("Ball is freezed");
        rb.constraints = RigidbodyConstraints.FreezePositionX | RigidbodyConstraints.FreezePositionY | RigidbodyConstraints.FreezePositionZ;
    }

    public void enableMotion()
    {
        rb.constraints = RigidbodyConstraints.None;

    }

    public void SetNewRound()
    {
        enableMotion();
        OnGameStart();
        Debug.Log("Ball is at the starting pos");
    }
    
    public void OnGameStart()
    {
        if(gameController.GetComponent<GameController>().isPractise == true)
        {
            count = 0;  //client can't send server current score. write func to send count async
            total = 999;
        }

        else{
            count = levelController.GetComponent<LevelController>().currentScore;
            total = levelController.GetComponent<LevelController>().totalScore;
        }

        initialPosition = new Vector3(-2,1,-3.5f);
        transform.position = initialPosition;
        countText.text = "Count: 0";
        isAllSetFalse = false;
    }

    public void OnDestroy()
    {
       // centralEventManager.onGameStart -= OnGameStart;
        gameController.StopMovement -= disableMotion;
        if(gameController.GetComponent<GameController>().isPractise == true){centralEventManager.onGameStart -= OnGameStart;} //left is subscriber
        else{centralEventManager.onGameStartMultiplayer -= OnGameStart; }
    }

    void Update()
    {
        total = brick.BrickQuantity;   
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("Brick"))
        {
            Debug.Log("Ball can identift brick");
            ABrickHit(other.gameObject);
            //other.gameObject.SetActive(false);
            count = count + 1;

            if(gameController.GetComponent<GameController>().isPractise == false)
            {
                levelController.GetComponent<LevelController>().SetCountMultiplayer();
                SetCount();
            }
            else{
                playerManager.GetComponent<PlayerManager>().currentScore = count;
                SetCount(); //sent this with parameter
            }

           // SetCountText();
        }

        if((count == total) && count != 0)
        {
            isAllSetFalse = true; //All bricks are shot
            AllBricksHit();
            //GameManager.LevelFinishedDueToSuccess();
            //hata çıkarıyor

        }
    }
    private void onCollisionEnter(Collision collision) //girmiyor
    {
        if(collision.transform.CompareTag("Wall"))
        {
            GetComponent<Rigidbody>().velocity = Vector3.zero;
            transform.position = initialPosition;
        }
    }
}
