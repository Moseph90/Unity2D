using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using static Unity.Collections.Unicode;

[RequireComponent(typeof(Rigidbody2D), typeof(SpriteRenderer), typeof(Animator))]
public class PlayerController : MonoBehaviour
{
    Rigidbody2D rb;
    SpriteRenderer sr;
    Animator anim;
    Transform trans;
    AudioSourceManager asm;
    Fire fireScript;
    CanvasManager cvm;

    Coroutine jumpForceChange;

    public AudioClip grunt;
    public static bool win = false;
    public bool animWin = false;

    //Movement variables
    public float speed = 5.0f;
    public float jumpForce = 300.0f;

    //Ground check
    public bool isGrounded;
    public bool isAirAttack;
    public Transform groundCheck;
    public LayerMask isGroundLayer;
    public float groundCheckRadius = 0.02f;

    //Player Transform
    public Transform PlayerTransform;

    public AudioClip jumpSound;
    public AudioClip fireSound;
    public AudioClip walkSound;
    public AudioClip stompSound;
    public AudioClip walkerDeath;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
        asm = GetComponent<AudioSourceManager>();
        fireScript = GetComponent<Fire>();
        cvm = GameObject.Find("Canvas").GetComponent<CanvasManager>();

        if (win) win = false;
        if (animWin) animWin = false;
        if (rb == null) Debug.Log("No RigidBody Reference");
        if (sr == null) Debug.Log("No Sprite Renerer Reference");
        if (anim == null) Debug.Log("No Anim Reference");
        if (asm == null) Debug.Log("No Audio Source Manager Reference");
        if (fireScript == null) Debug.Log("No Fire Script Reference");


        if (groundCheckRadius <= 0)
        {
            groundCheckRadius = 0.02f;
            Debug.Log("Ground Check Radius Was Set To Default");
        }
        if (jumpForce <= 0)
        {
            jumpForce = 300.0f;
            Debug.Log("Jump Force Was Set To Default");
        }
        if (speed <= 0)
        {
            speed = 5.0f;
            Debug.Log("Speed Was Set To Default");
        }
        if (groundCheck == null)
        {
            GameObject obj = new GameObject();
            obj.transform.SetParent(gameObject.transform);
            obj.transform.localPosition = Vector3.zero;
            obj.name = "GroundCheck";
            groundCheck = obj.transform;
        }
        fireScript.OnProjectileSpawned += OnProjectileSpawned;
    }

    public void PlayDeathSound(AudioClip clip)
    {
        asm.PlayOneShot(clip, false);
    }
    public void PlayPickUpSound(AudioClip clip)
    {
        asm.PlayOneShot(clip, false);
    }
    void OnProjectileSpawned()
    {
        asm.PlayOneShot(fireSound, false);
    }

    // Update is called once per frame
    void Update()
    {
        float hInput = Input.GetAxisRaw("Horizontal");

        if (hInput < 0f && !win)
        {
            sr.flipX = true;
        }
        else if (hInput > 0f && !win)
        {
            sr.flipX = false;
        }

        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, isGroundLayer);

        if (isGrounded && Input.GetButtonDown("Jump") && !win)
        {
            rb.AddForce(Vector2.up * jumpForce);
            asm.PlayOneShot(jumpSound, false);
        }
        if (!win)
        {
            Vector2 moveDirection = new Vector2(hInput * speed, rb.velocity.y);
            rb.velocity = moveDirection;
        }

        anim.SetFloat("hInput", Mathf.Abs(hInput));
        anim.SetBool("isGrounded", isGrounded);
        anim.SetBool("isAirAttack", isAirAttack);
        anim.SetBool("Win", animWin);

        AnimatorStateInfo stateInfo = anim.GetCurrentAnimatorStateInfo(0);
        if (Input.GetKey(KeyCode.LeftControl) && !win)
            if (!stateInfo.IsName("Attack")) anim.SetTrigger("attack");

        if (Input.GetKey(KeyCode.UpArrow) && Input.GetKey(KeyCode.Space) && !win)
        {
            anim.SetTrigger("airAttack");
            isAirAttack = true;
        }
        if (Input.GetKey(KeyCode.Return) && win)
        {
            GameManager.Instance.ChangeScene(0);
        }
    }
    
    public void IncreaseGravity()
    {
        rb.gravityScale = 10;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("PowerUps"))
        {
            Destroy(collision.gameObject);
            //asm.PlayOneShot(powerupSound, false);
        }
        if (collision.CompareTag("Squish"))
        {
            PlayDeathSound(walkerDeath);
            collision.transform.parent.gameObject.GetComponent<Enemy>().TakeDamage(9999);
            rb.velocity = Vector2.zero;
            rb.AddForce(Vector2.up * jumpForce);
            asm.PlayOneShot(stompSound, false);
        }
        if (collision.CompareTag("Enemy"))
        {
            PlayDeathSound(grunt);
            GameManager.Instance.lives--;
            GameManager.Instance.gotHit = true;
        }
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("WinCollider"))
        {
            win = true;
            animWin = true;
            anim.Play("Kneel");
            cvm.ShowText();
        }
        if (collision.gameObject.CompareTag("Enemy"))
        {
            PlayDeathSound(grunt);
            GameManager.Instance.lives--;
            GameManager.Instance.gotHit = true;
        }
    }
    public void StartJumpForceChange()
    {
        if (jumpForceChange == null) jumpForceChange = StartCoroutine(JumpForceChange());
        else
        {
            StopCoroutine(jumpForceChange);
            jumpForceChange = null;
            jumpForce /= 1.3f;
            jumpForceChange = StartCoroutine(JumpForceChange());
        }   
    }
    IEnumerator JumpForceChange()
    {
        jumpForce *= 1.3f;
        yield return new WaitForSeconds(5.0f);
        jumpForce /= 1.3f;
        jumpForceChange = null;
    }
}