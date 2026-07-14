using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering.Universal;

public class PlayerMove : MonoBehaviour
{
    [Header("Keyboard")]
    public float playerSpeed;
    private Rigidbody2D _rigid;
    public Vector2 _moveDir;

    [Header("Mouse")]
    [SerializeField] private float mouseSpeed;

    [SerializeField] private float followSpeedX;
    [SerializeField] private float followSpeedY;

    [SerializeField] private float minSmoothTime;
    [SerializeField] private float maxSmoothTime;

    public float milDang = 5f;
    public float cutLine;

    private Vector2 currentVelocity;

    [Header("else")]
    public float smallLine;
    [SerializeField] private float lastSmall;

    public float playerLerpX;
    public float playerLerpY;

    float minX;
    float maxX;
    float minY;
    float maxY;

    public float plyYPower;

    SpriteRenderer sp;
    public Sprite[] spriteDe;
    public int spCount = 0;
    public bool boolSp = true;
    public bool boolSpSp = true;

    public float roPower;

    public bool dash = false;

    private void Awake()
    {
        _rigid = GetComponent<Rigidbody2D>();
        sp=GetComponent<SpriteRenderer>();
    }
    private void Start()
    {
        minX = GameManager.instance.minX;
        maxX = GameManager.instance.maxX;
        minY = GameManager.instance.minY;
        maxY = GameManager.instance.maxY;

        StartCoroutine(BoosterBlinkRoutine());
    }

    private void Update()
    {
        Vector2 m = Mouse.current.position.ReadValue();
        Vector2 worldM = Camera.main.ScreenToWorldPoint(m);
        Vector2 player = transform.position;

        Vector2 guri = worldM - player;
        float gari = Mathf.Max(50, guri.magnitude);
        float giri = Mathf.InverseLerp(0f, 50f, gari);
        //float smoothTime = Mathf.Lerp(minSmoothTime, maxSmoothTime, gari);

        float smoothTime = dash ? 0.01f : Mathf.Lerp(minSmoothTime, maxSmoothTime, gari);

        Vector2 final = worldM;
        final.x = Mathf.Clamp(final.x * followSpeedX, minX, maxX);
        final.y = Mathf.Clamp(final.y * followSpeedY - cutLine, minY - (cutLine * 0.5f), maxY);

        transform.position = Vector2.SmoothDamp(player, final, ref currentVelocity, smoothTime);

        float small = Mathf.InverseLerp(minX, smallLine, transform.position.y);
        float currentScale = Mathf.Lerp(lastSmall, 1, small);
        transform.localScale = Vector3.one * currentScale;

        playerLerpX = Mathf.InverseLerp(minX, maxX, transform.position.x);
        playerLerpX = playerLerpX * 2f - 1f;
        playerLerpY = Mathf.InverseLerp(minY, maxY, transform.position.y);
        playerLerpY = playerLerpY * 2f - 1f;

        transform.rotation = Quaternion.Euler(0f, 0f, playerLerpX * roPower);

        if (playerLerpX < -0.5f)
        {
            spCount = 0;
        }
        else if (playerLerpX > 0.5)
        {
            spCount = 6;
        }
        else
        {
            spCount = 3;
        }
        if (!boolSp)
        {
            spCount += 1;
            if (!boolSpSp)
            {
                spCount += 1;
            }
        }

        sp.sprite = spriteDe[spCount];

    }

    IEnumerator BoosterBlinkRoutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(0.1f);
            boolSp = !boolSp;
            yield return new WaitForSeconds(0.1f);
            boolSpSp = !boolSpSp;
        }
    }

    public void OnMove(InputValue value)
    {
        _moveDir = value.Get<Vector2>();
    }
}
