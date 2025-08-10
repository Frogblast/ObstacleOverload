using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenu : MonoBehaviour
{
    public void Pause()
    {
        GameManager.Instance.Pause();
    }

    public void Quit()
    {
        GameManager.Instance.QuitGame();
    }
}
