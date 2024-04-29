using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameOver : MonoBehaviour
{
    public string levelToLoad = "LevelSelect";

    public GameObject ui;

    public SceneFader sceneFader;

    void Start()
    {
        Time.timeScale = 0;
        BuildManager.instance.SelectTurretToBuild(null);
    }

    public void Menu()
    {
        Time.timeScale = 1;
        sceneFader.FadeTo(levelToLoad);
    }
}
