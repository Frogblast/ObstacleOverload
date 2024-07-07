using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    private enum GameState { Menu, Playing, Paused, GameOver}
    private GameState _gameState;

    // Public methods:
    public void StartGame()
    {
        UpdateGameState(GameState.Playing);
        PlayerControls.onDeath += GameOver;
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

    private void UpdateGameState(GameState newState)
    {
        _gameState = newState;
        switch (_gameState)
        {
            case GameState.Menu:
                SceneManager.LoadScene("MainMenu");
                Time.timeScale = 1;
                break;
            case GameState.Playing:
                SceneManager.LoadScene("RandomGeneratedLevel");
                Time.timeScale = 1;
                break;
            case GameState.GameOver:
                SceneManager.LoadScene("GameOver");
                Time.timeScale = 1;
                break;
        }
        Debug.Log("Gamestate updated to: " + _gameState.ToString());
    }
}
