using UnityEngine;
[CreateAssetMenu(fileName = "MapSO", menuName ="ScriptableObject/MapData")]
public class MapData: ScriptableObject
{
    public Sprite top;
    public Sprite middle;
    public Sprite bottom;

    public float minRandom;
    public float maxRandom;
    public float Sc;
    public float guri; //건물 스폰 거리 조절
    public Sprite[] building;

    public Color skyColor;
    public Color angeColor;
    public Color groundColor;
}