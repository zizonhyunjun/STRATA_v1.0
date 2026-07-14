using UnityEngine;
using UnityEngine.InputSystem;


public class CoserFollow : MonoBehaviour
{
    float minX;
    float maxX;

    float minY;
    float maxY;
    public InputActionReference aim;
    private void Start()
    {
        Cursor.visible = false;

        minX = GameManager.instance.minX - 1;
        maxX = GameManager.instance.maxX + 1;

        minY = GameManager.instance.minY - 1;
        maxY = GameManager.instance.maxY + 1;
    }
    void OnEnable()
    {
        aim.action.Enable();
    }
    void OnDisable()
    {
        aim.action.Disable();
    }
    void Update()
    {
        Vector3 mousePos = Mouse.current.position.ReadValue();

        Vector3 worldPos = Camera.main.ScreenToWorldPoint(mousePos);

        worldPos.z = 0f;

        worldPos.x = Mathf.Clamp(worldPos.x, minX, maxX);
        worldPos.y = Mathf.Clamp(worldPos.y, minY, maxY);

        if (float.IsNaN(worldPos.x) || float.IsNaN(worldPos.y)) return;

        transform.position = worldPos;
    }
}
