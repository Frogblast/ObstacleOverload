using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class DifficultyManager : MonoBehaviour
{
    [SerializeField] private float speedUpAmount = .5f;
    [SerializeField] private float speedUpInterval = 15f;
    [SerializeField] private float platformBaseSpeed = 20f;

    public static event Action<float, float> OnIncreaseDifficulty; // Platform speed, speed up amount and game level
    public static event Action<float, float> OnStart;
    internal static Action<int> OnIncreaseLevel;

    [SerializeField] private int levelUpTime = 30;
    private float timeUntilIncreaseLevel = 0;
    private int _currentLevel = 1; // Starts at level 1
    private float timeUntilIncreaseSpeed = 0;


    public float GetPlatformBaseSpeed()
    {
        return platformBaseSpeed;
    }

    private void Start()
    {
        if (OnStart != null)
            OnStart(platformBaseSpeed, 0);
    }

    private void FixedUpdate()
    {
        timeUntilIncreaseLevel += Time.deltaTime;
        timeUntilIncreaseSpeed += Time.deltaTime;
        IncreaseDifficulty();
        if (UpdateLevel())
        {
            Debug.Log("Increased level");
            OnIncreaseLevel(_currentLevel);
        }
    }

    private void IncreaseDifficulty()
    {

        int seconds = (int)Mathf.Round(timeUntilIncreaseSpeed);
        if (seconds > speedUpInterval)
        {
            platformBaseSpeed += speedUpAmount;
            timeUntilIncreaseSpeed -= speedUpInterval;
            OnIncreaseDifficulty?.Invoke(platformBaseSpeed, speedUpAmount);
        }
    }

    private bool UpdateLevel()
    {
        if (timeUntilIncreaseLevel > levelUpTime)
        {
            timeUntilIncreaseLevel = 0;
            _currentLevel++;
            return true;
        }
        return false;
    }


}
