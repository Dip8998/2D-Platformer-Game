using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthController : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        PlayerController playerController = collision.gameObject.GetComponent<PlayerController>();
        if (playerController != null)
        {
            if (playerController.health < playerController.healthImages.Length)
            {
                playerController.AddHealth(1);
                Destroy(gameObject);
            }
            else
            {
                Debug.Log("Health is full. Cannot pick up health.");
            }
        }
    }
}
