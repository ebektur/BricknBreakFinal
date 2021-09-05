using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class Timer : MonoBehaviour // make it child of level controller
{
    [Header("Timer UI references: ")]
    [SerializeField] private Image uiFillImage;
    [SerializeField] private Text uiText;

    public int Duration { get; private set; }
    private int remainingDuration;
    public GameObject gameManagerObject;
    //Events
    private UnityAction onTimerBeginAction;
    private UnityAction<int> onTimerChangeAction;
    private UnityAction onTimerEndAction;
    public CentralEventManager centralEventManager;
    public GameController gameController;
    public LevelController levelController;

    public delegate void EndOfTimeDelegate();
    public event EndOfTimeDelegate EndofTime;


    public void Initialize(CentralEventManager centralEventManager)
    {
        //centralEventManager.onGameStart += OnGameStart;
        //gameController.newRoundStart += SetNewRound;
        levelController.ObjectsSetNewRound += SetNewRound;
        if(gameManagerObject.GetComponent<GameController>().isPractise == true)
        {centralEventManager.onGameStart += OnGameStart;} //left is subscriber
        else{centralEventManager.onGameStartMultiplayer += OnGameStart; }
    }

    public void OnGameStart()
    {
        ResetTimer();
        
        SetDuration(20)
        .OnBegin(() => Debug.Log("Timer started"))
        .OnChange((remainingSeconds) => Debug.Log("Timer changed: " + remainingSeconds))
        .OnEnd(() => Debug.Log("Timer ended"))
        .Begin();
    }

    private void ResetTimer()
    {
        uiText.text = "00.00";
        uiFillImage.fillAmount = 0f;
        Duration = remainingDuration = 0;

        onTimerBeginAction = null;
        onTimerChangeAction = null;
        onTimerEndAction = null;
    }

    public void SetNewRound()
    {
        OnGameStart();
    }

    public Timer SetDuration(int seconds)
    {
        Duration = remainingDuration = seconds;
        return this;
    }

    public Timer OnBegin(UnityAction action){
        onTimerBeginAction = action;
        return this;
    }

    public Timer OnChange(UnityAction<int> action)
    {
        onTimerChangeAction = action;
        return this;
    }

    public Timer OnEnd(UnityAction action)
    {
        onTimerEndAction = action;
        return this;
    }

    public void Begin(){
        //begin event
        if(onTimerBeginAction != null)
        {
            onTimerBeginAction.Invoke();
        }
        StopAllCoroutines();
        StartCoroutine(UpdateTimer());
    }

    private IEnumerator UpdateTimer(){
        while(remainingDuration > 0)
        {
            if(onTimerChangeAction != null)
                onTimerChangeAction.Invoke(remainingDuration);
            
            UpdateUI(remainingDuration);
            remainingDuration--;
            if(gameManagerObject.GetComponent<GameController>().isPractise == false){ClientSend.SetTime(remainingDuration);} 
            yield return new WaitForSeconds(1f);
        }
        End();
    }

    private void UpdateUI(int seconds){
        uiText.text = string.Format("{0:D2}:{1:D2}", seconds/60, seconds % 60);
        uiFillImage.fillAmount = Mathf.InverseLerp(0, Duration, seconds);
    }

    public void End(){ //Game over phase
        if (onTimerEndAction != null)
        {
            onTimerEndAction.Invoke();
            Debug.Log("Timer.cs returns time finished");
            //if(gameManagerObject.GetComponent<GameController>().isPractise == true){EndofTime();}
            EndofTime();
            //gameManager.LevelFinishedDueToDuration();
            //gameManagerObject.GetComponent<GameManager>().LevelFinishedDueToDuration();
        }
        ResetTimer();
    }

    private void OnDestroy(){
        StopAllCoroutines();
        centralEventManager.onGameStart -= OnGameStart;
    }

}
