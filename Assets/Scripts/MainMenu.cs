using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenu : MonoBehaviour
{
    private void Start()
    {
        Time.timeScale = 1;
    }
    public void Play()
    {
        GameManager.Instance.StartGame();
    }

    public void Quit()
    {
        GameManager.Instance.QuitGame();
    }
}
