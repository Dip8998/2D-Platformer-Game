using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
public class LevelManager : MonoBehaviour
{
    public string[] levels;
    private static LevelManager instance;
    public static LevelManager Instance { get { return instance; } }

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        if (GetLevelStatus(levels[0]) == LevelStatus.Locked || GetLevelStatus(levels[0]) == LevelStatus.Completed)
        {
            SetLevelStatus(levels[0], LevelStatus.Unlocked);
        }
    }

    public void MarkCurrentLevelComplete()
    {
        Scene currentScene = SceneManager.GetActiveScene();
        SetLevelStatus(currentScene.name, LevelStatus.Completed);

        int currentSceneIndex = Array.FindIndex(levels, level => level == currentScene.name);
        int nextSceneIndex = currentSceneIndex + 1;
        if(nextSceneIndex < levels.Length)
        {
            SetLevelStatus(levels[nextSceneIndex], LevelStatus.Unlocked);
        }


    }

    public LevelStatus GetLevelStatus(string level)
    {
        LevelStatus levelStatus = (LevelStatus)PlayerPrefs.GetInt(level,0);
        return levelStatus;
    }  
    
    public void  SetLevelStatus(string level, LevelStatus status)
    {
        PlayerPrefs.SetInt(level, (int)status);
    }

}
