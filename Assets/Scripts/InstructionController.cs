using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class InstructionController : MonoBehaviour
{
    public Button playButton;
    public Button backButton;

    private void Awake()
    {
        playButton.onClick.AddListener(PlayButton);
        backButton.onClick.AddListener(BackButton);
    }
    public void PlayButton()
    {
        SoundController.Instance.Play(SoundController.Sounds.ConfirmButtonClick);
        gameObject.SetActive(false);
    }
    private void BackButton()
    {
        SoundController.Instance.Play(SoundController.Sounds.BackButtonClick);
        SceneManager.LoadScene(0);
    }
}
