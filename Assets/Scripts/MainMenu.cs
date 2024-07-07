using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class MainMenu : MonoBehaviour
{
    // Get the game manager instance
    private GameManager _gmInstance;

    private void Start()
    {
        _gmInstance = GameManager.Instance;
    }

    public void Play()
    {
        _gmInstance.StartLevel();
    }

    public void Quit()
    {
        Application.Quit();
        Debug.Log("Player quit the game");
    }
}
