using UnityEngine;

public class ChangeZozunzum : MonoBehaviour
{
    public Sprite[] zozunzum;
    public int count;
    SpriteRenderer sp;
    private void Awake()
    {
        sp = GetComponent<SpriteRenderer>();
    }
    private void Update()
    {
        float sc = Input.mouseScrollDelta.y;
        if (sc == 0) return;

        if (sc > 0)
        {
            count++;
            if (count >= zozunzum.Length) count = 0;
        }
        else if (sc < 0)
        {
            count--;
            if (count < 0) count = zozunzum.Length - 1;
        }
        sp.sprite = zozunzum[count];
    }   
}
