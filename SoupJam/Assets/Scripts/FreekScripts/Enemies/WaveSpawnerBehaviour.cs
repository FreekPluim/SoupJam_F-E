using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveSpawnerBehaviour : MonoBehaviour
{
    [SerializeField] GameObject enemyPrefab;
    [SerializeField] List<Enemy> enemies = new List<Enemy>();
    List<Enemy> availableEnemies = new List<Enemy>();


    [SerializeField] float spawnRadiusMin = 14;
    [SerializeField] float spawnRadiusMax = 16;
    [SerializeField] float timeBetweenSpawn = 1;

    [SerializeField] int spendAmount;
    [SerializeField] int spendAmountMax;

    private void Start()
    {
        spendAmount = spendAmountMax;
        StartCoroutine(AddMoney());
        StartCoroutine(SpawnEnemy());
    }

    IEnumerator AddMoney()
    {
        yield return new WaitForSeconds(10);
        spendAmountMax = spendAmountMax + 3;
        spendAmount = spendAmountMax;

        if (availableEnemies.Count == 0)
            StartCoroutine(SpawnEnemy());
    }

    IEnumerator SpawnEnemy()
    {
        GameObject newEnemy = enemyPrefab;

        //Get random enemy
        newEnemy.GetComponent<EnemyBehaviour>().SO = selectRandomEnemy();

        //Get random spawnlocation
        Vector3 spawnPos = Random.insideUnitSphere;
        spawnPos.y = 0;

        //Spawn Enemy
        Instantiate(newEnemy, spawnPos * Random.Range(spawnRadiusMin, spendAmountMax), Quaternion.identity);
        yield return new WaitForSeconds(timeBetweenSpawn);

        //Check if there are still enemies that can spawn;
        if(availableEnemies.Count > 0)
        {
            StartCoroutine(SpawnEnemy());
        }
    }

    Enemy selectRandomEnemy()
    {
        availableEnemies.Clear();

        for (int i = 0; i < enemies.Count; i++)
        {
            if(enemies[i].Cost <= spendAmount)
            {
                availableEnemies.Add(enemies[i]);
            }
        }

        Enemy randomEnemy = availableEnemies[Random.Range(0, availableEnemies.Count)];
        spendAmount = spendAmount - availableEnemies[Random.Range(0, availableEnemies.Count)].Cost;
        return randomEnemy;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, spawnRadiusMin);

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, spawnRadiusMax);

    }

}
