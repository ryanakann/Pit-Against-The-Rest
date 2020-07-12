using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static bool GamePaused;

    private void Awake()
    {
        GamePaused = false;
    }

    public void Pause()
    {
        GamePaused = true;
        Time.timeScale = 0f;
    }

    public void Unpause()
    {
        GamePaused = false;
        Time.timeScale = 1f;
    }

    public void ExitGame()
    {
        Application.Quit();
    }
}
