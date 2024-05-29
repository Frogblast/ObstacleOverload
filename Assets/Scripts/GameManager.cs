using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public GameState State;

    public static event Action<GameState> OnGameStateChanged;

    [SerializeField] GameObject _startingPlatform;
    private PlatformMovement _startingPlatformMovementScript;

    private void Awake()
    {
        Instance = this;
        _startingPlatformMovementScript = _startingPlatform.GetComponent<PlatformMovement>();
    }

    //TODO ge start platformen en base speed. Enklast att flytta detta till difficulty manager men det makear inte 100% sense.
    private void Start()
    {
        UpdateGameState(GameState.Playing);
      //  _startingPlatformMovementScript.Speed = DifficultyManager.
    }

    public void UpdateGameState(GameState newState)
    {
        State = newState;
        switch (newState)
        {
            case GameState.Playing:
              //  StartLevel();
                break;
        }
        OnGameStateChanged?.Invoke(newState);
    }

    private void StartLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}

public enum GameState
{
    Menu,
    Playing,
    Lose
}
