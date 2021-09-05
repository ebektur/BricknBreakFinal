using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bricks : MonoBehaviour
{
    public CentralEventManager centralEventManager;
    public LevelController levelController;
    public GameController gameController;
    public Ball ball;

    public delegate void changedele(); //CHANGE
    public event changedele change;
    public float roundQuantity;
    public float BrickQuantity;
    public int width = 13;
    public int height = 10;
    public GameObject BricksPrefab;

    
    public ParticleSystem deathParticles;

    public void Initialize(CentralEventManager centralEventManager)
    {
        if(gameController.GetComponent<GameController>().isPractise == true){centralEventManager.onGameStart += OnGameStart;} //left is subscriber
        else{centralEventManager.onGameStartMultiplayer += OnGameStart; }
        
       // centralEventManager.onGameStart += OnGameStart; //left is subscriber
        ball.ABrickHit += DestroyBrick;
        levelController.ObjectsSetNewRound += SetNewRound;
    }

    public void SetNewRound()
    {
        Debug.Log("New Brics are Generated");
        OnGameStart();
    }
    
    public void OnGameStart() //place some bricks on the wall
    {
        roundQuantity = gameController.currentLevel;
        BrickQuantity = roundQuantity * 2 + 2;
        generateBricks();
    }

    public void generateBricks()
   {
       for(int i = 0; i < BrickQuantity; i++)
       {
            float x = Random.Range(5f, -5f);
            var position = new Vector3(x, Random.Range(1, 2), 5f);
            var newBrick = Instantiate(BricksPrefab, position, Quaternion.identity);
            newBrick.tag = "Brick";
            //newBrick.transform.SetParent(gameObject.transform);
       }
       //for each round, create different setup
   }

   public void DestroyBrick(GameObject theBrick)
   {
       Debug.Log("Buraya giriyo bricks.cs destroybrick");
       Instantiate(deathParticles, theBrick.transform.position, Quaternion.identity);
       //theBrick.SetActive(false); 
       Destroy(theBrick);
   }

   private void OnDestroy()
   {
        if(gameController.GetComponent<GameController>().isPractise == true){centralEventManager.onGameStart -= OnGameStart;} //left is subscriber
        else{centralEventManager.onGameStartMultiplayer -= OnGameStart; }
        ball.ABrickHit -= DestroyBrick;
        levelController.ObjectsSetNewRound -= SetNewRound;
   }

    // private void OnTriggerEnter(Collider other)
    // {
    //     if(other.gameObject.GetComponent<Ball>())
    //     {
    //         //Destroy(gameObject);
    //         Debug.Log("Buraya giriyo bricks.cs ontrigger");
    //         Instantiate(deathParticles, transform.position, Quaternion.identity);
    //         gameObject.SetActive(false); //or destroy

    //     }

    // }
}
