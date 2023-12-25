using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Statistics : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI speedText;
    [SerializeField] private TextMeshProUGUI timer;
    private float _currentSpeed = 0f;
    private float _currentTime = 0f;
    private string speedTextDefault;
    private string timerTextDefault;

    // TODO: action event for each difficulty increase with time as argument. Used to call methods in different classes depending on how far into the game it is: new platforms, music, atmosphere, text messages etc.

    

    private void Awake(){
        speedTextDefault = "Current speed: ";
        speedText.text = speedTextDefault + 0;
        timerTextDefault = "Time passed: ";

    }

    private void FixedUpdate() {
        UpdateTimer();
    }

    private void UpdateTimer(){
        _currentTime += Time.deltaTime;
        timer.text = timerTextDefault + _currentTime.ToString("#.#");
    }

    private void OnEnable(){
        DifficultyManager.OnIncreaseDifficulty += UpdateSpeedText;
    }

    private void UpdateSpeedText(float baseSpeed, float amount){
        _currentSpeed += amount;
        speedText.text = speedTextDefault + _currentSpeed;
    }
}
