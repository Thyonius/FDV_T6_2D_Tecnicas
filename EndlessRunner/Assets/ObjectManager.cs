using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectManager : MonoBehaviour
{
    [SerializeField] GameObject obstacle;

    [SerializeField] float obstacleTimerMax = 4;
    [SerializeField] float obstacleTimerMin = 3;

    [SerializeField] int obstacleAmount = 4;

    List<GameObject> obstaclePool;

    Transform generationPoint;

    float spawnTimer;

    private void Awake()
    {
        generationPoint = GameObject.FindWithTag("GenerationPoint").GetComponent<Transform>();

        spawnTimer = obstacleTimerMax;

        obstaclePool = new List<GameObject>();

        for (int i = 0; i < obstacleAmount; i++)
        {
            GameObject obj = (GameObject)Instantiate(obstacle, this.transform);
            obj.SetActive(false);
            obstaclePool.Add(obj);
        }
    }

    private void Update()
    {
        ManageSpawnTimer();
    }

    private void ManageSpawnTimer()
    {
        if (spawnTimer <= 0)
        {
            GameObject newObstacle = GetPooledObject();

            newObstacle.transform.position = generationPoint.position;
            newObstacle.SetActive(true);

            spawnTimer = UnityEngine.Random.Range(obstacleTimerMin, obstacleTimerMax);
        }
        else
        {
            spawnTimer -= Time.deltaTime;
        }
    }

    private GameObject GetPooledObject()
    {
        for (int i = 0; i < obstaclePool.Count; i++)
        {
            if (!obstaclePool[i].activeInHierarchy)
            {
                return obstaclePool[i];
            }
        }

        GameObject obj = (GameObject)Instantiate(obstacle, this.transform);
        obj.SetActive(false);
        obstaclePool.Add(obj);
        return obj;
    }
}
