using System.Collections;
using UnityEngine;

public class ChangeMap : MonoBehaviour
{
    private SpriteRenderer sp;
    float inTime;
    float outTime;
    private void Awake()
    {
        sp = GetComponent<SpriteRenderer>();
    }
    
    private void OnEnable()
    {
        inTime = LevelManager.instance.inTime;
        outTime = LevelManager.instance.outTime;
        StartCoroutine(WhiteBox());
    }
    private IEnumerator WhiteBox()
    {   
        Color a = sp.color;
        for (float t = 0; t < inTime; t += Time.deltaTime)
        {
            a.a = Mathf.Lerp(0f, 1f, t / inTime);
            sp.color = a;
            yield return null;
        }
        
        for (float t = 0; t < outTime; t += Time.deltaTime)
        {
            a.a = Mathf.Lerp(1f, 0f, t / outTime);
            sp.color = a;
            yield return null;
        }
        a.a = 0f;
        sp.color = a;
        gameObject.SetActive(false);
    }
}
