using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuController : MonoBehaviour
{
    public Button playButton;
    public Button quitButton;
    public Button backButton;
    public GameObject levelScreen;

    public void Awake()
    {
        playButton.onClick.AddListener(PlayGame);
        quitButton.onClick.AddListener(Quit);
        backButton.onClick.AddListener(Back);

    }
    private void PlayGame()
    {
        SoundController.Instance.Play(SoundController.Sounds.StartButtonClick);
        levelScreen.SetActive(true);
    }
    private void Back()
    {
        SoundController.Instance.Play(SoundController.Sounds.BackButtonClick);

        levelScreen.SetActive(false);
    }
    private void Quit()
    {
        SoundController.Instance.Play(SoundController.Sounds.BackButtonClick);

        Application.Quit();
    }
}
