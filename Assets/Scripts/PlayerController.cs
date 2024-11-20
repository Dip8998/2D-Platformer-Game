using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;


public class PlayerController : MonoBehaviour
{
    public Animator animator;
    public float speed;
    public float jump;
    private BoxCollider2D playerCollider;
    private Vector2 originalColliderSize, crouchingColliderSize;
    private Vector2 originalColliderOffset, crouchingColliderOffset;



    private void Awake()
    {
        playerCollider = GetComponent<BoxCollider2D>();
        originalColliderSize = playerCollider.size;
        originalColliderOffset = playerCollider.offset;
        crouchingColliderSize = new Vector2(originalColliderSize.x, originalColliderSize.y / 2);
        crouchingColliderOffset = new Vector2(originalColliderOffset.x, originalColliderOffset.y/2); // Adjust the offset
    }

    void Update()
    {
        // Vertical Input
        float vSpeed = Input.GetAxisRaw("Jump");
        // Horizontal Input
        float hSpeed = Input.GetAxisRaw("Horizontal");

        PlayerMovement(hSpeed,vSpeed);
        PlayerAnimation(hSpeed, vSpeed);
    }

    private void PlayerMovement(float hSpeed, float vSpeed)
    { 
         Vector2 position = transform.position;
         position.x += hSpeed * speed * Time.deltaTime;
        transform.position = position;
    }

    private void PlayerAnimation(float hSpeed, float vSpeed)
    {
        animator.SetFloat("RunSpeed", Mathf.Abs(hSpeed));

        Vector2 scale = transform.localScale;

        if (hSpeed < 0)
        {
            scale.x = -1f * Mathf.Abs(scale.x);
        }
        else if (hSpeed > 0)
        {
            scale.x = Mathf.Abs(scale.x);
        }
        transform.localScale = scale;


        if (vSpeed > 0)
        {
            animator.SetBool("Jump", true);
        }
        else
        {
            animator.SetBool("Jump", false);
        }

        if (Input.GetKeyDown(KeyCode.LeftControl))
        {
            Crouching();
        }
        else if (Input.GetKeyUp(KeyCode.LeftControl))
        {
            StandUp();
        }
    }

    void Crouching()
    {
        animator.SetBool("Crouching", true);
        playerCollider.size = crouchingColliderSize;
        playerCollider.offset = crouchingColliderOffset;
    }
    void StandUp()
    {
        animator.SetBool("Crouching", false);
        playerCollider.size = originalColliderSize;
        playerCollider.offset = originalColliderOffset;
    }
}
