﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    [SerializeField] private float spawnRate = 2f;
    [SerializeField] private bool canSpawn = false;
    [SerializeField] private bool isSpawning = false;
    [SerializeField] private float spawnDistanceFromBoundary = 1f;
    [SerializeField] private float startSpawnDelay = 3f;

    public GameObject meteorPrefab;

    private Vector2 screenBounds;
    private List<GameObject> spawnedObjects = new List<GameObject>();

    private GameManager gameManager;

    public void Initializer(GameManager _gameManager)
    {
        gameManager = _gameManager;
    }

    private void Awake()
    {
        CheckPrefabs();
        GetScreenBoundary();
    }

    private void Update()
    {
        if (canSpawn && !isSpawning)
        {
            isSpawning = true;
            StartCoroutine("SpawnMeteor");
        }
    }

    IEnumerator SpawnMeteor()
    {
        GameObject instance = Instantiate(meteorPrefab, GetSpawnPoint(), Quaternion.identity);
        spawnedObjects.Add(instance);

        MeteorController meteorController = instance.GetComponent<MeteorController>();
        Action<GameObject> callback = RemoveObject;
        meteorController.Initializer(callback, gameManager);

        yield return new WaitForSeconds(1 / spawnRate);

        if (canSpawn)
            StartCoroutine("SpawnMeteor");
        else
            isSpawning = false;
    }

    private void RemoveObject(GameObject _obj)
    {
        if (spawnedObjects.Contains(_obj))
            spawnedObjects.Remove(_obj);
    }

    private Vector3 GetSpawnPoint()
    {
        Vector3 spawnPoint = Vector3.zero;

        int side = UnityEngine.Random.Range(1, 4 + 1);
        switch (side)
        {
            case 1:
                spawnPoint.x = UnityEngine.Random.Range(-screenBounds.x, screenBounds.x + 1);
                spawnPoint.y = screenBounds.y;
                break;
            case 2:
                spawnPoint.y = UnityEngine.Random.Range(-screenBounds.y, screenBounds.y + 1);
                spawnPoint.x = screenBounds.x;
                break;
            case 3:
                spawnPoint.x = UnityEngine.Random.Range(-screenBounds.x, screenBounds.x + 1);
                spawnPoint.y = -screenBounds.y;
                break;
            case 4:
                spawnPoint.y = UnityEngine.Random.Range(-screenBounds.y, screenBounds.y + 1);
                spawnPoint.x = -screenBounds.x;
                break;
        }

        return spawnPoint;
    }

    private void GetScreenBoundary()
    {
        screenBounds = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, Camera.main.transform.position.z));
        screenBounds.y += spawnDistanceFromBoundary;
        screenBounds.x += spawnDistanceFromBoundary;
    }

    private void CheckPrefabs()
    {
        if (!meteorPrefab)
        {
            Debug.Log("[Spawner][Error] Meteor prefab missing!");
            canSpawn = false;
        }
    }

    public void StartSpawner()
    {
        StartCoroutine("StartSpawnerCourotine");
    }

    public void StopSpawner()
    {
        canSpawn = false;
        isSpawning = false;
        StopAllCoroutines();
    }

    public void DestroyAllSpawnedObjects()
    {
        foreach (GameObject obj in spawnedObjects)
        {
            Destroy(obj);
        }
        spawnedObjects.Clear();
    }

    public void StopAllObjectsMovement()
    {
        foreach (GameObject obj in spawnedObjects)
        {
            obj.GetComponent<MeteorController>().StopMovement();
        }
    }

    public void ResumeAllObjectsMovement()
    {
        foreach (GameObject obj in spawnedObjects)
        {
            obj.GetComponent<MeteorController>().ResumeMovement();
        }
    }

    IEnumerator StartSpawnerCourotine()
    {
        yield return new WaitForSeconds(startSpawnDelay);

        canSpawn = true;
    }

}
