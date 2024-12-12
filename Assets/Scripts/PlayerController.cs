using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    public ScoreController scoreController;
    public GameOverController gameOverController;
    public Animator animator;
    public Camera cam;
    public float speed = 5f;
    public float jumpForce = 10f;
    public Transform groundCheck; 
    public float groundCheckRadius = 0.2f; 
    public LayerMask groundLayer;
    public Image[] healthImages;
    public int health = 3;
   
    private Rigidbody2D rb;
    private BoxCollider2D playerCollider;

    private Vector2 originalColliderSize, crouchingColliderSize;
    private Vector2 originalColliderOffset, crouchingColliderOffset;

    private bool isGrounded;
    

    private void Awake()
    {
        playerCollider = GetComponent<BoxCollider2D>();
        rb = GetComponent<Rigidbody2D>();
        originalColliderSize = playerCollider.size;
        originalColliderOffset = playerCollider.offset;
        crouchingColliderSize = new Vector2(originalColliderSize.x, originalColliderSize.y / 2);
        crouchingColliderOffset = new Vector2(originalColliderOffset.x, originalColliderOffset.y / 2);
    }
    void Update()
    {
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);


        float hSpeed = Input.GetAxisRaw("Horizontal");
        bool jump = Input.GetButtonDown("Jump");

        PlayerMovement(hSpeed, jump);
        PlayerAnimation(hSpeed);
    }
    private void PlayerMovement(float hSpeed, bool jump)
    {
        if (animator.GetBool("Crouching"))
        {
            hSpeed = 0; 
            jump = false;
        }

        Vector2 velocity = rb.velocity;
        velocity.x = hSpeed * speed;
        rb.velocity = velocity;

        if(jump&&isGrounded)
        {
            rb.velocity = new Vector2(rb.velocity.x, 0);
            rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Force);

            animator.SetBool("Jump", true);
            if (isGrounded)
            {
                animator.SetBool("Jump", false);
            }

        }

    }
    private void PlayerAnimation(float hSpeed)
    {
        
        animator.SetFloat("RunSpeed", Mathf.Abs(hSpeed));

            Vector2 scale = transform.localScale;
            if (hSpeed < 0)
            {
                scale.x = -Mathf.Abs(scale.x);
            }
            else if (hSpeed > 0)
            {
                scale.x = Mathf.Abs(scale.x);
            }
            transform.localScale = scale;

            if (Input.GetKeyDown(KeyCode.RightControl))
            {
                Crouching();
            }
            else if (Input.GetKeyUp(KeyCode.RightControl))
            {
                StandUp();
            }

        animator.SetBool("Jump", !isGrounded);

        if (isGrounded)
        { 
            animator.SetBool("DoubleJump", false);
        }
    }

   
    private void Crouching()
    {
        animator.SetBool("Crouching", true);
        playerCollider.size = crouchingColliderSize;
        playerCollider.offset = crouchingColliderOffset;
    }
    private void StandUp()
    {
        animator.SetBool("Crouching", false);
        playerCollider.size = originalColliderSize;
        playerCollider.offset = originalColliderOffset;
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(groundCheck.position, groundCheckRadius);
    }

    public void PickUpKey()
    {
        Debug.Log("You picked up a key!");
        scoreController.IncreaseScore(1);
    }

    public void ReduceHealth()
    {
        if(health > 0)
        {
            health--;

            for(int i = 0; i < healthImages.Length; i++)
            {
                healthImages[i].enabled = i < health;
               
            }

            if(health <= 0)
            {
                PlayerDie();
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            animator.SetTrigger("PlayerHurt");
        }
    }

    public void RestartLevel()
    {
        Scene scene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(scene.buildIndex);
    }

    public void PlayerDie()
    {
        rb.constraints = RigidbodyConstraints2D.FreezePosition;
        animator.SetTrigger("PlayerDied");
        this.enabled = false;
        
        StartCoroutine(GameOver());
    }

    private IEnumerator GameOver()
    {
        yield return new WaitForSeconds(1f);

        Debug.Log("Game Over");
        gameOverController.ActivateScreen();
    }
}
