using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Purchasing;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class PlayerController : MonoBehaviour
{
    public ScoreController scoreController;
    public PowerUpsScoreController powerUpsScoreController;
    public GameOverController gameOverController;
    public Animator animator;
    public GameObject shieldEffect;
    public GameObject jumpIcon;
    public GameObject shieldIcon;
    public Camera cam;
    public float speed = 5f;
    public float jumpForce = 10f;
    public Transform groundCheck;
    public Transform shieldCheck;
    public float groundCheckRadius = 0.2f;
    public float shieldRadius = 1f;
    public LayerMask groundLayer;
    public Image[] healthImages;
    public int health = 3;
    public int currentKeyCount = 0;
    public int requiredKeyCount = 0;
    public int shieldDuration = 7;

    private Rigidbody2D rb;
    private BoxCollider2D playerCollider;
    private Vector2 originalColliderSize, jumpingColliderSize;
    private Vector2 originalColliderOffset, jumpingColliderOffset;
    private bool isGrounded;
    private bool wasGrounded;
    private bool isPlayingFootsteps = false;
    private bool canDoubleJumpPowerUp = false;
    private bool hasDoubleJump = false;
    private bool hasShield = false;
    private Coroutine shieldCoroutine;
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

        PlayerMovement(hSpeed);
        PlayerAnimation(hSpeed);
        PerformJump(jump);

        if (Input.GetKeyDown(KeyCode.LeftControl) || Input.GetKeyDown(KeyCode.RightControl))
        {
            if (powerUpsScoreController.sScore > 0 && !hasShield) 
            {
                ActivateShield();
            }
        }

        if (isGrounded && !wasGrounded)
        {
            ResetColliderSize();
            hasDoubleJump = false;
        }
    }

    private void PerformJump(bool jump)
    {
        if (jump)
        {
            if (isGrounded)
            {
                SoundController.Instance.Play(SoundController.Sounds.PlayerJump);
                rb.velocity = new Vector2(rb.velocity.x, 0);
                rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Force);

                hasDoubleJump = false;

                animator.SetBool("Jump", true);

                playerCollider.size = jumpingColliderSize;
                playerCollider.offset = jumpingColliderOffset;
            }
            else if (canDoubleJumpPowerUp && !hasDoubleJump)
            {
                SoundController.Instance.Play(SoundController.Sounds.PlayerJump);
                rb.velocity = new Vector2(rb.velocity.x, 0);
                rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Force);

                animator.SetBool("DoubleJump", true);

                hasDoubleJump = true;
                canDoubleJumpPowerUp = false;
                powerUpsScoreController.ResetJumpUI();
                powerUpsScoreController.JumpScoreDecreament(1);
            }

        }
        if (!wasGrounded && isGrounded)
        {
            playerCollider.size = originalColliderSize;
            playerCollider.offset = originalColliderOffset;
        }
        wasGrounded = isGrounded;
    }

    private void PlayerMovement(float hSpeed)
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
        if (hasShield)
        {
            return;
        }

        if (health > 0)
        {
            health--;

            for (int i = 0; i < healthImages.Length; i++)
            {
                healthImages[i].enabled = i < health;
                SoundController.Instance.Play(SoundController.Sounds.PlayerHurt);
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

    private void OnDrawGizmosSelected()
    {
        if (hasShield)
        {
            Gizmos.color = Color.cyan; 
            Gizmos.DrawSphere(shieldCheck.position, shieldRadius); 
        }

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(groundCheck.position, groundCheckRadius);
    }

    public void PickUpKey()
    {
        currentKeyCount++;
        Debug.Log("You picked up a key!");
        scoreController.IncreaseScore(1);
    }

    private void ResetColliderSize()
    {
        playerCollider.size = originalColliderSize;
        playerCollider.offset = originalColliderOffset;
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
        yield return new WaitForSeconds(0.75f);
        Debug.Log("Game Over");
        gameOverController.ActivateScreen();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("DoubleJumpItem"))
        {
            PickUpDoubleJumpItem();
            Destroy(other.gameObject); 
        }

        if (other.CompareTag("ShieldItem"))
        {
            PickUpShield();
            Destroy(other.gameObject);
        }
    }

    private void PickUpShield()
    {
        shieldIcon.SetActive(true);
        powerUpsScoreController.ShieldScoreIncrease(1);
        SoundController.Instance.Play(SoundController.Sounds.ShieldPickUp);
    }

    private void ActivateShield()
    {
        powerUpsScoreController.ShieldScoreDecrease(1);

        if (shieldCoroutine != null)
        {
            StopCoroutine(shieldCoroutine);
        }
        shieldCoroutine = StartCoroutine(ShieldTimer());
    }

    private IEnumerator ShieldTimer()
    {
        if(hasShield == true)
        {
            SoundController.Instance.PlayMusic(SoundController.Sounds.ShieldSound);
        }
        hasShield = true;
        shieldEffect.SetActive(true);
        Debug.Log("Shield is ON");
        yield return new WaitForSeconds(shieldDuration);
        hasShield = false;
        shieldEffect.SetActive(false);
        Debug.Log("Shield is OFF");
    }

    private void PickUpDoubleJumpItem()
    {
        jumpIcon.SetActive(true);
        powerUpsScoreController.JumpScoreIncrease(1);
        SoundController.Instance.Play(SoundController.Sounds.DoubleJumpPickUp);
        canDoubleJumpPowerUp = true;
        Debug.Log("Double jump unlcoked");
    }
}
