using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    bool hitting;
    private int count;
    public Transform aimTarget;
    public float speed;
    public float force;
    float h;
    float v;
    public Rigidbody rb; //sorun var
    public CentralEventManager centralEventManager;
    public GameController gameController;
    public Vector3 StartPos;


    public void Initialize(CentralEventManager centralEventManager)
    {
        if(gameController.GetComponent<GameController>().isPractise == true){centralEventManager.onGameStart += OnGameStart;} //left is subscriber
        else{centralEventManager.onGameStartMultiplayer += OnGameStart; }
        gameController.newRoundStart += SetNewRound;
        gameController.StopMovement += disableMotion;
    }

    public void OnGameStart() //central event managerda ayırmak gerekebilir multi-user
    {
        speed = 5f;
        force = 14;
        rb = GetComponent<Rigidbody>(); 
        //StartPos = transform.position; //olmadı
        StartPos = new Vector3(-0.8f, 1.12f, -4.27f);
        Move();
    }

    void Update() //Update()
    {
        Move();
    }

    public void Move()
    {
        h = Input.GetAxisRaw("Horizontal");
        v = Input.GetAxisRaw("Vertical");
        if(h!=0 || v!= 0)
        {
            Vector3 move = gameObject.transform.position + new Vector3(h, 0, v) * speed * Time.deltaTime;
            rb.MovePosition(move);

            //ClientSend.PlayerMovement(move);
        }
    }

    public void disableMotion()
    {
        Debug.Log("Player rigidbody is disabled");
        rb.constraints = RigidbodyConstraints.FreezePositionX | RigidbodyConstraints.FreezePositionY | RigidbodyConstraints.FreezePositionZ;
    }

    public void enableMotion()
    {
        Debug.Log("Player rigidbody is enabled");
        rb.constraints = RigidbodyConstraints.None;
    }

    public void SetNewRound() //set everything to zero
    {
        enableMotion();
        Debug.Log("Entered SETNEWROUND PLAYER");
        transform.position = StartPos;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Ball"))
        {
            Vector3 direction = aimTarget.position - transform.position;
            other.GetComponent<Rigidbody>().velocity =  direction.normalized * force + new Vector3(0,6,0);
        }

        if(other.CompareTag("Wall") || other.CompareTag("FrontWall"))
        {
            //GetComponent<Collider>().isTrigger = false;
           // GetComponent<Rigidbody>().position = transform.position - new Vector3(0,0,5);
            
        }

    }

    public void OnDestroy()
    {
        if(gameController.GetComponent<GameController>().isPractise == true){centralEventManager.onGameStart -= OnGameStart;} //left is subscriber
        else{centralEventManager.onGameStartMultiplayer -= OnGameStart; }
        gameController.newRoundStart -= SetNewRound;
        gameController.StopMovement -= disableMotion;

    }

}
