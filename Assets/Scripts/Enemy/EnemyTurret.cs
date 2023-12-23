using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Tracing;
using System.Xml.Serialization;
using UnityEngine;

[RequireComponent(typeof(Fire))]
public class EnemyTurret : Enemy
{
    Fire fireScript;
    AudioSourceManager asm;
    public AudioClip fireSound;
    public AudioClip death;

    GameObject playerObj;
    public float projectileFireRate;
    float timeSinceLastFire;
    bool flipped;
    // Start is called before the first frame update
    protected override void Start()
    {
        //playerObj = GameObject.FindWithTag("Player");
        //playerPosition = playerObj.GetComponent<PlayerController>();
        base.Start();

        fireScript = GetComponent<Fire>();
        asm = GetComponent<AudioSourceManager>();

        if (!asm) Debug.Log("No Audio Source Manager Reference");
        if (!fireScript) Debug.Log("No Fire Script Reference");

        if (projectileFireRate <= 0 ) 
            projectileFireRate = 1.5f;

        fireScript.OnProjectileSpawned += PlayFireSound;
    }
    void PlayDeathSound()
    {
        asm.PlayOneShot(death, false);
    }
    void PlayFireSound()
    {
        asm.PlayOneShot(fireSound, false);
    }

    // Update is called once per frame
    void Update()
    {
        if (GameManager.Instance.playerInstance == null) return;
        AnimatorClipInfo[] curPlayingclips = anim.GetCurrentAnimatorClipInfo(0);

        if (GameManager.Instance.playerInstance.transform.position.x < gameObject.transform.position.x)
        {
            if (!flipped)
                gameObject.transform.position = new Vector3(gameObject.transform.position.x + 0.4f, 
                    gameObject.transform.position.y, gameObject.transform.position.z);
            sr.flipX = true;
            flipped = true;
        }
        else
        {
            if (flipped)
                gameObject.transform.position = new Vector3(gameObject.transform.position.x - 0.4f,
                    gameObject.transform.position.y, gameObject.transform.position.z);
            sr.flipX = false;
            flipped = false;
        }
        float distance = Mathf.Abs(GameManager.Instance.playerInstance.transform.position.x - gameObject.transform.position.x);

        if (curPlayingclips[0].clip.name != "Fire" && distance <= 10)
        {
            if (Time.time >= timeSinceLastFire + projectileFireRate)
            {
                anim.SetTrigger("Fire");
                timeSinceLastFire = Time.time;
            }
        }
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("PlayerProjectile")) PlayDeathSound();
    }
}