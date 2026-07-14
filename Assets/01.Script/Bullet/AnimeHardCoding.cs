using UnityEngine;

public class buAnimeHardCoding : MonoBehaviour
{
    public Sprite[] sprites;
    SpriteRenderer sp;
    public float frame;
    private float timer;
    private int count;
    private void Awake()
    {
        sp=GetComponent<SpriteRenderer>();
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
            
            if (count >= sprites.Length)
            {
                GameManager.instance.pung.boomPool[gameObject.name].Push(gameObject);
                gameObject.SetActive(false);
                
                return;
            }
            sp.sprite = sprites[count]; 
            count++;
        }
    }
}
