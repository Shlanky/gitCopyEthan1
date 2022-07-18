using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class spawner : MonoBehaviour
{
    [SerializeField] int numEnemiesToSpawn;
    [SerializeField] int timer;
    [SerializeField] GameObject enemy;
    int spawnedEnemyNum;
    bool canSpawn = true ;

    // Start is called before the first frame update
    void Start()
    {

    }

    IEnumerator spawnEnemy()
    {
        canSpawn = false;
        Instantiate(enemy, transform.position, enemy.transform.rotation);
        spawnedEnemyNum++;
        yield return new WaitForSeconds(timer);
        canSpawn = true;
    }

    private void Update()
    {
       if(canSpawn && spawnedEnemyNum < numEnemiesToSpawn)
        {
            StartCoroutine(spawnEnemy());
        }
    }
}
