using System.Collections;
using UnityEngine;
using UnityEngine.Rendering;

public class EnemyMove : MonoBehaviour
{
    public enum Status { comming, stop, domag }
    Status status;

    [Header("[ Default ]------------------------------------------------------------")]
    [Space(15f)]
    public float t;
    public float realT;
    [Space(10f)]
    [SerializeField] float tSpeed;
    [SerializeField] float whyGok = 2f;
    [Header("-----------------------------------------------------------------------")]

    [Header("Setting")]
    public EnemyData myData;
    float speed;
    float stop;
    [HideInInspector] public Vector2 movePos;
    [HideInInspector] public Vector2 dir;
    [Space(15f)]
    [SerializeField] float bojugXPower;
    [SerializeField] float bojugYPower;
    Vector2 bojug;
    [SerializeField] float ropi;

    [Header("Domag")]
    [SerializeField] float domagTSpeed;
    [SerializeField] float domagWhyGok;

    [Header("Sprite")]
    [SerializeField] float startSc;
    [SerializeField] float endSc;
    [SerializeField] float maxScale;
    SpriteRenderer _sp;

    [HideInInspector] public float zinSu; //진수는 죽을 때 총알에게 크기전달용
    bool onebun;

    public bool isDead;
    public int attackInstanceId = 0;
    private void Awake()
    {
        _sp = GetComponent<SpriteRenderer>();
    }
    public void Setting()
    {   
        t = 0f;
        realT = 0f;

        dir = new Vector2(Random.Range(-1f, 1f), Random.Range(0f, 1f)).normalized;
        bojug = Vector2.zero;
        transform.localScale = Vector3.one * startSc;
        transform.rotation = Quaternion.identity;
        movePos = transform.position;

        _sp.sprite = myData.front;
        speed = myData.speed;
        stop = Random.Range(myData.stop , myData.stop);

        onebun = true;
        isDead = false;

        status = Status.comming;
    }

    private void Update()
    {
        if (!(status == Status.domag))
        {
            float yPower = 1f + Mathf.Lerp(0, 3, movePos.y);
            //float yPower = 1f + Mathf.InverseLerp(enemyMinY, enemyMaxY, movePos.y) * maxYPower;
            //위에 코드 왜한거임?
            //ㄴ구름마냥 높이 따라서 플레이어 좌우 움직일 때 배율 설정한거임
            float bojugX = GameManager.instance.playerMove.playerLerpX * bojugXPower * yPower;

            movePos.x += -bojugX * Time.deltaTime;
        }

        float bojugY = GameManager.instance.playerMove.playerLerpY * GameManager.instance.playerMove.plyYPower * bojugYPower;
        transform.position = new Vector3(movePos.x, movePos.y + bojugY, 0f);

        switch (status)
        {
            case Status.comming:
                t += tSpeed * Time.deltaTime;
                realT = Mathf.Pow(t, whyGok);

                movePos += dir * speed * realT * Time.deltaTime;

                float scale = Mathf.Lerp(startSc, endSc, t);
                transform.localScale = Vector3.one * scale;

                zinSu = scale;

                if (t > stop)
                {
                    status = Status.stop;
                }
                break;

            case Status.stop:
                
                if (!onebun) break;
                onebun = false;
                SoundManager.instance.EnemyBullet();
                
                GameManager.instance.ebs.WhatEnemy(myData, this);
                break;

            case Status.domag:
                t += domagTSpeed * Time.deltaTime;
                realT = Mathf.Pow(t, domagWhyGok);

                movePos += dir * realT * speed * 2f * Time.deltaTime;

                /*float playerY01 = (GameManager.instance.playerMove.playerLerpY + 1f) * 0.5f;
                float yScalePower = Mathf.Lerp(minYScale, maxYScale, playerY01);*/

                /*domagTT = Mathf.Clamp01(domagTT + domagSpeed * Time.deltaTime);
                realDomagTT = Mathf.Pow(domagTT, domagWhyGok);
                float grow = Mathf.LerpUnclamped(startSc,endSc, realDomagTT);*/

                float banban = Mathf.Lerp(startSc, endSc, stop);
                //float grow = Mathf.Pow(domagT, domagWhyGokScale); //위에 주석처리한거 원래 여따 곱했음.
                //float grow1 = scalePower * Mathf.Pow(t , domagWhyGokScale);
                float grow = Mathf.LerpUnclamped(startSc, endSc/4 , realT);
                float magScale = (banban + grow); 
                magScale = Mathf.Min(magScale, maxScale);
                transform.localScale = Vector3.one * magScale;

                float rota = Mathf.InverseLerp(-5, 5, movePos.x) * 2 - 1f;
                transform.rotation = Quaternion.Euler(0f, 0f, -rota*10);

                zinSu = magScale;

                break;
        }

    }

    public void EndAttack()
    {
        status = Status.domag;
        _sp.sprite = myData.back;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (!collision.CompareTag("Area")) return;
        isDead=true;
        gameObject.SetActive(false);
        GameManager.instance.enemySpawn.enemyPool.Push(gameObject);
    }
}