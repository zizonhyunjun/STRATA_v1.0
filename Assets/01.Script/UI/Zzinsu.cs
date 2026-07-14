using System.Collections;
using UnityEngine;

public class Zzinsu : MonoBehaviour
{
    public void St()
    {
        Debug.Log("start");
        LevelManager.instance.HyunWo();

    }
    private void Start()
    {
        StartCoroutine(Test());

    }
    public IEnumerator Test()
    {
        yield return new WaitForSeconds(1);
        LevelManager.instance.HyunWo();

    }

}
