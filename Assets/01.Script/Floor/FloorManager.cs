using System.Collections.Generic;
using UnityEngine;


public class FloorManager : MonoBehaviour
{
    [HideInInspector] public List<GameObject> rowPool = new List<GameObject>();
    [HideInInspector] public List<Transform> cityPool = new List<Transform>();

    [Header("set")]
    public float horizonY;
    public float downY;


    [Header("row")]
    public GameObject row;

    private float[] t;
    public int rowCount;

    public int gaMin;
    public int gaMax;
    public float seMin;
    public int seMax;
    
    [Header("building")]
    public Transform city;

    //여기 수정해야함
    //왜 수정해야했는지 까먹음;;
    public GameObject ihatethis;
    public int buildingCount;

    public float bloScMin;
    public float bloScMax;

    [Header("nukgu")]
    public float speed;
    public float whyGok;

    public float sideSpeed;

    [Header("vibe")]
    public float cycleSpeed = 0.5f;
    float currentCycle;
    float random;
    private void Start()
    {              
        SohwanRow();
    }
    public void Update()
    {
        for (int i = 0; i < rowCount; i++)
        {
            GameObject ro = rowPool[i];
            float tt = t[i];

            tt += speed * Time.deltaTime;

            if (tt >= 1f)
            {
                tt -= 1f;
                Sindosi(i);
            }
            t[i] = tt;
            float realT = Mathf.Pow(tt, whyGok);

            float yOffset = GameManager.instance.playerMove.playerLerpY * GameManager.instance.playerMove.plyYPower;
            float y = Mathf.Lerp(horizonY + yOffset, downY + yOffset, realT);

            float ga = Mathf.Lerp(gaMin, gaMax, realT);
            float se = Mathf.Lerp(seMin, seMax, realT);
            ro.transform.localScale = new Vector3(ga, se, 1);

            float x = -GameManager.instance.playerMove.playerLerpX * sideSpeed * realT;

            ro.transform.position = new Vector3(x, y, 0);
            
            SpriteRenderer sp = ro.GetComponent<SpriteRenderer>();
            Color co = sp.color;
            co.a = Mathf.Lerp(0f, 1f, tt);
            sp.color = co;

            cityPool[i].position = ro.transform.position;

            for (int j = 0; j < cityPool[i].childCount; j++)
            {
                GameObject sans = cityPool[i].GetChild(j).gameObject;

                if (!sans.activeSelf) continue;
                BuildingData data = sans.GetComponent<BuildingData>();

                float buildingX = x + data.localX * ga;
                float buildingY = y - se / 2 ;
                float buildingScale = Mathf.Lerp(bloScMin, bloScMax, realT) * data.baseScale;

                sans.transform.position = new Vector3(buildingX, buildingY, 0);
                sans.transform.localScale = new Vector3(buildingScale, buildingScale, 1);
            }
        }

        currentCycle += Time.deltaTime * cycleSpeed;
        float wave = (Mathf.Sin(currentCycle) + 1f) * 0.5f;
        random = Mathf.Lerp(LevelManager.instance.currentMapData.minRandom, LevelManager.instance.currentMapData.maxRandom, wave);

    }
    public void SohwanRow()
    {   
        t = new float[rowCount];
        for(int i=0; i<rowCount; i++)
        {
            GameObject rowrow = Instantiate(row,transform);
            t[i] = (float)i / rowCount;
            float yy = Mathf.Lerp(downY, horizonY, t[i]);
            rowrow.transform.position= new Vector3(0, yy ,0);
            rowPool.Add(rowrow);

            Transform citycity = Instantiate(city,transform);
            cityPool.Add(citycity);
        }

        Minecraft();
    }
    public void Minecraft()
    {
        for (int i = 0; i < rowCount; i++) 
        {
            for (int j = 0; j < buildingCount; j++)
            {
                GameObject builbuil = Instantiate(ihatethis, cityPool[i]);
                builbuil.transform.position = new Vector3(builbuil.transform.position.x, builbuil.transform.position.y-0.5f, builbuil.transform.position.z);
                builbuil.SetActive(false);
            }
        }
    }
    public void Sindosi(int i)
    {
        MapData map = LevelManager.instance.currentMapData;
        for (int j = 0; j < buildingCount; j++)
        {
            GameObject sans = cityPool[i].GetChild(j).gameObject;
            if (Random.value < random)
            {
                BuildingData data = sans.GetComponent<BuildingData>();

                float lineT = buildingCount <= 1 ? 0.5f : (float)j / (buildingCount - 1); //붙은 정도 조절 젬나이
                float localX = Mathf.Lerp(-0.85f * map.guri, 0.85f * map.guri, lineT);
                localX += Random.Range(-0.08f, 0.08f);

                data.localX = Mathf.Clamp(localX, -0.95f, 0.95f);
                data.baseScale = Random.Range(map.Sc *0.9f, map.Sc*1.1f);

                sans.GetComponent<SpriteRenderer>().sprite = map.building[Random.Range(0, map.building.Length)];

                sans.SetActive(true);
            }
            else
            {
                sans.SetActive(false);
            }
        }
    }
}
