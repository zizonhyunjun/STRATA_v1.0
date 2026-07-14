using PSXShadersPro.URP.Demo;
using UnityEngine;
using UnityEngine.Rendering;

public class Bullet : MonoBehaviour
{
    [Header("[ Default ]------------------------------------------------------------")]
    [Space(15f)]
    public float t;
    public float realT;
    [Space(10f)]
    [SerializeField] float tSpeed;
    [SerializeField] float whyGok = 2f;
    [Header("-----------------------------------------------------------------------")]

    [SerializeField] private float speed;
    private Vector2 dir;
    float speedDir;

    [SerializeField] float startSc;
    [SerializeField] float endSc;

    public float bummi; //뭔가했는데 범위였네ㅋㅋ
    public float hitBummiZozul;
    [SerializeField] float yyyy; //y값이 조준점보다 높다는 피드백 반영

    public GameObject boom;
    SpriteRenderer _sp;

    TrailRenderer _tr;
    [SerializeField] float trailLife = 0.25f;
    [SerializeField] float trailStartWidth = 1.2f;

    private void Awake()
    {
        _sp = GetComponent<SpriteRenderer>();
        _tr = GetComponent<TrailRenderer>();
    }
    public void Go(Vector2 zo, Vector2 pl)
    {   
        t = 0f;
        realT = 0f;

        dir = (zo-pl).normalized;
        dir.x = dir.x + Random.Range(-0.03f, 0.03f);
        dir.y = dir.y * yyyy;

        speedDir = (zo - pl).magnitude * speed;
        Vector2 start = Vector2.Lerp(pl, zo, 0.8f);
        transform.position = start;
        transform.localScale = Vector3.one * startSc;

        _tr.Clear();
        _tr.emitting = true;
        _tr.widthMultiplier = trailStartWidth;
    }


    private void Update()
    {
        t += tSpeed * Time.deltaTime;
        realT = Mathf.Pow(t, whyGok);

        float newT= 1f - Mathf.Pow(1f - t, whyGok);
        float reversePure = Mathf.Lerp(t, newT, 0.5f);
        float scale = Mathf.Lerp(startSc, endSc, reversePure);
        transform.localScale = Vector3.one * scale;

        _tr.widthMultiplier = Mathf.Lerp(trailStartWidth, 0f, reversePure);
        _tr.time = Mathf.Lerp(trailStartWidth, 0f, reversePure);
        if (t >= trailLife)
        {
            _tr.emitting = false;
        }

        float realSpeed = Mathf.Lerp(speedDir, -speedDir * 0.2f, newT);

        transform.position += (Vector3)(dir * realSpeed * Time.deltaTime);

        if (t >= 1f)
        {
            gameObject.SetActive(false);
            GameManager.instance.zanzun.bulletPool.Push(gameObject);
        }

        _sp.sortingOrder = (int)(19f - (t*10));
        _tr.sortingOrder = _sp.sortingOrder - 1;
    }


    private void OnTriggerStay2D(Collider2D collision)
    {
        if (!collision.gameObject.CompareTag("Enemy")) return;

        float kimininamaewa = collision.gameObject.GetComponent<EnemyMove>().t;
        if (!((kimininamaewa < (1 - t) + bummi*2f) && (kimininamaewa > (1 - t) - bummi))) return;

        //Debug.Log(kimininamaewa);
        SoundManager.instance.EnemyBoom();
        collision.gameObject.GetComponent<EnemyMove>().isDead = true;
        float zzinSu = collision.gameObject.GetComponent<EnemyMove>().zinSu;
        GameManager.instance.pung.WelcomToTheHell(collision.gameObject.GetComponent<EnemyMove>().movePos, zzinSu,boom);

        collision.gameObject.SetActive(false);
        GameManager.instance.enemySpawn.enemyPool.Push(collision.gameObject);
        gameObject.SetActive(false);
        GameManager.instance.zanzun.bulletPool.Push(gameObject);

        ScoreManager.instance.Kill += 1;
    }
}