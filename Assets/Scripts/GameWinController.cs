using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameWinController : MonoBehaviour
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
        Scene scene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(scene.buildIndex);
    }
    private void NextLevel()
    {
        LevelManager.Instance.LoadNextScene();
    }
}
