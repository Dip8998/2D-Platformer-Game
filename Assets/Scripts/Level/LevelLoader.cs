using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class LevelLoader : MonoBehaviour
{
    private Button levelButton;
    public string levelName;

    private void Awake()
    {
        levelButton = GetComponent<Button>();
        levelButton.onClick.AddListener(LoadLevel);
    }
    public void LoadLevel()
    {
        LevelStatus levelStatus = LevelManager.Instance.GetLevelStatus(levelName);
        switch (levelStatus)
        {
            case LevelStatus.Locked:
                Debug.Log("Level is locked");
                break;
            case LevelStatus.Unlocked:
                SceneManager.LoadScene(levelName);
                break;
            case LevelStatus.Completed:
                SceneManager.LoadScene(levelName);
                break;
        }
    }
}
