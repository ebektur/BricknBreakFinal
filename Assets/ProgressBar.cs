using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class ProgressBar : MonoBehaviour
{
    public GameObject GameManagerObject;
    GameObject SliderObject;
    //public float sliderPercentage = 0f;

    // Start is called before the first frame update
    public Slider slider;
    public float CurrentValue;// = GameManagerObject.GetComponent<GameManager>().sliderPercentage;

void Start()
{
    slider.value = 0f;
}

public void Update()
{
    slider.value = CurrentValue;
}
// Update is called once per frame
public void UpdateProgress() {
   // CurrentValue = GameManagerObject.GetComponent<GameManager>().sliderPercentage;
    slider.value = CurrentValue;
}

}
