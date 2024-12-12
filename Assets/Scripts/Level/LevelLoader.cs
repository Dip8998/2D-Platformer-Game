using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

[RequireComponent(typeof(Button))]
public class LevelLoader : MonoBehaviour
{
    private Button levelButton;
    public string levelName;
    public TextMeshProUGUI levelText;

    private void Awake()
    {
        levelButton = GetComponent<Button>();
        levelButton.onClick.AddListener(LoadLevel);
    }

    private void Start()
    {
        UpdateButtonState();
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
            case LevelStatus.Completed:
                SoundController.Instance.Play(SoundController.Sounds.LevelButtonClick);
                SceneManager.LoadScene(levelName);
                break;
        }
    }

    public void UpdateButtonState()
    {
        LevelStatus levelStatus = LevelManager.Instance.GetLevelStatus(levelName);
        switch(levelStatus)
        {
            case LevelStatus.Locked:
                levelText.text = "Locked";
                levelText.color = Color.grey;
                levelButton.interactable = false;
                break;
            case LevelStatus.Unlocked:
                levelText.text = " ";
                levelButton.interactable = true;
                break;
            case LevelStatus.Completed:
                levelText.text = "Completed";
                levelText.color = Color.black;
                levelButton.interactable = true;
                break;
        }
    }
}
