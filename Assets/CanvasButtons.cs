using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.UI;

public class CanvasButtons : MonoBehaviour
{
    public CentralEventManager centralEventManager;
    public GameController gameController;
    public LevelController levelController;
    public GameObject CanvasButtonsOptions;
    public GameObject LevelStatusPractiseTexts;
    public GameObject Buttons;

    //pop-up stuff
    public GameObject PopUpBox;
    public TextMeshProUGUI PopUpText;

    //scores
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI TotalscoreText;
    public TextMeshProUGUI gameStatusText;

    public GameObject button;

    public delegate void PressedMoveDelegate();
    public event PressedMoveDelegate PressedMoveMulti;



    public void InitializeMultiplayer(CentralEventManager centralEventManager)
    {
        centralEventManager.onGameStart += OnGameStart; //left is subscriber
        gameController.SetCanvasButtonMultiplayer += SetCanvasMultiplayer; 
        levelController.ObjectsSetNewRound += SetNewRound;
        gameController.PleaseWait += SetNotifMultiplayer; 
    }

    public void Initialize(CentralEventManager centralEventManager)
    {
        centralEventManager.onGameStart += OnGameStart; //left is subscriber
        gameController.SetCanvasButton += SetCanvas;       
        levelController.ObjectsSetNewRound += SetNewRound;
    }

    public void SetNewRound()
    {
        ClientSend.SetMove(GetComponent<ClientOps>().myId, false);
        //OnGameStart();
    }

    public void OnGameStart()
    {
        LevelStatusPractiseTexts.SetActive(false);
        CanvasButtonsOptions.SetActive(false);
        Buttons.SetActive(false);
    }

    public void SetTextsMultiplayer(TMP_Text[] TextObjects)
    {
        //public TMP_Text[] TextObjects;
        if (TextObjects == null || TextObjects.Length == 0)
            return;
 
        // Iterate over each of the text objects in the array to find a good test candidate
        // There are different ways to figure out the best candidate
        // Preferred width works fine for single line text objects
        int candidateIndex = 0;
        float maxPreferredWidth = 0;
 
        for (int i = 1; i < TextObjects.Length; i++)
        {
            float preferredWidth = TextObjects[i].preferredWidth;
            if (preferredWidth > maxPreferredWidth)
            {
                maxPreferredWidth = preferredWidth;
                candidateIndex = i;
            }
        }
 
        // Force an update of the candidate text object so we can retrieve its optimum point size.
        TextObjects[candidateIndex].enableAutoSizing = true;
        TextObjects[candidateIndex].ForceMeshUpdate();
        float optimumPointSize = TextObjects[candidateIndex].fontSize;
 
        // Disable auto size on our test candidate
        TextObjects[candidateIndex].enableAutoSizing = false;
 
        // Iterate over all other text objects to set the point size
        for (int i = 1; i < TextObjects.Length; i++)
            TextObjects[i].fontSize = optimumPointSize;
    }

    public void SetNotifMultiplayer(string message) //pop-up loading symbol and message and blink effect? 
    {
        PopUpText.text = message;
    }

    public void SetCanvasMultiplayer(Dictionary<int, PlayerManager> players){
        TMP_Text[] TextObjects = new TMP_Text[players.Count + 1];
        LevelStatusPractiseTexts.SetActive(false);
        Debug.Log("Canvasbuttons.cs setcanvasmultiplayer");
        int x_position = -50;


        //ikinci açan kişide, dictionary playerlar oluşmuyor
        //The given key was not present in the dictionary.
        

        //BURAYI ÇÖZ SONRA
        for(int j=1; j<= players.Count; j++)
        {
            x_position = x_position + (j * 20);
            Vector3 position = new Vector3(x_position,-40f,0f);

            TextMeshProUGUI scoreTextNew = Instantiate(scoreText, position, Quaternion.identity) as TextMeshProUGUI;
            scoreTextNew.transform.SetParent(CanvasButtonsOptions.transform, false);
            scoreTextNew.text = players[j].username + ": " + players[j].currentScore.ToString();
            TextObjects[j] = scoreTextNew;
           // scoreTextNew.enableAutoSizing = false;
           // scoreTextNew.fontSize = 50;
            Debug.Log("Canvasbuttons.cs setcanvasmultiplayer" + scoreTextNew.text);
        }

        SetTextsMultiplayer(TextObjects);
        Buttons.SetActive(true);
        CanvasButtonsOptions.SetActive(true);

    }

    public void DeleteScoreTextsMultiplayer(){
        var clonesScore = GameObject.FindGameObjectsWithTag ("RoundScoreText");
        foreach (var clone in clonesScore)
        {
        Destroy(clone);
        }
    }

    public void SetCanvas(string scoreTextSt, string totalScoreTextSt, string gameStatusTextSt)
    {
        scoreText.text = scoreTextSt;
        TotalscoreText.text = totalScoreTextSt;
        gameStatusText.text = gameStatusTextSt;
        Debug.Log("enter set canvas practise");
        LevelStatusPractiseTexts.SetActive(true);
        Buttons.SetActive(true);
        CanvasButtonsOptions.SetActive(true);
    }


    public void OnDestroy()
    {
        centralEventManager.onGameStart -= OnGameStart;
        if(gameController.GetComponent<GameController>().isPractise == true){gameController.SetCanvasButton -= SetCanvas;}
        else{gameController.SetCanvasButtonMultiplayer += SetCanvasMultiplayer; }
        levelController.ObjectsSetNewRound -= SetNewRound;

    }

    public void LoadScene(int i){
        SceneManager.LoadScene(i);
    }

    public IEnumerator NotifPopUp()
    {
        PopUpBox.SetActive(true);
        yield return new WaitForSeconds(1f);
        PopUpBox.SetActive(false);
        CanvasButtonsOptions.SetActive(false);
        gameController.InitializeNewRoundMessage(); 
    }


    public void PressMove() { 
        if(gameController.GetComponent<GameController>().isPractise == false){
            Debug.Log("multiplayer Move is Pressed on button.cs");
            StartCoroutine(NotifPopUp());
            if(gameController.GetComponent<GameController>().allowNextLevel == false)
            {
                ClientSend.SetMove(true);
            }
            else{ //allownextlevel = true
                DeleteScoreTextsMultiplayer();
                //PopUpBox.SetActive(true);
                //gameController.GetComponent<GameController>().allowNextLevel = false;
                //CanvasButtonsOptions.SetActive(false); //bunu başka yere taşı
                //PopUpBox.SetActive(false);
                
            }
        }
        else{
            Debug.Log("Move is Pressed on button.cs");
            CanvasButtonsOptions.SetActive(false);
            gameController.InitializeNewRoundMessage();
        }
    }
}
