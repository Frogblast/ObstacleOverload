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

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        UpdateGameState(GameState.Playing);
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
