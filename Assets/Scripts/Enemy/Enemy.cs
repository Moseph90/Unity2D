using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer), typeof(Animator), typeof(BoxCollider2D))]
public class Enemy : MonoBehaviour
{
    AudioSourceManager asm;
    public AudioClip dead;

    protected SpriteRenderer sr;
    protected Animator anim;

    protected int health;
    public int maxHealth;
    
    protected virtual void Start()
    {
        asm = GetComponent<AudioSourceManager>();
        anim = GetComponent<Animator>();
        sr = GetComponent<SpriteRenderer>();

        if (maxHealth <= 0 ) maxHealth = 10;

        health = maxHealth;
    }
    public virtual void TakeDamage(int damage)
    {
        health -= damage;
        PlayDeathSound(dead);

        if (health <= 0) anim.SetTrigger("Death");
        Destroy(gameObject, 1);
    }
    public void PlayDeathSound(AudioClip clip)
    {
        asm.PlayOneShot(clip, false);
    }
}
