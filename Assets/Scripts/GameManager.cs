using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class GameManager : MonoBehaviour
{
    private static GameManager _instance;

    private GameState _gameState;

    public static event Action<GameState> OnGameStateChanged;

    [SerializeField] GameObject _startingPlatform;
    private SpawnMovement _startingPlatformMovementScript;

    // Game manager is a singleton
    public static GameManager Instance
    {
        get
        {
            if (_instance is null) Debug.LogError("Game Manager instance is null");
            return _instance;
        }
    }

    private void Awake()
    {
        _instance = this;
        _startingPlatformMovementScript = _startingPlatform.GetComponent<SpawnMovement>();
    }

    //TODO ge start platformen en base speed. Enklast att flytta detta till difficulty manager men det makear inte 100% sense.
    private void Start()
    {
        UpdateGameState(GameState.Playing);
      //  _startingPlatformMovementScript.Speed = DifficultyManager.
    }

    public void UpdateGameState(GameState newState)
    {
        _gameState = newState;
        switch (newState)
        {
            case GameState.Playing:
              //  StartLevel();
                break;
        }
        OnGameStateChanged?.Invoke(newState);
    }

    public void StartLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        UpdateGameState(GameState.Playing);
        Debug.Log("Gamestate is: " + _gameState.ToString());
    }
}

public enum GameState
{
    Menu,
    Playing,
    Lose
}
