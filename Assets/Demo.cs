using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Demo : MonoBehaviour
{
    [SerializeField] Timer timer1; 
    // Start is called before the first frame update
    public void Start()
    {
        timer1.SetDuration(20)
        .OnBegin(() => Debug.Log("Timer started"))
        .OnChange((remainingSeconds) => Debug.Log("Timer changed: " + remainingSeconds))
        .OnEnd(() => Debug.Log("Timer ended"))
        .Begin();
    }

    public void ChangeDuration(int seconds)
    {
        timer1.SetDuration(seconds)
        .OnBegin(() => Debug.Log("Timer started"))
        .OnChange((remainingSeconds) => Debug.Log("Timer changed: " + remainingSeconds))
        .OnEnd(() => Debug.Log("Timer ended"))
        .Begin();
    }
}
