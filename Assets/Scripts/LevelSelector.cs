using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelSelector : MonoBehaviour
{
    public SceneFader Fader;

    public GameObject[] levelButtons;

    void Start()
    {
        int levelReached = PlayerPrefs.GetInt("levelReached", 1);

        for (int i = 0; i < levelButtons.Length; i++)
        {
            if (i + 1 > levelReached)
            levelButtons[i].SetActive(false);
        }
    }

    public void Select(string levelToLoad)
    {
        Fader.FadeTo(levelToLoad);
    }
}
