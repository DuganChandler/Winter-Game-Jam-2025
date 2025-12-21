using System.Collections.Generic;
using UnityEngine;

public class BossBulletHellManager : MonoBehaviour
{
    [SerializeField] private List<BulletPhase> phases;
    [SerializeField] private List<BulletSpawner> bulletSpawners;

    private Dictionary<SpawnerId, BulletSpawner> spawnerLookup;

    private int currentPatternIndex = 0;

    private int currentPhaseIndex = 0;

    void OnEnable()
    {
        BossCore.OnAttackChange += NextPattern;
        BossCore.OnPhaseChange += NextPhase;
        BossCore.OnDeactivateBullets += DeactivateSpawners;
    }

    void OnDisable()
    {
        BossCore.OnAttackChange -= NextPattern;
        BossCore.OnPhaseChange -= NextPhase;
        BossCore.OnDeactivateBullets -= DeactivateSpawners;
    }

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
        //ActivatePattern(0);
    }

    public void NextPattern()
    {
        DeactivateSpawners();

        currentPatternIndex = (currentPatternIndex + 1) % phases[currentPhaseIndex].BulletPatternAssets.Count;
        ActivatePattern(currentPatternIndex);
    }

    public void NextPhase()
    {
        currentPhaseIndex++;
        if (currentPhaseIndex >= phases.Count)
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
        BulletPatternAsset pattern = phases[currentPhaseIndex].BulletPatternAssets[index];

        foreach (BulletSpawnerConfig config in pattern.bulletSpawnerConfigs)
        {
            if (spawnerLookup.TryGetValue(config.SpawnerId, out BulletSpawner bulletSpawner))
            {
                bulletSpawner.ApplyConfig(config);
            }
        }

    }
    public List<BulletPhase> GetPhases()
    {
        return phases;
    }
}
