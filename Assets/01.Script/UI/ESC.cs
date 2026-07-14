using RetroShadersPro.URP;
using System.Collections;
using UnityEngine;
using UnityEngine.Rendering; // 볼륨 제어 필수
using UnityEngine.UI;

public class PauseManager : MonoBehaviour
{
    public static bool isPaused = false;

    [Header("UI Elements")]
    public GameObject pauseCanvas;
    public Image darkPanel;
    public RectTransform pauseIcon;

    [Header("URP Volume Settings")]
    public Volume globalVolume;

    public GameObject zozunzum;

    // 💡 스크린샷을 토대로 예측한 진짜 컴포넌트 명
    private CRTSettings crt;

    void Start()
    {
        pauseCanvas.SetActive(false);
        if (pauseIcon != null) iconAlpha = pauseIcon.GetComponent<CanvasGroup>();

        // 시작할 때 글로벌 볼륨에서 CRT 컴포넌트를 미리 찾아둡니다.
        if (globalVolume != null)
        {
            globalVolume.profile.TryGet(out crt);
        }
    }

    private CanvasGroup iconAlpha;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape)||Input.GetKeyDown(KeyCode.Space))
        {
            if (isPaused) ResumeGame();
            else PauseGame();
        }
    }

    void PauseGame()
    {   
        isPaused = true;
        Time.timeScale = 0f;
        zozunzum.GetComponent<CoserFollow>().enabled = false;
        GameManager.instance.player.GetComponent<Zanzun>().enabled = false;

        pauseCanvas.SetActive(true);

        // 💡 일시정지 시 스크린샷의 변수 수치 조절
        if (crt != null)
        {
            crt.trackingStrength.value = 5.0f; // 지지직 강하게
            crt.brightness.value = 0.4f;       // 어둡게
        }

        StopAllCoroutines();
        StartCoroutine(PauseAnimation());
    }

    void ResumeGame()
    {
        isPaused = false;
        Time.timeScale = 1.0f;
        zozunzum.GetComponent<CoserFollow>().enabled = true;
        GameManager.instance.player.GetComponent<Zanzun>().enabled = true;


        // 💡 해제 시 스크린샷 2026-07-05 223930.png에 있던 원래 세팅값으로 복구
        if (crt != null)
        {
            crt.trackingStrength.value = 1.0f;  // 원래 값 1
            crt.brightness.value = 1.2f;        // 원래 값 1.2
        }

        pauseCanvas.SetActive(false);
    }

    private IEnumerator PauseAnimation()
    {
        float ti = 0f;
        float duration = 0.1f; // 0.4초에서 0.1초로 단축 (인간이 인지하기에 거의 '즉시'인 속도)

        darkPanel.color = new Color(0f, 0f, 0f, 0f);
        pauseIcon.localScale = Vector3.one * 0.7f; // 시작 크기를 조금 더 키워서 더 빨리 튀어나오는 느낌 부여
        if (iconAlpha != null) iconAlpha.alpha = 1f;

        while (ti < duration)
        {
            ti += Time.unscaledDeltaTime;
            float progress = ti / duration;

            // 화면 어두워짐과 아이콘 커지는 연출을 0.1초 만에 팍! 끝냅니다.
            darkPanel.color = new Color(0f, 0f, 0f, Mathf.Lerp(0f, 0.55f, progress));
            pauseIcon.localScale = Vector3.Lerp(Vector3.one * 0.7f, Vector3.one * 1.5f, progress);
            if (iconAlpha != null) iconAlpha.alpha = Mathf.Lerp(1f, 0f, progress);

            yield return null;
        }
    }
}