using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private GameObject enemyPrefab;
    [SerializeField] private float respawnDelay = 10f;
    private GameObject currentEnemy;

    private void Start()
    {
        SpawnEnemy();
        Debug.Log("OK");
    }

    private void SpawnEnemy()
    {
        if (enemyPrefab == null)
        {
            return;
        }
        currentEnemy = Instantiate(enemyPrefab, transform.position, transform.rotation);
        Animal animal = currentEnemy.GetComponent<Animal>();
        if (animal != null)
        {
            animal.OnDestroyed += HandleOnEnemyDeath;
            Debug.Log("success produce");
        }
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnDestroy()
    {
        if (currentEnemy != null)
        {
            Animal animal = currentEnemy.GetComponent<Animal>();
            if (animal != null)
            {
                animal.OnDestroyed -= HandleOnEnemyDeath;
            }
        }
    }

    private void HandleOnEnemyDeath()
    {
        if (currentEnemy != null)
        {
            Animal animal = currentEnemy.GetComponent<Animal>();
            if (animal != null)
            {
                animal.OnDestroyed -= HandleOnEnemyDeath;
            }
            currentEnemy = null;
            Invoke(nameof(SpawnEnemy), respawnDelay);
        }
    }
}
