using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelOverController : MonoBehaviour
{
    public GameObject winScreen;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.GetComponent<PlayerController>()  != null)
        {
            Debug.Log("Level is completed");
            LevelManager.Instance.MarkCurrentLevelComplete();
            winScreen.SetActive(true);
        }
    }
}