using UnityEngine;

public enum BulletSpawnerType {
    Spin,
    Tracking,
    SineWave
}

public enum BulletSpawnerOwner
{
    Boss,
    Environment
}

public enum SpawnerId
{
    North,
    East,
    South,
    West,
    NorthEast,
    NorthWest,
    SouthEast,
    SouthWest,
    BossCardinal,
    BossCone,
    BossLine
}

public class BulletSpawner : MonoBehaviour {
    [Header("Identifier")]
    [SerializeField] private SpawnerId spawnerId;

    [Header("Spawning")]
    [SerializeField] private Transform[] spawnPoints;
    [SerializeField] private Transform bulletSpawnerPivot;

    [Header("Targetting")]
    [SerializeField] private TargetManager targetManager;

    [Header("Pooling")]
    [SerializeField] private BulletPool bulletPool;

    private BulletSpawnerType bulletSpawnerType;

    private bool useBurst;
    private float fireDuration = 1.5f;
    private float pauseDuration = 0.5f;

    private float rotationSpeed;

    private float sineAmplitude = 45f;
    private float sineFrequency = 1f;

    private float bulletSpeed;
    private float bulletLifeTime;
    private float fireRate;

    private Vector3 rotationAxis = Vector3.up;

    // rotate and sine timings
    private float timer = 0f;
    private float sineTimer;

    // burst vals
    private bool isFiring = true;
    private float burstTimer = 0f;

    // for pattern cycels
    private bool active = false;

    public SpawnerId SpawnerId { get => spawnerId; }

    void Update() 
    {
        if (!active) return;

        HandleBurst();
        if (!isFiring) return;

        timer += Time.deltaTime;
        
        switch (bulletSpawnerType)
        {
            case BulletSpawnerType.Spin:
                RotatePivot();
                break;

            case BulletSpawnerType.SineWave:
                RotatePivotSine();
                break;

            case BulletSpawnerType.Tracking:
                TrackTarget();
                break;
        }

        if (timer >= fireRate)
        {
            FireBullet();
            timer = 0;
        }
    }

    public void ApplyConfig(BulletSpawnerConfig config)
    {
        // type
        bulletSpawnerType = config.BulletSpawnerType;

        // burst
        useBurst = config.UseBurst; 
        fireDuration = config.FireDuration;
        pauseDuration = config.PauseDuration; 

        // spin 
        rotationSpeed = config.RotationSpeed;

        // sine
        sineAmplitude = config.SineAmplitude;
        sineFrequency = config.SineFrequency;

        // bullet
        bulletSpeed = config.BulletSpeed;
        bulletLifeTime = config.BulletLifeTime;
        fireRate = config.FireRate;

        active = true;
    }

    public void Disable()
    {
        active = false;
        bulletPool.DeactivatePooledObjects();
    }

    private void FireBullet() 
    {
        {
            foreach(Transform spawnPoint in spawnPoints)
            {
                GameObject bulletObj = bulletPool.GetPooledObject();

                bulletObj.transform.SetPositionAndRotation(spawnPoint.position, spawnPoint.rotation);
                Bullet bullet = bulletObj.GetComponent<Bullet>();
                bullet.Speed = bulletSpeed;
                bullet.LifeTime = bulletLifeTime;
            }
        }
    }

    private void RotatePivot() 
    {
       Quaternion rotationDelta = Quaternion.AngleAxis(rotationSpeed * Time.deltaTime, rotationAxis); 
       bulletSpawnerPivot.rotation *= rotationDelta;
    }

    private void RotatePivotSine()
    {
        sineTimer += Time.deltaTime;

        float angle = Mathf.Sin(sineTimer * sineFrequency) * sineAmplitude;
        bulletSpawnerPivot.localRotation = Quaternion.AngleAxis(angle, rotationAxis);
    }

    private void TrackTarget()
    {
        if (!targetManager.PlayerTarget) return;

        Vector3 direction = (targetManager.PlayerTarget.position - bulletSpawnerPivot.position).normalized;
        Quaternion lookRot = Quaternion.LookRotation(direction);
        bulletSpawnerPivot.rotation = lookRot;
    }

    private void HandleBurst()
    {
        if (!useBurst) return;

        burstTimer += Time.deltaTime;
        
        if (isFiring && burstTimer >= fireDuration)
        {
            isFiring = false;
            burstTimer = 0f;
        } else if (!isFiring && burstTimer >= pauseDuration) {
            isFiring = true;
            burstTimer = 0f;
        }
    }

}
