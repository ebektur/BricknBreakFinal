using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//brick quantity niye sıfır  geliyor
//duvarlar neden zıplatmıyor
//generates random bricks after level/round finishes

public class FrontWall : MonoBehaviour
{
   public float roundQuantity;
   public float BrickQuantity;
   public int width = 13;
   public int height = 10;
   public GameObject BricksPrefab;
   public GameObject FrontWallObject;
   //public GameObject GameManagerObject;
   public GameObject LevelControllerObject;
   public CentralEventManager centralEventManager;

    //public delegate void SetBrickTransformDelegate();
    //public event SetBrickTransformDelegate SetBrickTransform;

    // public void Start()
    // {
    // //slider.value = 0f;
    // }


    public void Initialize(CentralEventManager centralEventManager)
    {
        centralEventManager.onGameStart += OnGameStart; //left is subscriber
    }

    public void OnGameStart()
    {

    }

    public Vector3 SetPosition()
    {
        float x = Random.Range(5f, -5f);
        var position = new Vector3(x, Random.Range(1, 2), 5f);
        return position;
    }


}
