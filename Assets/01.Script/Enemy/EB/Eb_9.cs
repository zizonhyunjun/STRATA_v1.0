using System.Collections;
using UnityEngine;

public class Eb9 : MonoBehaviour, IEb
{   
    //6꺼에다 스케일 추가
    [Header("[ Default ]------------------------------------------------------------")]
    [Space(15f)]
    public float t;
    public float realT;
    public float realTT;
    [Space(10f)]
    [SerializeField] float tSpeed;
    [SerializeField] float whyGok;
    [SerializeField] float whyGokT;
    [Header("-----------------------------------------------------------------------")]
    [Space(10f)]
    [SerializeField] float speed;
    Vector3 basePos;
    Vector2 dir;
    [Space(10f)]
    [SerializeField] float startSc;
    [SerializeField] float endSc;
    [Space(10f)]
    public int damage = 1;
    [SerializeField] float hit;
    [Space(10f)]
    public GameObject boom;
    public float boomSc;
    float zinSu;
    [Space(10f)]
    SpriteRenderer _sp;
    public float backLayer;
    public float wait;    
    private void Awake()
    {
        _sp = GetComponent<SpriteRenderer>();
    }
    public void Go(Vector2 enemyPos, Vector2 pos)
    {
        transform.position = pos;

        t = 0f;
        realT = 0f;
        realTT = 0f;

        basePos = pos;
        transform.localScale = Vector3.one * startSc;
        if (Mathf.Approximately(pos.x, enemyPos.x))
        {
            dir = (pos.y >= enemyPos.y) ? Vector2.up : Vector2.down;
        }
        else
        {
            dir = (pos - enemyPos).normalized;
        }
        //Vector2 playerPos = GameManager.instance.player.position;
        //dir = (playerPos - enemyPos).normalized;

        dir = (pos - enemyPos) * 2f;

        _sp.sortingLayerID = SortingLayer.NameToID("frontEb");

        gameObject.SetActive(true);
        StartCoroutine(Wait());
    }
    private IEnumerator Wait()
    {
        yield return new WaitForSeconds(wait);
    }

    private void Update()
    {
        t += tSpeed * Time.deltaTime;
        realT = Mathf.Pow(t, whyGok);
        //realTT = 1f - Mathf.Pow(1f - t, whyGokT);
        realTT = Mathf.Pow(t, whyGokT);


        basePos += (Vector3)(dir * speed * realT * Time.deltaTime);

        float scale = Mathf.LerpUnclamped(startSc, endSc, realTT);
        transform.localScale = Vector2.one * scale;
        zinSu = scale;

        float bojugY = GameManager.instance.playerMove.playerLerpY * GameManager.instance.playerMove.plyYPower * t;
        //float bojugX = GameManager.instance.playerMove.playerLerpX * -1f;

        transform.position = new Vector3(basePos.x, basePos.y + bojugY, 0f);

        _sp.sortingOrder = (int)(t*10f);
        if (t > backLayer)
        {
            _sp.sortingLayerID = SortingLayer.NameToID("backEb");
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Area")) ReturnPool();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag("Player")) return;
        if (t > 1 - hit && t < backLayer)
        {
            UIManager.instance.health.GetDamage(damage);
            GameManager.instance.pung.WelcomToTheHell(transform.position, zinSu * boomSc, boom);
            ReturnPool();
            return;
        }
    }
    private void ReturnPool()
    {
        gameObject.SetActive(false);
        string key = gameObject.name;
        GameManager.instance.ebs.enemyBulletPool[key].Push(gameObject);
    }

}