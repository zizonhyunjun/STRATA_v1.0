using UnityEngine;
[CreateAssetMenu(fileName = "EnemySO", menuName ="ScriptableObject/EnemyData")]
public class EnemyData: ScriptableObject
{
    public string nname;
    public int number;

    public int speed;
    public float stop;

    public Sprite front;
    public Sprite back;

    public int balsaSu;
    public float delay;
    public float whoDelay;

    public GameObject bullet;
}