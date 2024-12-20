using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
public class LevelManager : MonoBehaviour
{
    public string[] levels;
    public int[] keysPerLevel;
    public PlayerController player;
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
        for(int i = 0; i < levels.Length; i++)
    {
            if (i == 0)
            {
                SetLevelStatus(levels[i], LevelStatus.Unlocked); // Unlock Level 1
            }
            else
            {
                SetLevelStatus(levels[i], LevelStatus.Locked); // Lock other levels
            }
        }
    }

    public void MarkCurrentLevelComplete()
    {
        Scene currentScene = SceneManager.GetActiveScene();
        Debug.Log("Marking current level complete: " + currentScene.name);
        SetLevelStatus(currentScene.name, LevelStatus.Completed);

        int currentSceneIndex = Array.FindIndex(levels, level => level == currentScene.name);
        int nextSceneIndex = currentSceneIndex + 1;

        if (nextSceneIndex < levels.Length)
        {
            LevelStatus nextLevelStatus = GetLevelStatus(levels[nextSceneIndex]);
            if (nextLevelStatus == LevelStatus.Locked) 
            {
                SetLevelStatus(levels[nextSceneIndex], LevelStatus.Unlocked);
            }
        }
    }

    public void LoadNextScene()
    {
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        int nextSceneIndex = currentSceneIndex + 1;

        if (nextSceneIndex < SceneManager.sceneCountInBuildSettings)
        {
            Debug.Log("Loading next scene: " + nextSceneIndex);
            SceneManager.LoadScene(nextSceneIndex);
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
        PlayerPrefs.Save();
        Debug.Log("Level number " + level + " level status " + status);
    }
    private void SetRequiredKeysForCurrentLevel()
    {
        Scene currentScene = SceneManager.GetActiveScene();
        int currentSceneIndex = Array.FindIndex(levels, level => level == currentScene.name);

        if(currentSceneIndex >= 0 && currentSceneIndex < keysPerLevel.Length)
        {
            if (player != null) 
            {
                player.requiredKeyCount = keysPerLevel[currentSceneIndex];
            }
        }
    }
   
}
