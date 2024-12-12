using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public Transform pointA;
    public Transform pointB;
    public float speed;
    private Transform targetPoint;

    private void Start()
    {
        targetPoint = pointA;
        //anim.SetBool()
    }

    private void Update()
    {
        transform.position = Vector2.MoveTowards(transform.position, targetPoint.position, speed* Time.deltaTime);

        if(Vector2.Distance(transform.position, targetPoint.position) < 0.1f)
        {
            targetPoint = ((targetPoint == pointA) ? pointB : pointA);

            Vector3 scale = transform.localScale;
            scale.x *= -1;
            transform.localScale = scale;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
       
        PlayerController playerController = collision.gameObject.GetComponent<PlayerController>();
        if (playerController != null && playerController.enabled)
        {
            playerController.ReduceHealth(false);
        }
    }
   
}
