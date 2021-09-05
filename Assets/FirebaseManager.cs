using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Firebase;
using Firebase.Auth;
using UnityEngine.Events;
//using TMPro;

public class FirebaseManager : MonoBehaviour
{
    public Text messageText;
    public CentralEventManager centralEventManager;

    [Header("Login")]
    public InputField emailInput;
    public InputField passwordInput;
    public GameObject loginPanel;
    public Toggle rememberMe;
    //signup panel
    //profile panel
    //forget password panel

    public delegate void LoginSuccessfulDelegate();
    public event LoginSuccessfulDelegate LoginSuccessful;


    [Header("Firebase")]
    public DependencyStatus dependencyStatus;
    public FirebaseAuth auth;
    public FirebaseUser user;

    //[Header("Firebase Sign-In States")]



    // public void OpenLoginPanel()
    // {
    //     loginPanel.SetActive(true);
    // }

    public void RegisterButton()
    {

    }


    public void LoginButton()
    {
        if(string.IsNullOrEmpty(emailInput.text) 
        && string.IsNullOrEmpty(passwordInput.text))
        {
            showNotificationMessage("Email/Password is empty");
            return;
            //send break if necessary
        }
        StartCoroutine(Login(emailInput.text, passwordInput.text));
    }

    private IEnumerator Login(string email, string password)
    {

        Debug.Log("Firebase Auth login");
        var LoginTask = auth.SignInWithEmailAndPasswordAsync(email, password);
        yield return new WaitUntil(predicate: () => LoginTask.IsCompleted);

        if(LoginTask.Exception != null)
        {
            Debug.LogWarning(message: $"Failed to register task with {LoginTask.Exception}");
            FirebaseException firebaseEx = LoginTask.Exception.GetBaseException() as FirebaseException;
            AuthError errorCode = (AuthError)firebaseEx.ErrorCode;
            string message = "Login Failed";
            switch(errorCode)
            {
                case AuthError.MissingEmail:
                    message = "Missing Email";
                    break;
                case AuthError.MissingPassword:
                    message = "Missing Password";
                    break;
                case AuthError.WrongPassword:
                    message = "Wrong Password";
                    break;
                case AuthError.InvalidEmail:
                    message = "Invalid Email";
                    break;
                case AuthError.UserNotFound:
                    message = "User is not found";
                    break; 
            }

            messageText.text = message;
        }
        else{
            user = LoginTask.Result;
            Debug.Log("Login successful");
            loginPanel.SetActive(false);
            centralEventManager.Activate();
            LoginSuccessful();
            //Open MainPage

        }
    }
    public void ResetPasswordButton()
    {

    }

    void OnPasswordReset(){}

    private void showNotificationMessage(string title)
    {
        messageText.text = "" + title;
    }

    public void resetNotificationMesage()
    {
        messageText.text = ""; //invoke this after some seconds
    }

    public void LogOut()
    {
        Debug.Log("Logout is pressed");
    }

    private void Awake()
    {
        FirebaseApp.CheckAndFixDependenciesAsync().ContinueWith(
            task => {
                dependencyStatus = task.Result;
                if(dependencyStatus == DependencyStatus.Available)
                {
                    InitializeFirebase();
                }
                else
                {
                    Debug.LogError("Could not resolve all Firebase dependencies: " + dependencyStatus);
                }
            }
        );
    }

    private void InitializeFirebase()
    {
        Debug.Log("Setting up Firebase Auth");
        auth = FirebaseAuth.DefaultInstance;
        auth.StateChanged += AuthStateChanged;
        AuthStateChanged(this, null);
    }

    void AuthStateChanged(object sender, System.EventArgs eventArgs)
    {
        if(auth.CurrentUser != user) //remember me phase eksik, auth state change!!
        {
            bool signedIn = user != auth.CurrentUser && auth.CurrentUser != null;
            if(!signedIn && user != null)
            {
                Debug.Log("Signed out " + user.UserId);
            }

            user = auth.CurrentUser;
            if(signedIn)
            {
                Debug.Log("Signed in" + (user.DisplayName ?? "Anonymous"));
                //if it is signed in, no need to login again,
            }
        }
    }

    void OnDestroy() {
    auth.StateChanged -= AuthStateChanged;
    auth = null;
    }
}
