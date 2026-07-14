using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class StartGame : MonoBehaviour
{
    public float holdTime = 0f;

    public Image black;

    public CanvasGroup UIGroup; 
    public Slider loadingSlider;   

    public GameObject zozunzum;
    public Zanzun zanzun;

    public GameObject player;
    public Vector3 playerStartPos;
    public Vector3 targetPos;

    public float bakeTime = 0f;
    public float bootTime = 1.5f;

    public float enterUI = 0.5f;
    public float whyGok;
    public float whyGokGok;
    public float playerscale;

    public float music;
    public float titleOn;
    public float titleOff;
    public float aa;
    public float aaa;
    public float maxAlp;
    public TMPro.TMP_Text ti;

    public TextMeshProUGUI guideText;
    private Coroutine blink;
    private bool isClick = false;

    bool canStartHold = false;


    public AudioClip boot;
    public AudioSource bootCl;
    private void Start()
    {
        Time.timeScale = 0f;
        ti.gameObject.SetActive(false);

        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;

        black.gameObject.SetActive(true);
        zozunzum.gameObject.SetActive(false);

        loadingSlider.value = 0f;
        UIGroup.alpha = 0f;

        player.transform.position = playerStartPos;
        player.GetComponent<PlayerMove>().enabled = false;
        player.GetComponent<Zanzun>().enabled = false;

        guideText.gameObject.SetActive(true);
        blink = StartCoroutine(GuideText());

    }

    private IEnumerator GuideText()
    {
        while (true)
        {
            guideText.enabled = !guideText.enabled;
            yield return new WaitForSecondsRealtime(0.5f);
        }
    }

    private IEnumerator FlickerUI()
    {
        guideText.gameObject.SetActive(false); 
        SoundManager.instance.PlaySFX(SoundManager.instance.onUI);
        for (int i = 0; i < 3; i++)
        {
            UIGroup.alpha = 0.8f;
            yield return new WaitForSecondsRealtime(0.05f);
            UIGroup.alpha = 0.1f;
            yield return new WaitForSecondsRealtime(0.05f);
        }
        UIGroup.alpha = 1f;
    }

    private void Update()
    {
        if (!canStartHold)
        {
            if (!Input.GetMouseButton(0))
            {
                canStartHold = true;
            }
            return;
        }

        float reHoldTime = bakeTime + bootTime;

        if (Input.GetMouseButton(0))
        {
            holdTime += Time.unscaledDeltaTime;
            holdTime = Mathf.Min(holdTime, reHoldTime);

            if ( reHoldTime > 0f)
            {
                loadingSlider.value = Mathf.Clamp01(holdTime / reHoldTime);
            }

            if (holdTime <= bakeTime)
            {
                Color c = black.color;
                c.a = 1f;
                black.color = c;

                Time.timeScale = 0f;

                if (holdTime >= enterUI)
                {
                    isClick = true;
                    StopCoroutine(blink);
                    StartCoroutine(FlickerUI());
                }
            }
            else
            {
                if (holdTime > 4f)
                {
                    if (!bootCl.isPlaying)
                    {
                        bootCl.clip = boot;
                        bootCl.Play();
                    }
                }

                float neHoldTime = (holdTime - bakeTime) / bootTime;
                neHoldTime = Mathf.Clamp01(neHoldTime);

                float t = Mathf.Pow(neHoldTime, whyGok);
                Color c = black.color;
                c.a = Mathf.Lerp(1f, 0f, t);
                black.color = c;

                Time.timeScale = neHoldTime;
            }

            if (holdTime >= reHoldTime)
            {
                GameStart();
            }
        }
        else
        {   
            
            if (holdTime > 0f)
            {  
                if (isClick)
                {
                    isClick = false;
                    guideText.gameObject.SetActive(true);
                    blink = StartCoroutine(GuideText());
                }

                holdTime -= Time.unscaledDeltaTime * 2.5f;
                holdTime = Mathf.Max(holdTime, 0f);

                if ( reHoldTime > 0f)
                {
                    loadingSlider.value = Mathf.Clamp01(holdTime / reHoldTime);
                }

                if (holdTime <= bakeTime)
                {
                    Color cc = black.color;
                    cc.a = 1f;
                    black.color = cc;
                    Time.timeScale = 0f;
                }
                else
                {
                    if (bootCl.isPlaying)
                    {
                        bootCl.Stop();
                    }
                    
                    float progress = (holdTime - bakeTime) / bootTime;
                    progress = Mathf.Clamp01(progress);

                    Color c = black.color;
                    c.a = Mathf.Lerp(1f, 0f, progress);
                    black.color = c;

                    Time.timeScale = progress;
                }
            }
        }
    }
    private void GameStart()
    {   

        this.enabled = false;
        black.gameObject.SetActive(false);
        loadingSlider.gameObject.SetActive(false);
        guideText.enabled = false;
        Time.timeScale = 1f;

        StartCoroutine(PlayerWantMove());
        gameObject.GetComponent<Zzinsu>().St();

    }

    private IEnumerator PlayerWantMove()
    {
        float t = 0f;
        zozunzum.SetActive(true);

        while (t < 1f)
        {
            t += Time.deltaTime;
            float realT = 1f - Mathf.Pow(1f - t, whyGokGok);
            player.transform.position = Vector3.Lerp(playerStartPos, targetPos, realT);
            player.transform.localScale = Vector3.Lerp(Vector3.one * playerscale, Vector3.one, realT);
            yield return null;
        }

        Cursor.lockState = CursorLockMode.None;
        player.GetComponent<PlayerMove>().enabled = true;
        player.GetComponent<Zanzun>().enabled = true;
        SoundManager.instance.PlayStageBGM(LevelManager.instance.level);
        StartCoroutine(Title());
    }

    private IEnumerator Title()
    {
        Color c = ti.color;
        c.a = 0f;
        ti.color = c;
        ti.gameObject.SetActive(true);
        float t = 0f;
        yield return new WaitForSeconds(aa);
        while (t < 1f)
        {
            t += Time.deltaTime / titleOn;
            c.a = Mathf.Lerp(0f, maxAlp, 1f - Mathf.Pow(1f - t, 2));
            ti.color = c;
            yield return null;
        }
        yield return new WaitForSeconds(aaa);

        t = 0f;
        while (t < 1f)
        {
            t += Time.deltaTime / titleOff;
            c.a = Mathf.Lerp(0f, maxAlp, Mathf.Pow(1f - t, 2));
            ti.color = c;
            yield return null;
        }

        c.a = 0f;
        ti.color = c;
    }
}