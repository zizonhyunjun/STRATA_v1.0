using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Health : MonoBehaviour
{
    public int health;
    Image[] healthPoint;
    [SerializeField] Image healthPre;
    [SerializeField] private int maxHealth;

    [SerializeField] CameraFollow ca;
    [SerializeField] Image[] chance;
    public int count = 3;
    [SerializeField] Sprite x;

    public bool muGuk = false;
    public float muGukTime;

    public GameObject hyunkyu;
    private void Start()
    {
        health = maxHealth;
        healthPoint = new Image[health];
        for (int i = 0; i < health; i++)
        {
            healthPoint[i] = Instantiate(healthPre,transform);
        }
    }

    public void GetDamage(int damage)
    {
        if (muGuk) return;
        health -= damage;
        Mathf.Clamp(health, 0, maxHealth);
        UpdateHealthUI(health);
        SoundManager.instance.PlaySFX(SoundManager.instance.playerHit);
        GameManager.instance.player.GetComponent<PlayerShader>().A();
        StartCoroutine(THEWORLD());
        ca.GetComponent<CameraFollow>().Shake(0.01f * damage, 0.7f * damage);

        if (health <= 0)
        {
            Debug.Log("Dead");
            count--;
            chance[count].sprite = x;
            SoundManager.instance.PlaySFX(SoundManager.instance.playerDead);
            
            bool end = count == 0 ? true : false;
            GameManager.instance.player.GetComponent<PlayerDeathEffect>().OnDeath(end);
        }

        
    }
    public void Re()
    {   
        SoundManager.instance.PlaySFX(SoundManager.instance.respawn);
        StartCoroutine(Hyun());
        health = maxHealth;
        for (int i = 0; i < health; i++)
        {
            healthPoint[i] = Instantiate(healthPre, transform);
        }
    }
    private IEnumerator Hyun()
    {
        hyunkyu.GetComponent<TextMeshProUGUI>().text = $"GET READY x{count}";
        bool a=true;
        for (int i = 0; i < 6; i++)
        {
            hyunkyu.SetActive(a);
            yield return new WaitForSeconds(0.3f);
            a = !a;
        }
    }
    public void UpdateHealthUI(int life)
    {   
        for (int i = 0; i < maxHealth; i++)
        {
            if (i < life)
                healthPoint[i].gameObject.SetActive(true);
            else
                StartCoroutine(NeGaCoRuTinUolSSuYaGetNea(healthPoint[i]));
        }
    }
    private IEnumerator NeGaCoRuTinUolSSuYaGetNea(Image po)
    {
        //if (!po.gameObject.activeSelf) yield break;
        float a = 0f;
        Vector2 sc = new Vector2(po.gameObject.transform.localScale.x * 1.5f, po.gameObject.transform.localScale.y);
        while (true)
        {
            a = Mathf.Clamp01(a + Time.deltaTime);

            Color co = po.color;
            co.a = Mathf.Pow(1f - a, 2);
            po.color = co;

            float t = 1 - Mathf.Pow(1 - a, 5);
            float tt = Mathf.Pow(1 - a, 5);
            po.gameObject.transform.localScale = new Vector2(sc.x * (tt +0.4f), sc.y * (t + 1));
            yield return null;
            if (a >= 1)
            {
                po.gameObject.SetActive(false);
            }
        }
    }
    private IEnumerator THEWORLD()
    {
        muGuk = true;
        yield return new WaitForSeconds(muGukTime);
        muGuk = false;
    }

}