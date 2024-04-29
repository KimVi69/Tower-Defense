using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CompleteLevel : MonoBehaviour
{
    public string levelToLoad = "LevelSelect";

    public SceneFader sceneFader;

    public int levelToUnlock;

    public void Menu()
    {
        PlayerPrefs.SetInt("levelReached", levelToUnlock);
        sceneFader.FadeTo(levelToLoad);
    }
}
