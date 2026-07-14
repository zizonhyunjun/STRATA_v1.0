using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.U2D;

public class ShiftShift : MonoBehaviour
{
    public float shitime = 0.2f;
    public float shiclonetime = 0.03f;
    public float shicool = 1.5f;
    public float checkRadius = 3f;
    public LayerMask bulletLayer;
    public GameObject ghostPrefab;
    public Transform radarIndicator;

    private List<GameObject> ghostPool = new List<GameObject>();
    private bool sh = false;
    SpriteRenderer sp;
    private void Awake()
    {
         sp = GetComponent<SpriteRenderer>();
    }
    private void Start()
    {
        float diameter = checkRadius * 2f;
        radarIndicator.localScale = new Vector3(diameter, diameter, 1f);
    }
    private void Update()
    {
        if ((Input.GetMouseButtonDown(1) || Keyboard.current.shiftKey.wasPressedThisFrame) && !sh)
        {
            sh = true;
            Time.timeScale = 0.5f;

            StartCoroutine(ShiftC());
        }
    }


    private IEnumerator ShiftC()
    {
        Collider2D[] hitColliders = Physics2D.OverlapCircleAll(transform.position, checkRadius, bulletLayer);
        foreach (var col in hitColliders)
        {
            if (col.TryGetComponent<IE>(out var bullet) && bullet.MyT())
            {
                Debug.Log("san");
                Time.timeScale = 0.1f;

                StartCoroutine(RestoreTimeScale(col.transform));
                break;
            }
        }
        gameObject.GetComponent<PlayerMove>().dash = true;

        float ti = 0f, spawnTi = 0f;

        while (ti < shitime)
        {
            ti += Time.deltaTime;
            spawnTi += Time.deltaTime;

            if (spawnTi >= shiclonetime)
            {
                SpawnGhostFromPool();
                spawnTi = 0f;
            }
            yield return null;
        }

        gameObject.GetComponent<PlayerMove>().dash = false;
        Time.timeScale = 1.0f;
        StartCoroutine(ShiftDelay());
    }

    private IEnumerator RestoreTimeScale(Transform targetBullet)
    {
        while (targetBullet != null && Vector2.Distance(transform.position, targetBullet.position) <= checkRadius)
        {
            yield return new WaitForSecondsRealtime(0.1f);
        }
        Time.timeScale = 1.0f;
    }

    private void SpawnGhostFromPool()
    {
        GameObject ghost = null;
        for (int i = 0; i < ghostPool.Count; i++)
        {
            if (!ghostPool[i].activeSelf)
            {
                ghost = ghostPool[i];
                break;
            }
        }

        if (ghost == null)
        {
            ghost = Instantiate(ghostPrefab);
            ghostPool.Add(ghost);
        }

        ghost.transform.position = transform.position;
        ghost.transform.rotation = transform.rotation;

        SpriteRenderer ghostSR = ghost.GetComponent<SpriteRenderer>();
        ghostSR.sprite = sp.sprite;
        ghostSR.flipX = sp.flipX;

        Color c = sp.color;
        c.a = 0.4f;
        ghostSR.color = c;

        ghost.SetActive(true);
    }

    private IEnumerator ShiftDelay()
    {
        yield return new WaitForSecondsRealtime(shicool);
        sh = false;

        StartCoroutine(FlashRoutine());
    }
    private IEnumerator FlashRoutine()
    {
        Color originalColor = sp.color;
        float currentAlpha = originalColor.a;

        sp.color = new Color(1f, 1f, 1f, currentAlpha);
        yield return new WaitForSecondsRealtime(0.1f);

        float ti = 0f;
        while (ti < 0.15f)
        {
            ti += Time.unscaledDeltaTime;
            sp.color = Color.Lerp(new Color(1f, 1f, 1f, currentAlpha), originalColor, ti / 0.15f);
            yield return null;
        }

        sp.color = originalColor;
    }

}
