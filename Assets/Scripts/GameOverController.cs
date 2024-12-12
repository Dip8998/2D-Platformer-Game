using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameOverController : MonoBehaviour
{
    public Button restartButton;
    public Button mainMenuButton;
    public Button quitButton;
  
    private void Awake()
    {
        restartButton.onClick.AddListener(RestartLevel);
        mainMenuButton.onClick.AddListener(MainMenu);
        quitButton.onClick.AddListener(Quit);
    }
    public void ActivateScreen()
    {
        gameObject.SetActive(true);
        SoundController.Instance.Play(SoundController.Sounds.GameOver);
    }
    private void RestartLevel()
    {
        Scene scene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(scene.buildIndex);
        SoundController.Instance.Play(SoundController.Sounds.StartButtonClick);
    }
    private void MainMenu()
    {
        SoundController.Instance.Play(SoundController.Sounds.BackButtonClick);
        SceneManager.LoadScene(0);
    }
    private void Quit()
    {
        SoundController.Instance.Play(SoundController.Sounds.BackButtonClick);
        Application.Quit();
    }
}
