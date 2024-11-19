using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayerController : MonoBehaviour
{
    public Animator animator;
    private BoxCollider2D playerCollider;
    private Vector2 originalColliderSize, crouchingColliderSize;
    private Vector2 originalColliderOffset, crouchingColliderOffset;



    private void Start()
    {
        playerCollider = GetComponent<BoxCollider2D>();
        originalColliderSize = playerCollider.size;
        originalColliderOffset = playerCollider.offset;
        crouchingColliderSize = new Vector2(originalColliderSize.x, originalColliderSize.y / 2);
        crouchingColliderOffset = new Vector2(originalColliderOffset.x, originalColliderOffset.y/2); // Adjust the offset
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.LeftControl))
        {
            Crouching();
        }
        else if (Input.GetKeyUp(KeyCode.LeftControl))
        {
            StandUp();
        }

        // Vertical Input
        float vSpeed = Input.GetAxisRaw("Vertical");
       
        if(vSpeed > 0)
        {
            //Jump Animation
        }

        // Horizontal Input
        float hSpeed = Input.GetAxisRaw("Horizontal");

        animator.SetFloat("RunSpeed", Mathf.Abs(hSpeed));

        Vector2 scale = transform.localScale;

        if (hSpeed < 0)
        {
            scale.x = -1f * Mathf.Abs(scale.x);
        }
        else if(hSpeed > 0)
        {
            scale.x = Mathf.Abs(scale.x);
        }
        transform.localScale = scale;
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
