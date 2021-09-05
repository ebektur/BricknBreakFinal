using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BrickBreakerEffect : MonoBehaviour
{
   public float time;

   void Start(){
       Destroy(gameObject, time);
   }
}
