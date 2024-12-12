using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelOverController : MonoBehaviour
{
    public GameObject winScreen;
    public GameObject collectedKeysMessage;
    public ParticleSystem particles;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        PlayerController player = collision.gameObject.GetComponent<PlayerController>();
        if (player != null)
        {
            if(player.currentKeyCount >= player.requiredKeyCount)
            {
                Debug.Log("Level is completed");
                LevelManager.Instance.MarkCurrentLevelComplete();
                particles.Play();
                SoundController.Instance.Play(SoundController.Sounds.LevelComplete);
                winScreen.SetActive(true);
            }
            else
            {
                collectedKeysMessage.SetActive(true);
                StartCoroutine(HideMessage());
            }
        }
    }
    private IEnumerator HideMessage()
    {
        yield return new WaitForSeconds(1.5f); 
        collectedKeysMessage.SetActive(false);
    }
}
