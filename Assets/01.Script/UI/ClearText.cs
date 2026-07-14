using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ClearText : MonoBehaviour
{
    public SpriteRenderer managerSpriteRenderer;

    public GameObject letterPrefab;
    public Transform startPos;

    public float letterSpacing = 1.0f;
    public float shiftX = 5f;
    public float lerpSpeed = 4.0f;
    public float delayBetweenLetters = 0.2f;

    public Sprite[] clearManagerSheets;
    public Sprite[] clearSingleLetters;
    public Sprite[] clearSingleFlashes;

    public Sprite[] overManagerSheets;
    public Sprite[] overSingleLetters;
    public Sprite[] overSingleFlashes;

    public GameObject endPanel;

    public int spaceAfterIndex = 3;

    public CanvasGroup panelCanvasGroup;
    public CanvasGroup UICanvasGroup;
    public RectTransform panelRectTransform;
    public float fadeSpeed = 2.0f;
    public float scrollSpeed = 2.0f;
    public Vector2 panelStartPos = new Vector3(0f, 1080f,1f);
    public Vector2 panelTargetPos = new Vector3(0f, 0f,1f);
    public float managerScrollUpY = 3.0f;

    public GameObject zo;
    private SpriteRenderer sp;
    void Awake()
    {
        sp= GetComponent<SpriteRenderer>();
    }

    public void GameClear()
    {
        sp.enabled = true;
        zo.gameObject.SetActive(false);
        managerSpriteRenderer.gameObject.SetActive(true);
        managerSpriteRenderer.sprite = null;
        StartCoroutine(AnimateSequence(clearManagerSheets, clearSingleLetters, clearSingleFlashes));
    }

    public void GameOver()
    {
        sp.enabled = true;
        zo.gameObject.SetActive(false);

        managerSpriteRenderer.gameObject.SetActive(true);
        managerSpriteRenderer.sprite = null;
        StartCoroutine(AnimateSequence(overManagerSheets, overSingleLetters, overSingleFlashes));
    }

    IEnumerator AnimateSequence(Sprite[] managerSheets, Sprite[] singleLetters, Sprite[] singleFlashes)
    {   
        
        Time.timeScale = 0f;

        //Vector2 startPos = new Vector2(transform.position.x + 12f, transform.position.y);
        float minusX = 0f;

        for (int i = 0; i < singleLetters.Length; i++)
        {
            GameObject newLetter = Instantiate(letterPrefab, startPos);
            SpriteRenderer sr = newLetter.GetComponent<SpriteRenderer>();

            sr.sprite = singleLetters[i];

            Vector3 targetPos = transform.position + new Vector3(shiftX - minusX, 0f, 1f);
            minusX += letterSpacing;

            if (i == spaceAfterIndex)
            {
                minusX += letterSpacing;
            }

            yield return StartCoroutine(IndividualMove(newLetter, sr, targetPos, singleFlashes[i], managerSheets[i]));

            yield return new WaitForSecondsRealtime(delayBetweenLetters);
        }

        yield return StartCoroutine(FadeAndScrollPanel());
    }

    IEnumerator IndividualMove(GameObject obj, SpriteRenderer sr, Vector3 target, Sprite flashSprite, Sprite nextManagerSheet)
    {
        float t = 0f;
        Vector3 startPos = obj.transform.position;

        while (t < 1f)
        {
            t += Time.unscaledDeltaTime * lerpSpeed;
            float curve = 1f - Mathf.Pow(1f - t, 3);
            obj.transform.position = Vector3.Lerp(startPos, target, curve);
            yield return null;
        }
        obj.transform.position = target;

        sr.sprite = flashSprite;
        yield return new WaitForSecondsRealtime(0.12f);

        managerSpriteRenderer.sprite = nextManagerSheet;

        obj.SetActive(false);
    }
    IEnumerator FadeAndScrollPanel()
    {
        endPanel.SetActive(true);
        panelCanvasGroup.alpha = 0f;
        UICanvasGroup.alpha = 0f;
        panelRectTransform.anchoredPosition = panelStartPos;

        float t = 0f;
        while (t < 1f)
        {
            t += Time.unscaledDeltaTime * fadeSpeed;
            panelCanvasGroup.alpha = Mathf.Lerp(0f, 1f, t);
            yield return null;
        }
        panelCanvasGroup.alpha = 1f;

        yield return new WaitForSecondsRealtime(0.3f);

        Vector3 managerStartPos = managerSpriteRenderer.transform.position;
        Vector3 managerTargetPos = managerStartPos + new Vector3(0f, managerScrollUpY, 0f);

        t = 0f;
        while (t < 1f)
        {
            t += Time.unscaledDeltaTime * scrollSpeed;
            float curve = 1f - Mathf.Pow(1f - t, 3);

            panelRectTransform.anchoredPosition = Vector2.Lerp(panelStartPos, panelTargetPos, curve);
            managerSpriteRenderer.transform.position = Vector3.Lerp(managerStartPos, managerTargetPos, curve);

            yield return null;
        }
        panelRectTransform.anchoredPosition = panelTargetPos;
        managerSpriteRenderer.transform.position = managerTargetPos;

        Cursor.visible = true;

        Cursor.lockState = CursorLockMode.None;
    }

    public void RestartGame()
    {   
        Debug.Log("restart");
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void QuitGame()
    {   
        Debug.Log("quit");
        
        Application.Quit();
    }
}