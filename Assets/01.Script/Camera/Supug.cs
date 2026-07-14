using UnityEngine;

public class Supug : MonoBehaviour
{
    public Transform horizonHUD;

    public Transform playerTransform;

    void Update()
    {
        float targetZAngle = -playerTransform.localEulerAngles.z;
        horizonHUD.localRotation = Quaternion.Euler(0f, 0f, targetZAngle);
    }
}
