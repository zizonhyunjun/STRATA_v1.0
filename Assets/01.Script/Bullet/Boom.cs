using UnityEngine;

public class Boom : MonoBehaviour
{
    Animator animator;
    private void Awake()
    {
        animator = GetComponent<Animator>();
    }
    private void OnEnable()
    {
        gameObject.transform.rotation = Quaternion.Euler(0, 0, Random.Range(0f, 360f));
        animator.Play("realBoomEnemy", 0, 0f);
    }
    public void AniEnd()
    {
        gameObject.SetActive(false);
        string key = gameObject.name;
        GameManager.instance.pung.boomPool[key].Push(gameObject);
    }
}
