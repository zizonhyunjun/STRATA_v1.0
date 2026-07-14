using UnityEngine;

public class BAniHardCo : MonoBehaviour
{
    public Sprite[] sprites;
    SpriteRenderer sp;
    public float frame;
    private float timer;
    private int count;
    private void Awake()
    {
        sp = GetComponent<SpriteRenderer>();
    }
    private void OnEnable()
    {
        timer = 0f;
        count = 0;
    }
    private void Update()
    {
        timer += Time.deltaTime;
        if (timer >= frame)
        {
            timer = 0f;
            sp.sprite = sprites[count];
            count++;
            if (count >= sprites.Length) count = 0;
        }
    }
}
