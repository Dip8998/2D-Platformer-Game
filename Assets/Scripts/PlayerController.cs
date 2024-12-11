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
    public int currentKeyCount = 0;
    public int requiredKeyCount = 0;

    private Rigidbody2D rb;
    private BoxCollider2D playerCollider;
    private Vector2 originalColliderSize, jumpingColliderSize;
    private Vector2 originalColliderOffset, jumpingColliderOffset;
    private bool isGrounded;
    private bool wasGrounded;
    private bool isPlayingFootsteps = false;

    private void Awake()
    {
        playerCollider = GetComponent<BoxCollider2D>();
        rb = GetComponent<Rigidbody2D>();
        originalColliderSize = playerCollider.size;
        originalColliderOffset = playerCollider.offset;
        jumpingColliderSize = new Vector2(originalColliderSize.x, originalColliderSize.y / 2);
        jumpingColliderOffset = new Vector2(originalColliderOffset.x, originalColliderOffset.y * 1.25f );
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
        Vector2 velocity = rb.velocity;
        velocity.x = hSpeed * speed;
        rb.velocity = velocity;

        if (Mathf.Abs(hSpeed) > 0.1f && isGrounded)
        {
            if (!isPlayingFootsteps)
            {
                StartCoroutine(PlayFootstepSound());
            }
        }
        

        if (jump && isGrounded)
        {
            SoundController.Instance.Play(SoundController.Sounds.PlayerJump);
            rb.velocity = new Vector2(rb.velocity.x, 0);
            rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Force);
            
            animator.SetBool("Jump", true);

            playerCollider.size = jumpingColliderSize;
            playerCollider.offset = jumpingColliderOffset;
        }
        if (!wasGrounded && isGrounded)
        {
            playerCollider.size = originalColliderSize;
            playerCollider.offset = originalColliderOffset;
        }
        wasGrounded = isGrounded;
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

        animator.SetBool("Jump", !isGrounded);

        if (isGrounded)
        {
            animator.SetBool("DoubleJump", false);
        }
    }

    public void AddHealth(int amount)
    {
        if(health < healthImages.Length)
        {
            health += amount;
            for(int i = 0; i < healthImages.Length; i++)
            {
                healthImages[i].enabled = i < health;
            }

            SoundController.Instance.Play(SoundController.Sounds.HealthPickup);
        }
    }

    public void ReduceHealth(bool isFalling)
    {
        if (health > 0)
        {
            health--;

            for (int i = 0; i < healthImages.Length; i++)
            {
                healthImages[i].enabled = i < health;
            }
            
            if (health <= 0)
            {
                PlayerDie(); 
            }
            else if (isFalling)
            {
                RestartAtSpawnPoint(); 
            }
            if (!isFalling)
            {
                animator.SetTrigger("PlayerHurt");
            }
              
        }
    }
    private IEnumerator PlayFootstepSound()
    {
        isPlayingFootsteps = true;
        while ((Mathf.Abs(rb.velocity.x) > 0.1f) && isGrounded)
        {
            SoundController.Instance.Play(SoundController.Sounds.PlayerMove);
            yield return new WaitForSeconds(0.35f); 
        }
        isPlayingFootsteps = false;
    }
  
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(groundCheck.position, groundCheckRadius);
    }

    public void PickUpKey()
    {
        currentKeyCount++;
        Debug.Log("You picked up a key!");
        scoreController.IncreaseScore(1);
    }

    private void RestartAtSpawnPoint()
    {
        transform.position = new Vector2(0, 0); 
        rb.velocity = Vector2.zero;
        Debug.Log("Player restarted at spawn point.");
    }

    public void PlayerDie()
    {
        SoundController.Instance.Play(SoundController.Sounds.PlayerDeath);
        rb.constraints = RigidbodyConstraints2D.FreezeAll;
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
