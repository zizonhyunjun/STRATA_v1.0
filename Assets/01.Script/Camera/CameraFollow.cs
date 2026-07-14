using UnityEngine;
using UnityEngine.UI;

public class CameraFollow : MonoBehaviour
{
    public Transform player;
    public PlayerMove playerMove;

    [Header("Follow")]
    public float smoothTime = 0.2f;

    [Header("Camera Bounds")]
    public float minX = -5f;
    public float maxX = 5f;
    public float minY = -3f;
    public float maxY = 3f;

    [Header("Rotation")]
    public float minZRotation = 8f;
    public float maxZRotation = -8f;
    public float smoothSpeed = 8f;

    private Vector3 velocity;

    [Header("hit")]
    public float shakeTime = -1f;
    public float shakePower;

    public Image redBox;

    private void LateUpdate()
    {
        if (PauseManager.isPaused) return;

        Vector3 targetPosition = new Vector3(
            player.position.x,
            player.position.y,
            transform.position.z
        );

        targetPosition.x = Mathf.Clamp(targetPosition.x, minX, maxX);
        targetPosition.y = Mathf.Clamp(targetPosition.y, minY, maxY);

        transform.position = Vector3.SmoothDamp(
            transform.position,
            targetPosition,
            ref velocity,
            smoothTime
        );


        float shake = 0f;
        if(shakeTime  >= 0f)
        {
            shakeTime -= Time.deltaTime;
            shake = Mathf.Sin(Time.time * 60f) * shakePower;

            Color red = redBox.color;
            red.a = Mathf.Lerp(0f, 1f, shakeTime);
            redBox.color = red;
        }


        transform.position = new Vector3(transform.position.x + shake,transform.position.y,transform.position.z);

        float t = (-playerMove.playerLerpX + 1f) * 0.5f;

        float targetZ = Mathf.Lerp(minZRotation, maxZRotation, t);

        float currentZ = transform.eulerAngles.z;

        if (currentZ > 180f)
        {
            currentZ -= 360f;
        }

        float smoothZ = Mathf.Lerp(
            currentZ,
            targetZ,
            smoothSpeed * Time.deltaTime
        );

        transform.rotation = Quaternion.Euler(0f, 0f, smoothZ);
    }
    public void Shake(float power, float time)
    {
        shakePower = power;
        shakeTime = time;
    }

}