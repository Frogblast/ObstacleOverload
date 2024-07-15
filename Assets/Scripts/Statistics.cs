using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Statistics : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI speedText;
    [SerializeField] private TextMeshProUGUI timer;
    private float _currentTime = 0f;
    private string speedTextDefault;
    private string timerTextDefault;
    

    private void Awake(){
        speedTextDefault = "Level: ";
        speedText.text = speedTextDefault + 1; // Set 1 as the starting level
        timerTextDefault = "Time passed: ";
    }

    private void FixedUpdate() {
        UpdateTimer();
    }

    private void UpdateTimer(){
        _currentTime += Time.deltaTime;
        timer.text = timerTextDefault + FormatTime(_currentTime);
    }

    private string FormatTime(float timeInSeconds)
    {
        int minutes = (int)timeInSeconds / 60;
        int seconds = (int)timeInSeconds % 60;
        int decimals = (int)((timeInSeconds - (int)timeInSeconds) * 100);

        return $"{minutes:D2}:{seconds:D2}:{decimals:D2}";
    }

    private void OnEnable(){
        DifficultyManager.OnIncreaseLevel += UpdateSpeedText;
    }

    private void UpdateSpeedText(int level){
        speedText.text = speedTextDefault + level;
    }
}
