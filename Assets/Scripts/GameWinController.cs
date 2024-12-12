using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameWinController : MonoBehaviour
{
    public Button mainMenu;
    public Button quit;

    private void Awake()
    {
        mainMenu.onClick.AddListener(MainMenu);
        quit.onClick.AddListener(Quit); 
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
