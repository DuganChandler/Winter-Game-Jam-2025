using UnityEngine;

[CreateAssetMenu(fileName = "CreateNewBulletPattern", menuName = "Scriptable Objects/BulletPattern")]
public class BulletPatternAsset : ScriptableObject
{
    public float duration;

    public BulletSpawnerConfig[] bulletSpawnerConfigs;
}
