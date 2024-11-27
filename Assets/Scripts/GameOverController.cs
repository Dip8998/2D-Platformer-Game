using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameOverController : MonoBehaviour
{
    public Button restartButton;

    private void Awake()
    {
        restartButton.onClick.AddListener(RestartLevel);
    }
    public void ActivateScreen()
    {
        gameObject.SetActive(true);
    }
    private void RestartLevel()
    {
        SceneManager.LoadScene(1);
    }
}
