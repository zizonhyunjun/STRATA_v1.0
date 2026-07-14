using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class Zanzun : MonoBehaviour
{
    
    public Stack<GameObject> bulletPool = new Stack<GameObject>();
    public GameObject bulletPre;
    public int bulletCount;

    private bool canFire = true;
    [SerializeField] private float coolTime;

    [SerializeField] private Transform con;
    [SerializeField] private Transform zo;

    public bool isFire;

    public float currentHeat;  
    public float maxHeat;     
    public float heatPlus;  
    public float coolDownSpeed; 
    public float stopHeat; 
    public bool isStop=false; 

    public float bandog;
    Rigidbody2D rb;
    public Slider heatSlider;
    public Image gag;
    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }
    private void Start()
    {
        Starto();
        heatSlider.minValue = 0f;
        heatSlider.maxValue = maxHeat;
        heatSlider.value = 0f;
    }
    public void OnFire(InputValue value)
    {
        isFire = value.isPressed;
    }
    public void Update()
    {
        if (currentHeat > 0)
        {
            currentHeat -= coolDownSpeed * Time.deltaTime;
        }

        heatSlider.value = currentHeat;
        if (currentHeat <= stopHeat)
        {
            isStop = false;
            
            gag.gameObject.SetActive(false);
        }

        if (!isFire || !canFire || isStop) return;
        
        StartCoroutine(cool());
        Balsa();
        SoundManager.instance.PlayShoot();
        currentHeat += heatPlus;
        if (currentHeat >= maxHeat - 0.1f)
        {
            isStop = true;
            SoundManager.instance.PlaySFX(SoundManager.instance.error);
            gag.gameObject.SetActive(true);
        }
        rb.AddForce(Vector2.down * bandog, ForceMode2D.Impulse);

    }
    public void Balsa()
    {
        
        GameObject bu;

        if (bulletPool.Count > 0)
        {
            bu = bulletPool.Pop();
        }
        else
        {
            bu = Instantiate(bulletPre, con);
            bulletPool.Push(bu);
        }

        bu.GetComponent<Bullet>().Go(zo.position, transform.position);
        bu.SetActive(true);
    }

    private IEnumerator cool()
    {
        canFire = false;
        yield return new WaitForSeconds(coolTime);
        canFire = true;
    }
    public void Starto()
    {
        for (int i = 0; i < bulletCount; i++)
        {
            GameObject bu = Instantiate(bulletPre, con);
            bulletPool.Push(bu);

            if (bu.TryGetComponent<TrailRenderer>(out var trail)) trail.Clear();

            bu.SetActive(false);
        }
    }
}
