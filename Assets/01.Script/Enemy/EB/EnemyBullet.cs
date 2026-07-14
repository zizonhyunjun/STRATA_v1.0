using UnityEngine;

public class EnemyBullet : MonoBehaviour
{
    [Header("[ Default ]------------------------------------------------------------")]
    [Space(15f)]
    public float t;
    public float realT;
    [Space(10f)]
    [SerializeField] float tSpeed;
    [SerializeField] float whyGok = 2f;
    [Header("-----------------------------------------------------------------------")]

    [SerializeField] float speed;
    Vector2 dir;

    [SerializeField] float startSc;
    [SerializeField] float endSc;

    public int damage;
    [SerializeField] float hit = 0.1f;

    public GameObject boom;

    private Vector3 basePos;
    private float zinSu;

    public void Go(Vector2 enemyPos)
    {   
        t = 0f;
        realT = 0f;

        transform.position = enemyPos;
        transform.localScale = Vector3.one * startSc;

        Vector2 playerPos = GameManager.instance.player.position;
        dir = (playerPos - enemyPos).normalized;

        /*        Vector2 realGuri = (playerPos - enemyPos);
                guriY = Mathf.InverseLerp(GameManager.instance.maxY, GameManager.instance.minY, realGuri.y);
                guriY = Mathf.InverseLerp(GameManager.instance.maxX, GameManager.instance.minX, realGuri.x);
                guri=new Vector2(guriY, guriX);*/

        basePos = transform.position;

        gameObject.SetActive(true);
    }

    private void Update()
    {
        t += tSpeed * Time.deltaTime;
        realT = Mathf.Pow(t, whyGok);
        
        basePos += (Vector3)(dir * speed * realT * Time.deltaTime);

        float scale = Mathf.LerpUnclamped(startSc, endSc, realT);
        transform.localScale = Vector2.one * scale;
        zinSu = scale;

        float bojugY = GameManager.instance.playerMove.playerLerpY * GameManager.instance.playerMove.plyYPower * t;
        //float bojugX = GameManager.instance.playerMove.playerLerpX * -1f;

        transform.position = new Vector3(basePos.x, basePos.y + bojugY, 0f);
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Area")) ReturnPool();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag("Player")) return;
        //float cha = Mathf.Abs(1 - t);
        if (t>1-hit)
        {
            UIManager.instance.health.GetDamage(damage);
            GameManager.instance.pung.WelcomToTheHell(transform.position,zinSu, boom);
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