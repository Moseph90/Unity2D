using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level : MonoBehaviour
{
    public int startingLives = 3;
    public Transform SpawnPoint;
    // Start is called before the first frame update
    void Start()
    {
        GameManager.Instance.spawnPlayer(SpawnPoint);
        GameManager.Instance.lives = startingLives;
    }
}
