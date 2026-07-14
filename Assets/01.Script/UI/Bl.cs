using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Bl : MonoBehaviour
{
    void OnEnable()
    {
        StartCoroutine(Blink());
    }

    IEnumerator Blink()
    {
        while (true)
        {   
            gameObject.GetComponent<Image>().color = new Color(1f, 1f, 1f, 0f);
            yield return new WaitForSecondsRealtime(0.3f);
            gameObject.GetComponent<Image>().color = new Color(1f, 1f, 1f, 0.8f);
            yield return new WaitForSecondsRealtime(0.3f);
        }
    }
}