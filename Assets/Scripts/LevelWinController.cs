using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelWinController : MonoBehaviour
{
    public Button restartButton;
    public Button nextLevelButton;
    

    private void Awake()
    {
        restartButton.onClick.AddListener(RestartLevel);
        nextLevelButton.onClick.AddListener(NextLevel);

    }
    private void RestartLevel()
    {
        SoundController.Instance.Play(SoundController.Sounds.StartButtonClick);
        Scene scene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(scene.buildIndex);
    }
    private void NextLevel()
    {
        SoundController.Instance.Play(SoundController.Sounds.LevelButtonClick);
        LevelManager.Instance.LoadNextScene();
    }
}
