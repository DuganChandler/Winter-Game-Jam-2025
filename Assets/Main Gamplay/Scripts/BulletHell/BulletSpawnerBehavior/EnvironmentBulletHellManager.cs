using System.Collections.Generic;
using UnityEngine;

public class EnvironmentBulletHellManager : MonoBehaviour
{
    [SerializeField] private List<List<BulletPatternAsset>> patterns;
    [SerializeField] private List<BulletSpawner> bulletSpawners;

    private Dictionary<SpawnerId, BulletSpawner> spawnerLookup;

    private int currentPatternIndex = 0;

    private int currentPhaseIndex = 0;

    void OnEnable()
    {
        BossCore.OnBossProne += DeactivateSpawners;
        BossCore.OnPhaseChange += NextPhase;  

        BossMovement.OnBossCenter += DeactivateSpawners;
        BossCore.OnBossRoar += NextPattern;
    }

    void OnDisable()
    {
        BossCore.OnPhaseChange -= NextPhase;  
        BossCore.OnBossProne -= DeactivateSpawners;

        BossMovement.OnBossCenter -= DeactivateSpawners;
        BossCore.OnBossRoar -= NextPattern;
    }

    void Awake()
    {
        spawnerLookup = new();
        foreach (BulletSpawner spawner in bulletSpawners)
        {
            spawnerLookup.Add(spawner.SpawnerId, spawner);
        }
    }

    public void NextPattern()
    {
        DeactivateSpawners();

        currentPatternIndex = (currentPatternIndex + 1) % patterns[currentPhaseIndex].Count;
        ActivatePattern(currentPatternIndex);
    }

    public void NextPhase()
    {
        currentPhaseIndex++;
        if (currentPhaseIndex >= patterns.Count)
        {
            Debug.Log("Current Phase Index too high");
            return;
        }
        currentPatternIndex = 0;
        ActivatePattern(currentPatternIndex);
    }

    public void DeactivateSpawners()
    {
        foreach (BulletSpawner bulletSpawner in bulletSpawners)
        {
            bulletSpawner.Disable();
        }
    }

    private void ActivatePattern(int index)
    {
        BulletPatternAsset pattern = patterns[currentPhaseIndex][index];

        foreach (BulletSpawnerConfig config in pattern.bulletSpawnerConfigs)
        {
            if (spawnerLookup.TryGetValue(config.SpawnerId, out BulletSpawner bulletSpawner))
            {
                bulletSpawner.ApplyConfig(config);
            }
        }

    }

}
