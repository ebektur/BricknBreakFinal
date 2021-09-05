using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Button : MonoBehaviour
{
    public void LoadScene(int i){
        SceneManager.LoadScene(i);
    }
    
    public void RestartGame() {
        Debug.Log("Move is Pressed on button.cs");
        SceneManager.LoadScene(SceneManager.GetActiveScene().name); // loads current scene
    }
}
