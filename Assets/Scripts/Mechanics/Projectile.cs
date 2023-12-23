using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Projectile : MonoBehaviour
{
    AudioSourceManager asm;
    public AudioClip grunt;
    public AudioClip hit;

    EnemyTurret archer;
    EnemyWalk walker;
    public float lifeTime;
    public int damage;

    //Meant to be modified by the object creating the projectile
    // eg. the fire script
    [HideInInspector]
    public Vector2 initVel; 
    void Start()
    {
        asm = GetComponent<AudioSourceManager>();

        if (lifeTime <= 0) lifeTime = 2.0f;
        if (damage <= 0) damage = 2;
        
        GetComponent<Rigidbody2D>().velocity = initVel;
        Destroy(gameObject, lifeTime);
    }

    // Update is called once per frame
    private void OnCollisionEnter2D(Collision2D collision) 
    {
       if (collision.gameObject.layer == LayerMask.NameToLayer("Wall") || collision.gameObject.layer == LayerMask.NameToLayer("Ground"))
            Destroy(gameObject);
       if (collision.gameObject.layer == LayerMask.NameToLayer("Enemy"))
        {
            collision.gameObject.GetComponent<Enemy>().TakeDamage(damage);
            Destroy(gameObject);
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Wall") || collision.gameObject.layer == LayerMask.NameToLayer("Ground"))
        {
            Destroy(gameObject);
        }
        if (collision.CompareTag("Player"))
        {
            collision.gameObject.GetComponent<PlayerController>().PlayDeathSound(grunt);
            GameManager.Instance.lives--;
            GameManager.Instance.gotHit = true;
            Destroy(gameObject);
        }
    }
}
