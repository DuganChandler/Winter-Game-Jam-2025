using System.Collections.Generic;
using UnityEngine;

public class BulletHellManager : MonoBehaviour
{
    [SerializeField] private List<BulletPatternAsset> patterns;
    [SerializeField] private List<BulletSpawner> bulletSpawners;

    private Dictionary<SpawnerId, BulletSpawner> spawnerLookup;

    private int currentPatternIndex;
    private float patternTimer;

    void Awake()
    {
        spawnerLookup = new();
        foreach (BulletSpawner spawner in bulletSpawners)
        {
            spawnerLookup.Add(spawner.SpawnerId, spawner);
        }
    }

    void Start()
    {
        // testing
        ActivatePattern(0);    
    }

    void Update()
    {
        patternTimer += Time.deltaTime;

        if (patternTimer >= patterns[currentPatternIndex].duration)
        {
           NextPattern(); 
        }
    }

    // on boss roar
    public void NextPattern()
    {
        DeactivateSpawners();

        currentPatternIndex = (currentPatternIndex + 1) % patterns.Count;
        ActivatePattern(currentPatternIndex);
    }

    public void ActivatePattern(int index)
    {
        patternTimer = 0f;

        BulletPatternAsset pattern = patterns[index];

        foreach (BulletSpawnerConfig config in pattern.bulletSpawnerConfigs)
        {
            if (spawnerLookup.TryGetValue(config.SpawnerId, out BulletSpawner bulletSpawner))
            {
                bulletSpawner.ApplyConfig(config);
            }
        }

    }

    // on boss center
    public void DeactivateSpawners()
    {
        foreach (BulletPatternAsset pattern in patterns)
        {
            foreach (BulletSpawner bulletSpawner in bulletSpawners)
            {
                bulletSpawner.Disable();
            }
        }
    }
}
