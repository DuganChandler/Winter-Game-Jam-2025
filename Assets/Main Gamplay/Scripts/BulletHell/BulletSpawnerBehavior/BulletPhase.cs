using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class BulletPhase 
{
    [SerializeField] private List<BulletPatternAsset> bulletPatternAssets;

    public List<BulletPatternAsset> BulletPatternAssets { get => bulletPatternAssets; }
}
