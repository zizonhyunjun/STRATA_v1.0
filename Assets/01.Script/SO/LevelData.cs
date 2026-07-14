using UnityEngine;
[CreateAssetMenu(fileName = "LevelData", menuName = "ScriptableObject/LevelData")]
public class LevelData : ScriptableObject
{
    public MapData map;
    public EnemyData[] enemy;
    public int enSpawnMin;
    public int enSpawnMax;
}
