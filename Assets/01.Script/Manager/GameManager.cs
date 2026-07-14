using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public Transform player;
    public PlayerMove playerMove;
    public Zanzun zanzun;
    public EnemySpawn enemySpawn;
    public Pung pung;
    public EBS ebs;

    public float minX;
    public float maxX;
    public float minY;
    public float maxY;
    private void Awake()
    {
        instance = this;
    }
}
