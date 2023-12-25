using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class DifficultyManager : MonoBehaviour
{
    [SerializeField] private float speedUpAmount = .1f;
    [SerializeField] private float speedUpInterval = 10f;
    [SerializeField] private float platformBaseSpeed = 10f;

    public static event Action<float, float> OnIncreaseDifficulty; // Platform base speed and speed up amount
    public static event Action<float, float> OnStart;

    private float timeUntilIncreaseSpeed;


    private void Awake()
    {
        timeUntilIncreaseSpeed = 0f;
    }

    private void Start()
    {
        if (OnStart != null)
            OnStart(platformBaseSpeed, 0);
    }

    private void FixedUpdate()
    {
        IncreaseDifficulty();
    }

    private void IncreaseDifficulty()
    {
        timeUntilIncreaseSpeed += Time.deltaTime;

        int seconds = (int)Mathf.Round(timeUntilIncreaseSpeed);
        if (seconds > speedUpInterval)
        {
          //  Debug.Log("Seconds: " + seconds + ", Speed: " + platformBaseSpeed);
            platformBaseSpeed += speedUpAmount;
            timeUntilIncreaseSpeed -= speedUpInterval;
            if (OnIncreaseDifficulty != null)
                OnIncreaseDifficulty(platformBaseSpeed, speedUpAmount);
        }
    }


}