using UnityEngine;

[System.Serializable]
public class BulletSpawnerConfig
{
    [Header("Associated Spawner")]
    [SerializeField] SpawnerId spawnerId;

    [Header("Spawner Type")]
    [SerializeField] private BulletSpawnerType bulletSpawnerType;

    [Header("Burst Config")]
    [SerializeField] private bool useBurst;
    [SerializeField] private float fireDuration = 1.5f;
    [SerializeField] private float pauseDuration = 0.5f;

    [Header("Rotation Config")]
    [SerializeField] private float rotationSpeed;

    [Header("Sine Config")]
    [SerializeField] private float sineAmplitude = 45f;
    [SerializeField] private float sineFrequency = 1f;

    [Header("Bullet Config")]
    [SerializeField] private float bulletSpeed;
    [SerializeField] private float bulletLifeTime;
    [SerializeField] private float fireRate;

    public SpawnerId SpawnerId { get => spawnerId; }

    public BulletSpawnerType BulletSpawnerType { get => bulletSpawnerType; }

    public bool UseBurst { get => useBurst; }
    public float FireDuration { get => fireDuration; }
    public float PauseDuration { get => pauseDuration; }

    public float RotationSpeed { get => rotationSpeed; }

    public float SineAmplitude { get => sineAmplitude; }
    public float SineFrequency { get => sineFrequency; }

    public float BulletSpeed { get => bulletSpeed; }
    public float BulletLifeTime { get => bulletLifeTime; }
    public float FireRate { get => fireRate; }
}
