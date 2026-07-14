using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class Za : MonoBehaviour
{
    public RectTransform bar;

    public float minYPosition = -50f; 
    public float maxYPosition = 50f;
    private void Start()
    {
        bar.localPosition = new Vector3(bar.localPosition.x, 0f, bar.localPosition.z);
    }
    void Update()
    {
        float ee = GameManager.instance.playerMove.playerLerpY;

        

        bar.localPosition = new Vector3(bar.localPosition.x, ee*50f, bar.localPosition.z);
    }

}
