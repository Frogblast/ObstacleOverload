using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;


public class Pause : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI PauseElement;
    void Start()
    {
        GameManager.onGameStateChange += TogglePause;
        PauseElement.SetText("");
    }

    private void TogglePause()
    {
        if (GameManager.Instance.State == GameManager.GameState.Paused)
        {
            PauseElement.SetText("Paused");
        }
        else
        {
            PauseElement.SetText("");
        }
    }
}
