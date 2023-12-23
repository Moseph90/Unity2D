using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class Fire : MonoBehaviour
{
    public event Action OnProjectileSpawned;

    SpriteRenderer sr;

    public float xVelocity;
    public float yVelocity;
    public Transform spawnPointRight;
    public Transform spawnPointLeft;

    public Projectile projectilePrefab;
    // Start is called before the first frame update
    void Start()
    {
        sr = GetComponent<SpriteRenderer>();

        //if (xVelocity <= 0) xVelocity = 4.0f;
        //if (yVelocity <= 0) yVelocity = 1.0f;

        if (!spawnPointLeft || !spawnPointRight || !projectilePrefab)
            Debug.Log("Please Set Default Values On " + gameObject.name);
    }

    public void fire()
    {
        SpriteRenderer projectileRenderer = projectilePrefab.GetComponent<SpriteRenderer>();
        if (sr.flipX)
        {
            Projectile currentProjectile = Instantiate(projectilePrefab, spawnPointLeft.position, spawnPointLeft.rotation);
            currentProjectile.initVel = new Vector2(-xVelocity, yVelocity);
            projectileRenderer.flipX = true;
        }
        else
        {
            Projectile currentProjectile = Instantiate(projectilePrefab, spawnPointRight.position, spawnPointRight.rotation);
            currentProjectile.initVel = new Vector2(xVelocity, yVelocity);
            projectileRenderer.flipX = false;
        }
        OnProjectileSpawned?.Invoke();
    }
}