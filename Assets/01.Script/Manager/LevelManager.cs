using System.Collections;
using TMPro;
using UnityEngine;

public class LevelManager : MonoBehaviour
{   
    public static LevelManager instance;

    public int level;
    public float time;

    public LevelData currentLevelData;
    public MapData currentMapData;

    [Header("Setting")]

    public LevelData[] levelData;
    public float changeLevelTime;

    public float inTime;
    public float outTime;
    public float enemySpTime;
    public float startTime;

    [SerializeField] CloudMove cloud;
    [SerializeField] GameObject whiteBox;
    [SerializeField] EnemySpawn enemySpawn;
    [SerializeField] TextMeshProUGUI stage;

    private bool notone = false;
    private bool a;
    private void Awake()
    {
        instance = this;
    }
    private void Start()
    {
        a = true;
        level = 0;
        SetLevel();
        StartCoroutine(St());
    }


    private void Update()
    {   
        if(a) return;
        time += Time.deltaTime;
        if (time > changeLevelTime)
        {
            time = 0f;
            level++;
            if (level >= levelData.Length)
            {
                StartCoroutine(En());
            }
            else
            {
                StartCoroutine(SpawnWait());
                if (!notone)
                {
                    notone = true;
                    changeLevelTime += startTime;
                }
            }
        }
    }
    private IEnumerator En()
    {
        yield return null;
        UIManager.instance.clearText.GameClear();
    }
    public void SetLevel()
    {
        currentLevelData = levelData[level];
        currentMapData = currentLevelData.map;
        cloud.ChangeLevel(currentMapData);
        stage.text = $"STAGE {level + 1}/{levelData.Length}";
    }
    public IEnumerator SpawnWait()
    {   
        enemySpawn.wait = true;
        yield return new WaitForSeconds(enemySpTime);
        whiteBox.SetActive(true);
        SoundManager.instance.PlayStageBGM(level);
        
        yield return new WaitForSeconds(inTime);
        
        SetLevel();
        yield return new WaitForSeconds(enemySpTime);

        enemySpawn.wait = false;
    }

    public void HyunWo()
    {
        StartCoroutine(St());
    }
    private IEnumerator St()
    {
        yield return new WaitForSeconds(startTime);
        a = false;
        enemySpawn.wait = false;
        ScoreManager.instance.stopSc = false;
    }
}
