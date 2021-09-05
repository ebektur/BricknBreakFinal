using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UserLoginPanel : MonoBehaviour
{
    public FirebaseManager firebaseManager;
    // Start is called before the first frame update
    void Start()
    {
        gameObject.SetActive(true);
        firebaseManager.LoginSuccessful += CloseLoginPanel;
    }

    // Update is called once per frame
    public void CloseLoginPanel()
    {
        gameObject.SetActive(false);
    }
}
