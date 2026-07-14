using UnityEngine;

public class CloudMove : MonoBehaviour
{
    public PlayerMove playerMove;
    SpriteRenderer sp;

    [Header("Top")]
    public Transform top1;
    public Transform top2;

    [Header("Middle")]
    public Transform mid1;
    public Transform mid2;

    [Header("Bottom")]
    public Transform bottom1;
    public Transform bottom2;

    [Space(15f)]
    public GameObject sky;
    public GameObject ange;
    public GameObject ground;

    [Space(15f)]
    public float width;

    public float topY;
    public float midY;
    public float bottomY;

    public float topSpeed;
    public float midSpeed;
    public float bottomSpeed;

    private void Awake()
    {
        sp = GetComponent<SpriteRenderer>();
    }

    public void ChangeLevel(MapData data)
    {   
        top1.GetComponent<SpriteRenderer>().sprite = data.top;
        top2.GetComponent<SpriteRenderer>().sprite = data.top;
        mid1.GetComponent<SpriteRenderer>().sprite = data.middle;
        mid2.GetComponent<SpriteRenderer>().sprite = data.middle;
        bottom1.GetComponent<SpriteRenderer>().sprite = data.bottom;
        bottom2.GetComponent<SpriteRenderer>().sprite = data.bottom;

        sky.GetComponent<SpriteRenderer>().color = data.skyColor;
        ange.GetComponent<SpriteRenderer>().color = data.angeColor;
        ground.GetComponent<SpriteRenderer>().color = data.groundColor;
        
    }
    private void Update()
    {
        float targetY = playerMove.playerLerpY * playerMove.plyYPower;
        transform.position = new Vector3(transform.position.x, targetY, transform.position.z);

        MovePair(top1, top2, topSpeed, topY);
        MovePair(mid1, mid2, midSpeed, midY);
        MovePair(bottom1, bottom2, bottomSpeed, bottomY);
    }
    private void MovePair(Transform a, Transform b, float speedPower, float baseY)
    {   
        float speed = -playerMove.playerLerpX * speedPower;

        a.localPosition += new Vector3(speed * Time.deltaTime, 0, 0);
        b.localPosition += new Vector3(speed * Time.deltaTime, 0, 0);

        if (a.localPosition.x >= width) { a.localPosition = new Vector3(b.localPosition.x - width, baseY, 0); }
        if (b.localPosition.x >= width) { b.localPosition = new Vector3(a.localPosition.x - width, baseY, 0); }

        if (a.localPosition.x <= -width) { a.localPosition = new Vector3(b.localPosition.x + width, baseY, 0); }
        if (b.localPosition.x <= -width) { b.localPosition = new Vector3(a.localPosition.x + width, baseY, 0); }
    }
}