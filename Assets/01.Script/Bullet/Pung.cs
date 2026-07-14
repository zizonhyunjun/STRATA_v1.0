using System.Collections.Generic;
using UnityEngine;

public class Pung : MonoBehaviour
{
    public Dictionary<string, Stack<GameObject>> boomPool = new Dictionary<string, Stack<GameObject>>();
    [SerializeField] float minSc;
    [SerializeField] float maxSc;
    public void WelcomToTheHell(Vector2 wichi,float scale, GameObject boom)
    {
        string key = boom.name;

        if (!boomPool.ContainsKey(key)) { boomPool.Add(key, new Stack<GameObject>()); }

        GameObject bo;
        if (boomPool[key].Count > 0)
        {
            bo = boomPool[key].Pop();
        }
        else
        {
            bo = Instantiate(boom, transform);
            bo.name = key;
        }
        bo.transform.localScale = Vector3.one * scale;
        bo.transform.position = wichi;
        bo.SetActive(true);
    }
}