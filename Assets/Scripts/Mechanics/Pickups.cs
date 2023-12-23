using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pickups : MonoBehaviour
{
    public enum PickUpType
    {
        powerUp, life, score, magic, collect
    }

    public PickUpType currentPickUp;
    public AudioClip pickupSound;

    private void OnTriggerEnter2D(Collider2D collision)
    {

        if (collision.CompareTag("Player"))
        {
            collision.GetComponent<PlayerController>().PlayPickUpSound(pickupSound);
            switch(currentPickUp)
            {
                case PickUpType.powerUp:
                    collision.GetComponent<PlayerController>();
                    break;
                case PickUpType.life:
                    GameManager.Instance.lives++;
                    break;
                case PickUpType.score:
                    GameManager.Instance.score++;
                    break;
                case PickUpType.magic:
                    GameManager.Instance.magic++;
                    break;
                case PickUpType.collect:
                    GameManager.Instance.collect++;
                    break;
            }
            //Destroy(gameObject);
        }
    }
}
