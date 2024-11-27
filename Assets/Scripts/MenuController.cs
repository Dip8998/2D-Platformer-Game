using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuController : MonoBehaviour
{
    public Button playButton;
    public Button quitButton;

    public void Awake()
    {
        playButton.onClick.AddListener(PlayGame);
        playButton.onClick.AddListener(Quit);
    }
    private void PlayGame()
    {
        SceneManager.LoadScene(1);
    }
    private void Quit()
    {
        Application.Quit();
    }
}
