using UnityEngine;

public class Zan : MonoBehaviour
{
    public float fadeSpeed = 2.5f;
    private SpriteRenderer _sp;
    private Color _color;

    private void Awake()
    {
        _sp = GetComponent<SpriteRenderer>();
    }

    private void OnEnable()
    {
        _color = _sp.color;
    }

    private void Update()
    {
        _color.a -= fadeSpeed * Time.deltaTime;
        _sp.color = _color;

        if (_color.a <= 0f)
        {
            gameObject.SetActive(false);
        }
    }
}
