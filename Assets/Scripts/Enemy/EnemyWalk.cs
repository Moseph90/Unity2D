using System.Collections;
using System.Collections.Generic;
using Unity.Burst.Intrinsics;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class EnemyWalk : Enemy
{
    AudioSourceManager asm;
    public AudioClip death;
    public float xSpeed;
    Rigidbody2D rb;
    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();

        asm = GetComponent<AudioSourceManager>();
        rb = GetComponent<Rigidbody2D>();

        rb.sleepMode = RigidbodySleepMode2D.NeverSleep;

        if (xSpeed <= 0) xSpeed = 3;
    }
    void PlayDeathSound()
    {
        asm.PlayOneShot(death, false);
    }
    public override void TakeDamage(int damage)
    {
        health -= damage;
        if (health <= 0) Destroy(transform.parent.gameObject, 1);

        if (damage == 9999)
        {

            anim.SetTrigger("Death");
            return;
        }
        
        base.TakeDamage(damage);

        Debug.Log("Enemy Walker Took " + damage.ToString() + " Damage");
    }

    void Update()
    {
        AnimatorClipInfo[] curPlayingClips = anim.GetCurrentAnimatorClipInfo(0);

        if (curPlayingClips[0].clip.name == "Walk")
            rb.velocity = sr.flipX ? new Vector2(-xSpeed, rb.velocity.y) : new Vector2(xSpeed, rb.velocity.y);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Barrier")
        {
            sr.flipX = !sr.flipX;
        }
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("PlayerProjectile")) PlayDeathSound();
    }
}
