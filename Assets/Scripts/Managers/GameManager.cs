using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Events;

[DefaultExecutionOrder(-1)]
public class GameManager : MonoBehaviour
{
    static GameManager instance = null;
    public static GameManager Instance => instance;

    public int maxLives = 5;
    private int _lives;
    private int lifeCounter;
    private int _score = 0;
    private int _magic = 5;
    private int _collect = 0;
    private bool _gotHit;

    public bool gotHit
    {
        get => _gotHit;
        set => _gotHit = value;
    }
    public int magic
    {
        get => _magic;
        set
        {
            _magic = value;
            Debug.Log("Magic Has Been Set To: " + _magic.ToString());
        }
    }
    public int score
    {
        get => _score;
        set
        {
            _score = value;
            Debug.Log("Score Has Been Set To: " + _score.ToString());
        }
    }
    public int collect
    {
        get => _collect;
        set
        {
            _score = value;
            Debug.Log("Collectibles Has Been Set To: " + _collect.ToString());
        }
    }
    public int lives
    {
        get => _lives;
        set
        {
            _lives = value;
            if (_lives > maxLives) _lives = maxLives;
            Debug.Log("Lives Has Been Set To: " + _lives.ToString());

            if (_lives == 0)
                SceneManager.LoadScene(2);
            OnLivesValueChanged?.Invoke(_lives);
        }
    }

    public PlayerController playerPrefab;
    public UnityEvent<int> OnLivesValueChanged;

    [HideInInspector] public PlayerController playerInstance;
    [HideInInspector] public Transform spawnPoint;

    // Start is called before the first frame update
    private void Start()
    {
        Debug.Log("Lives Has Been Set To: " + _lives.ToString());

        if (instance)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;
        DontDestroyOnLoad(this);
    }
    // Update is called once per frame
    void Update()
    {
        //if (Input.GetKeyDown(KeyCode.Return))
        //{
        //    switch (SceneManager.GetActiveScene().name)
        //    {
        //        case "MainMenu":
        //            SceneManager.LoadScene(1);
        //            break;
        //        case "GameOver":
        //            SceneManager.LoadScene(0);
        //            break;
        //    }
        //}
        //if (Input.GetKeyDown(KeyCode.Escape))
        //{
        //    if (SceneManager.GetActiveScene().name == "Level")
        //        SceneManager.LoadScene(0);
        //}
    }
    public void ChangeScene(int scene)
    {
        SceneManager.LoadScene(scene);
    }
    private void LateUpdate()
    {
        if (_gotHit)
        {
            _gotHit = false;
            Respawn();
            Debug.Log("Lives Has Been Set To: " + _lives.ToString());
        }
    }
    public void spawnPlayer(Transform spawnLocation)
    {
        playerInstance = Instantiate(playerPrefab, spawnLocation.position, spawnLocation.rotation);
        spawnPoint = spawnLocation;
    }
    public void Respawn()
    {
        playerInstance.transform.position = spawnPoint.position;
    }
    public void updateSpawnPoint(Transform updatedPoint)
    {
        spawnPoint = updatedPoint;
    }
}
