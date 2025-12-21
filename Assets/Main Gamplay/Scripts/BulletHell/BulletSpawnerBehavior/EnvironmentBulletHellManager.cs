using System.Collections.Generic;
using UnityEngine;

public class EnvironmentBulletHellManager : MonoBehaviour
{
    [SerializeField] private List<BulletPhase> phases;
    [SerializeField] private List<BulletSpawner> bulletSpawners;

    private Dictionary<SpawnerId, BulletSpawner> spawnerLookup;

    private int currentPatternIndex = 0;

    private int currentPhaseIndex = 0;

    private bool startTriggered = false;

    void OnEnable()
    {
        BossCore.OnDeactivateBullets += DeactivateSpawners;
        BossCore.OnPhaseChange += NextPhase;  

        BossMovement.OnBossCenter += DeactivateSpawners;
        BossCore.OnBossRoar += NextPattern;
    }

    void OnDisable()
    {
        startTriggered = false;
        BossCore.OnPhaseChange -= NextPhase;  
        BossCore.OnDeactivateBullets -= DeactivateSpawners;

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

    void Update()
    {
        if (startTriggered) return;        

        if (GameManager.Instance.GameState == GameState.Gameplay)
        {
            startTriggered = true;
            ActivatePattern(0);
        }
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

}
