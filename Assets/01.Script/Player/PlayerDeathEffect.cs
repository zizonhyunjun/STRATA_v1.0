using UnityEngine;
using System.Collections;

public class PlayerDeathEffect : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;
    private Collider2D playerCollider;

    public float del = 1.0f;
    public float timeTime = 0.1f;

    public float respawnDelay = 2.0f;

    public GameObject zozunzum;
    public float whyGokGok = 3f;
    public float playerscale = 0.2f;
    public Vector3 playerStartPos;
    public Vector3 targetPos;

    public Health health;

    bool end = false;

    void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        playerCollider = GetComponent<Collider2D>();
    }

    public void OnDeath(bool e)
    {
        end = e;
        playerCollider.enabled = false;
        UIManager.instance.health.muGuk = true;
        
        GetComponent<PlayerMove>().enabled = false;
        GetComponent<Zanzun>().enabled = false;

        StartCoroutine(Blink());
    }

    IEnumerator Blink()
    {
        float blTIme = 0f;
        while (blTIme < del)
        {
            spriteRenderer.enabled = !spriteRenderer.enabled;
            yield return new WaitForSecondsRealtime(timeTime);
            blTIme += timeTime;
        }
        spriteRenderer.enabled = false;

        if (!end)
        {
            yield return new WaitForSecondsRealtime(respawnDelay);
            health.Re();
            StartCoroutine(PlayerWantMove());
        }
        else
        {
            yield return new WaitForSecondsRealtime(respawnDelay);

            UIManager.instance.clearText.GameOver();
            yield return null;
        }
    }

    private IEnumerator PlayerWantMove()
    {
        transform.position = playerStartPos;
        transform.localScale = Vector3.one * playerscale;
        spriteRenderer.enabled = true;

        playerCollider.enabled = true;

        float t = 0f;
        zozunzum.SetActive(true);

        while (t < 1f)
        {
            t += Time.deltaTime;
            float realT = 1f - Mathf.Pow(1f - t, whyGokGok);
            transform.position = Vector3.Lerp(playerStartPos, targetPos, realT);
            transform.localScale = Vector3.Lerp(Vector3.one * playerscale, Vector3.one, realT);
            yield return null;
        }

        Cursor.lockState = CursorLockMode.None;
        GetComponent<PlayerMove>().enabled = true;
        GetComponent<Zanzun>().enabled = true;

        StartCoroutine(KK());
    }

    private IEnumerator KK()
    {   

        yield return new WaitForSeconds(4f);
        UIManager.instance.health.muGuk = false;
        
    }
}