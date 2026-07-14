using TMPro;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{   
    public static ScoreManager instance;

    public bool stopSc =true;
    public float timer =0f;
    public float cho;

    private int score = 0;
    private int kill = 0;

    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI killText;
    private void Awake()
    {
        instance = this;
    }
    private void Update()
    {
        if (stopSc) return;

        timer += Time.deltaTime;
        if (timer >= cho)
        {
            Score += 1;
            timer -= cho;
        }
    }
    public int Score
    {
        get { return score; }
        set
        {
            score = value;
            UpdateScore();
        }
    }
    public int Kill
    {
        get { return kill; }
        set
        {
            kill = value;
            UpdateKill();
        }
    }
    public void UpdateKill()
    {
        killText.text = "[KILL] " + kill.ToString("D3");
    }
    public void UpdateScore()
    {
        scoreText.text = "[SCORE] " + score.ToString("D6");
    }
}
