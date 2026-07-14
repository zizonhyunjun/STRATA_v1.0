using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class EBS : MonoBehaviour
{   
    public Dictionary<string, Stack<GameObject>> enemyBulletPool = new Dictionary<string, Stack<GameObject>>();

    float maxX;
    float minX;
    private void Start()
    {
        maxX = GameManager.instance.maxX; 
        minX = GameManager.instance.minX;
    }
    public void WhatEnemy(EnemyData enemyData, EnemyMove enemyMove)
    {

        switch (enemyData.number)
        {
            case 1:
            case 11:
            case 13:
                StartCoroutine(Attack_1(enemyData, enemyMove));
                break;
            case 2:
            case 12:
                StartCoroutine(Attack_2(enemyData, enemyMove,0.2f));
                break;
            case 3:
                StartCoroutine(Attack_3(enemyData, enemyMove));
                break;
            case 4:
            case 16:
                StartCoroutine(Attack_4(enemyData, enemyMove,enemyData.number/4));
                break;
            case 5:
            case 17:
                StartCoroutine(Attack_5(enemyData, enemyMove));
                break;
            case 6:
                StartCoroutine(Attack_6(enemyData, enemyMove));
                break;
            case 7:
                StartCoroutine(Attack_7(enemyData, enemyMove));
                break;
            case 8:
            case 14:
                StartCoroutine(Attack_8(enemyData, enemyMove));
                break;
            case 15:
                StartCoroutine(Attack_15(enemyData, enemyMove));
                break;
            case 18:
                StartCoroutine(Attack_18(enemyData, enemyMove));
                break;
            case 19:
                StartCoroutine(Attack_19(enemyData, enemyMove));
                break;
            case 10:
                StartCoroutine(Attack_10(enemyData, enemyMove));
                break;
                /*case 9:
                    StartCoroutine(Attack_9(enemyData, enemyMove));
                    break;*/

        }
    }
    private IEnumerator Attack_10(EnemyData myData, EnemyMove enemyMove)
    {
        float timer = 0f;
        while (timer < myData.delay)
        {
            timer += Time.deltaTime;

            // 기다리는 도중 "단 1프레임"이라도 죽거나 풀에 들어가면 그 즉시 코루틴 파괴!
            if (enemyMove == null || enemyMove.isDead || !enemyMove.gameObject.activeInHierarchy)
                yield break;

            yield return null;
        }

        // 씬에 살아있는 진짜 내 10번 적 위치에서만 발사
        IWantBulletAndImEnemy(enemyMove.transform.position, Vector2.one, myData.bullet);

        yield return new WaitForSeconds(myData.whoDelay);

        // 끝날 때도 안전할 때만 엔드 처리
        if (enemyMove != null && !enemyMove.isDead)
        {
            enemyMove.EndAttack();
        }
    }
    private IEnumerator Attack_1(EnemyData myData, EnemyMove enemyMove)
    {
        for (int i = 0; i < myData.balsaSu; i++)
        {   

            yield return new WaitForSeconds(myData.delay);
            if (enemyMove.isDead) yield break;

            IWantBulletAndImEnemy(enemyMove.transform.position,Vector2.one, myData.bullet);
        }
        yield return new WaitForSeconds(myData.whoDelay);

        enemyMove.EndAttack();
    }
    private IEnumerator Attack_2(EnemyData myData, EnemyMove enemyMove,float k)
    {
        for (int i = 0; i < myData.balsaSu; i++)
        {
            yield return new WaitForSeconds(myData.delay);
            if (enemyMove.isDead) yield break;

            IWantBulletAndImEnemy(new Vector2(enemyMove.transform.position.x, enemyMove.transform.position.y - k), enemyMove.transform.position, myData.bullet);
        }
        yield return new WaitForSeconds(myData.whoDelay);

        enemyMove.EndAttack();
    }
    private IEnumerator Attack_3(EnemyData myData, EnemyMove enemyMove)
    {
        for (int i = 0; i < myData.balsaSu; i++)
        {
            yield return new WaitForSeconds(myData.delay);
            IWantBulletAndImEnemy(new Vector2(enemyMove.transform.position.x + 0.5f, enemyMove.transform.position.y), Vector2.one, myData.bullet);
            IWantBulletAndImEnemy(new Vector2(enemyMove.transform.position.x - 0.5f, enemyMove.transform.position.y), Vector2.one, myData.bullet);
        }
        yield return new WaitForSeconds(myData.whoDelay);

        enemyMove.EndAttack();
    }
    private IEnumerator Attack_4(EnemyData myData, EnemyMove enemyMove,int kk)
    {
        Vector2 en = new Vector2(enemyMove.transform.position.x, enemyMove.transform.position.y); 
        float an = 0f; 
        float guri = 0.2f; 
        for (int k=0; k < kk; k++)
        {
            yield return new WaitForSeconds(myData.delay);
            for (int i = 0; i < myData.balsaSu; i++)
            {
                Quaternion ro = Quaternion.Euler(0, 0, an + (15f * k));
                Vector2 di = ro * Vector3.up;
                Vector3 mypos = en + (di * guri);
                IWantBulletAndImEnemy(en, mypos, myData.bullet);

                an += 360f / myData.balsaSu;
            }  
        }
        yield return new WaitForSeconds(myData.whoDelay);

        enemyMove.EndAttack();
    }
    private IEnumerator Attack_5(EnemyData myData, EnemyMove enemyMove)
    {
        float an = Random.Range(0f, 360f);
        float guri = 0.1f;

        for (int i = 0; i < myData.balsaSu; i++)
        {
            yield return new WaitForSeconds(myData.delay);
            Vector2 en = new Vector2(enemyMove.transform.position.x, enemyMove.transform.position.y);

            Quaternion ro = Quaternion.Euler(0, 0, an);
            Vector2 di = ro * Vector3.up;
            Vector3 mypos = en + (di * guri);
            if (enemyMove.isDead) yield break;

            IWantBulletAndImEnemy(en, mypos, myData.bullet);

            an += 360f / myData.balsaSu;
        }
        yield return new WaitForSeconds(myData.whoDelay);

        enemyMove.EndAttack();
    }
    private IEnumerator Attack_6(EnemyData myData, EnemyMove enemyMove)
    {
        float an = Random.Range(0f, 360f);
        float guri = 0.2f;

        for (int i = 0; i < myData.balsaSu; i++)
        {   
            yield return new WaitForSeconds(myData.delay);
            Vector2 en = new Vector2(enemyMove.transform.position.x, enemyMove.transform.position.y);

            Quaternion ro = Quaternion.Euler(0, 0, an);
            Vector2 di = ro * Vector3.up;
            Vector3 mypos = en + (di * guri * (myData.balsaSu - i));
            if (enemyMove.isDead) yield break;

            IWantBulletAndImEnemy(en, mypos, myData.bullet);
        }
        yield return new WaitForSeconds(myData.whoDelay);

        enemyMove.EndAttack();
    }
    private IEnumerator Attack_7(EnemyData myData, EnemyMove enemyMove)
    {
        Vector2 en = new Vector2(enemyMove.transform.position.x, enemyMove.transform.position.y);
        float[] angles = { 0f, 90f, 180f, 270f };
        float gan = 0.15f;
        for (int i = 0; i < myData.balsaSu; i++)
        {
            foreach (float angle in angles)
            {
                yield return new WaitForSeconds(myData.delay);
                Quaternion ro = Quaternion.Euler(0, 0, angle);
                Vector2 di = ro * Vector3.up;
                Vector3 mypos = en + (di * (gan * (i + 1)));
                if (enemyMove.isDead) yield break;

                IWantBulletAndImEnemy(en, mypos, myData.bullet);
            }
        }
        yield return new WaitForSeconds(myData.whoDelay);



        enemyMove.EndAttack();
    }
    
    private IEnumerator Attack_8(EnemyData myData, EnemyMove enemyMove)
    {
        Vector2 en = new Vector2(enemyMove.transform.position.x, enemyMove.transform.position.y);
        float maxRadius = 0.4f;

        for (int i = 0; i < myData.balsaSu; i++)
        {   

            Vector2 randomInCircle = Random.insideUnitCircle * maxRadius;
            Vector3 mypos = en + randomInCircle;
            if (enemyMove.isDead) yield break;

            IWantBulletAndImEnemy(en, mypos, myData.bullet);
        }
        yield return new WaitForSeconds(myData.whoDelay);

        enemyMove.EndAttack();
    }
    private IEnumerator Attack_15(EnemyData myData, EnemyMove enemyMove)
    {
        Vector2 en = new Vector2(enemyMove.transform.position.x, enemyMove.transform.position.y);
        float[] angles = { 0f, 60f, 120f, 180f, 240f, 300f };
        float gan = 0.15f;
        
        for(int k=0; k < 3; k++)
        {
            for (int i = 0; i < myData.balsaSu; i++)
            {
                foreach (float angle in angles)
                {
                    yield return new WaitForSeconds(myData.delay);
                    Quaternion ro = Quaternion.Euler(0, 0, angle + (20f * k));
                    Vector2 di = ro * Vector3.up;
                    Vector3 mypos = en + (di * (gan * (i + 1f)));

                    if (enemyMove.isDead) yield break;
                    IWantBulletAndImEnemy(en, mypos, myData.bullet);
                }
            }
            yield return new WaitForSeconds(myData.whoDelay);
        }

        enemyMove.EndAttack();
    }
    private IEnumerator Attack_18(EnemyData myData, EnemyMove enemyMove)
    {
        Vector2 en = new Vector2(enemyMove.transform.position.x, enemyMove.transform.position.y);
        float halfSize = 0.2f;
        int emptySide = Random.Range(0, 4);

        yield return new WaitForSeconds(myData.delay);

        int bulletsPerSide = myData.balsaSu / 4;

        for (int i = 0; i < bulletsPerSide; i++)
        {   

            float ratio = (bulletsPerSide > 1) ? (float)i / (bulletsPerSide - 1) : 0.5f;
            float offset = Mathf.Lerp(-halfSize, halfSize, ratio);

            Vector3 topPos = new Vector3(en.x + offset, en.y + halfSize, 0f);
            Vector3 bottomPos = new Vector3(en.x + offset, en.y - halfSize, 0f);
            Vector3 leftPos = new Vector3(en.x - halfSize, en.y + offset, 0f);
            Vector3 rightPos = new Vector3(en.x + halfSize, en.y + offset, 0f);

            
            if (emptySide != 0) IWantBulletAndImEnemy(en, topPos, myData.bullet);
            if (emptySide != 1) IWantBulletAndImEnemy(en, bottomPos, myData.bullet);
            if (emptySide != 2) IWantBulletAndImEnemy(en, leftPos, myData.bullet);
            if (emptySide != 3) IWantBulletAndImEnemy(en, rightPos, myData.bullet);
        }

        yield return new WaitForSeconds(myData.whoDelay);
        enemyMove.EndAttack();
    }
    private IEnumerator Attack_19(EnemyData myData, EnemyMove enemyMove)
    {
        Vector2 en = new Vector2(enemyMove.transform.position.x, enemyMove.transform.position.y);
        int lineCount = 5;
        float lineInterval = 0.05f; 
        float halfSize = 0.3f;   

        bool isLeftToRight = Random.value > 0.5f;
        for (int j = 0; j < lineCount; j++)
        {
            int step = isLeftToRight ? j : (lineCount - 1 - j);
            float lineX = en.x + (step - (lineCount - 1) / 2f) * lineInterval;
            for (int i = 0; i < myData.balsaSu; i++)
            {
                float ratio = (float)i / (myData.balsaSu - 1);
                float offsetY = Mathf.Lerp(-halfSize, halfSize, ratio);
                Vector3 mypos = new Vector3(lineX, en.y + offsetY, 0f);
                IWantBulletAndImEnemy(en, mypos, myData.bullet);
            }
            yield return new WaitForSeconds(myData.delay);
        }
        enemyMove.EndAttack();
    }
    public void IWantBulletAndImEnemy(Vector2 pos,Vector2 mypos, GameObject bullet) 
    {
        string key = bullet.name;

        if (!enemyBulletPool.ContainsKey(key)) { enemyBulletPool.Add(key, new Stack<GameObject>()); }
        
        GameObject bu;
        if (enemyBulletPool[key].Count > 0)
        {
            GameObject popbu = enemyBulletPool[key].Pop();
            if (popbu.activeInHierarchy)
            {
                bu = Instantiate(popbu, transform);
                bu.name = key;
            }
            else
            {
                bu = popbu;
            }
        }
        else
        {
            bu = Instantiate(bullet, transform);
            bu.name = key;
        }
        if(bu.TryGetComponent(out IEb interfaceEb))
        {
            interfaceEb.Go(pos,mypos);
        }
    }

}