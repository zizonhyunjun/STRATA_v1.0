using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawn : MonoBehaviour
{
    public Stack<GameObject> enemyPool = new Stack<GameObject>();
    public GameObject enemyPre;
    private int enemyCount = 10;

    bool isSpawn = true;
    public bool wait = true;

    [SerializeField] private float SpawnMaxX;
    [SerializeField] private float SpawnMinX;
    [SerializeField] private float SpawnMaxY;
    [SerializeField] private float SpawnMinY;
    private void Start()
    {
        Spawn();
    }
    private void Update()
    {
        if (!isSpawn || wait) return;
        isSpawn = false;

        LevelData levelData = LevelManager.instance.currentLevelData;

        GameObject en;
        if( enemyPool.Count > 0)
        {   
            GameObject popEnemy = enemyPool.Pop();
            if (popEnemy.activeInHierarchy)
            {
                en = Instantiate(enemyPre, transform);
            }
            else
            {
                en = popEnemy;
            }
        }
        else
        {
            en = Instantiate(enemyPre, transform);
        }
        EnemyData enemyData = levelData.enemy[Random.Range(0, levelData.enemy.Length)];
        en.GetComponent<EnemyMove>().myData = enemyData;

        en.transform.position = new Vector2(Random.Range(SpawnMinX, SpawnMaxX), Random.Range(SpawnMinY, SpawnMaxY));
        
        //초기화 안될라면 여기에 조건추가?
        en.GetComponent<EnemyMove>().Setting();
        en.SetActive(true);

        StartCoroutine(Wait());
    }
    private IEnumerator Wait()
    {
        float t = Random.Range(LevelManager.instance.currentLevelData.enSpawnMin, LevelManager.instance.currentLevelData.enSpawnMax);
        yield return new WaitForSeconds(t);
        isSpawn = true;
    }
    private void Spawn()
    {   
        for (int i = 0; i < enemyCount; i++)
        {
            GameObject en = Instantiate(enemyPre,transform);
            enemyPool.Push(en);
            en.SetActive(false);
        }
        
    }
}
