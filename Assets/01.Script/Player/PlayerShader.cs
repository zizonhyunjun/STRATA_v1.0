using System.Collections;
using UnityEngine;

public class PlayerShader : MonoBehaviour
{
    SpriteRenderer sp;
    Material material;

    private readonly int shaderFlashAmount = Shader.PropertyToID("_FlashAmount");
    private readonly int shaderFlashColor = Shader.PropertyToID("_FlashColor");

    private void Start()
    {
        sp = GetComponent<SpriteRenderer>();
        material = sp.sharedMaterial;
        material.SetFloat(shaderFlashAmount, 0f);
    }
    public void A()
    {
        StartCoroutine(HitShader());
    }
    private IEnumerator HitShader()
    {
        material.SetColor(shaderFlashColor, Color.white);
        material.SetFloat(shaderFlashAmount, 1f);

        yield return new WaitForSeconds(0.1f);

        material.SetFloat(shaderFlashAmount, 0f);
    }
}
