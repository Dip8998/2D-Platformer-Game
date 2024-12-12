using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameOverController : MonoBehaviour
{
    public Button restartButton;
    public Button mainMenuButton;

    private void Awake()
    {
        restartButton.onClick.AddListener(RestartLevel);
        mainMenuButton.onClick.AddListener(MainMenu);
    }
    public void ActivateScreen()
    {
        gameObject.SetActive(true);
    }
    private void RestartLevel()
    {
        SceneManager.LoadScene(1);
    }
    private void MainMenu()
    {
        SceneManager.LoadScene(0);
    }
}
