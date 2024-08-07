using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public enum GameState { Menu, Playing, Paused, GameOver }
    public GameState State { get; private set; }

    public delegate void OnDeath();
    public static OnDeath onDeath;
    public delegate void OnGameStateChange();
    public static OnGameStateChange onGameStateChange;

    // Public methods:
    public void StartGame()
    {
        UpdateGameState(GameState.Playing);
    }

    private void OnEnable()
    {
        PlayerPhysics.onDeath += GameOver;
    }

    private void OnDisable()
    {
        PlayerPhysics.onDeath -= GameOver;
    }

    public void QuitGame()
    {
        Application.Quit();
        Debug.Log("Player quit the game");
    }

    // Private methods:
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Ensure the GameManager persists between scenes
        }
        else
        {
            Destroy(gameObject); // Ensure only one instance of GameManager exists
        }
    }

    private void GameOver()
    {
        UpdateGameState(GameState.GameOver);
    }

    private void Start()
    {
        UpdateGameState(GameState.Menu);
    }

    public void Pause()
    {
        if (State == GameState.Playing)
        {
            UpdateGameState(GameState.Paused);
        }
        else if (State == GameState.Paused)
        {
            UpdateGameState(GameState.Playing);
        }
    }

    private void UpdateGameState(GameState newState)
    {
        GameState oldState = State;
        State = newState;
        switch (newState)
        {
            case GameState.Menu:
                SceneManager.LoadScene("MainMenu");
                Time.timeScale = 1;
                break;
            case GameState.Playing:
                if (oldState != GameState.Paused)
                {
                    SceneManager.LoadScene("RandomGeneratedLevel");
                }
                Time.timeScale = 1;
                break;
            case GameState.GameOver:
                SceneManager.LoadScene("GameOver");
                Time.timeScale = 1;
                break;
            case GameState.Paused:
                Time.timeScale = 0;
                break;
        }
        onGameStateChange?.Invoke();
        Debug.Log("Gamestate updated to: " + State.ToString());
    }
}
