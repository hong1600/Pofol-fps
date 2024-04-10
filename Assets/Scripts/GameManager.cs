using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public List<Transform> points = new List<Transform>();

    public GameObject enemy;

    public float spawnTime = 3.0f;

    private bool isGameOver;

    public bool IsGameOver
    {
        get 
        {
            return isGameOver; 
        }
        set 
        {
            isGameOver = value;

            if (isGameOver)
            {
                CancelInvoke("SpawnEnemy");
            }
        }
    }

    

    public static GameManager instance = null;

    private void Awake()
    {
        if (instance == null) 
        {
            instance = this;
        }
        else
        {
            Destroy(this.gameObject);
        }

        DontDestroyOnLoad(this.gameObject);
    }

    void Start()
    {
        Transform spawnPoint = GameObject.Find("SpawnPoint")?.transform;

        foreach (Transform point in spawnPoint)
        {
            points.Add(point);
        }

        InvokeRepeating("SpawnEnemy", 2.0f, spawnTime);
    }

    void Update()
    {

    }

    private void SpawnEnemy()
    {
        int rand = Random.Range(0, points.Count);
        
        Instantiate(enemy, points[rand].position, points[rand].rotation);
    }
}
