using System.Security.Cryptography;
using NUnit.Framework;
using UnityEngine;

public enum BulletSpawnerType {
    Spin,
    Tracking,
    SineWave
}

public class BulletSpawner : MonoBehaviour {
    [Header("Spawner Type")]
    [SerializeField] private BulletSpawnerType bulletSpawnerType;

    [Header("Targetting Config")]
    [SerializeField] private TargetManager targetManager;

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
    [SerializeField] private GameObject bullet;
    [SerializeField] private BulletPool bulletPool;


    [Header("Spawn Config")]
    [SerializeField] private Transform[] spawnPoints;
    [SerializeField] private Transform bulletSpawnerPivot;

    private Vector3 rotationAxis = Vector3.up;

    private float timer = 0f;
    private float sineTimer;

    private bool isFiring = true;
    private float burstTimer = 0f;

    void Update() 
    {
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
