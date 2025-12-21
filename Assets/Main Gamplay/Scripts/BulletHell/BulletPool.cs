using System.Collections.Generic;
using UnityEngine;

public class BulletPool : MonoBehaviour
{
    public static BulletPool SharedInstance;

    [SerializeField] private int amountToPool;
    [SerializeField] private GameObject bulletToPool;

    private List<GameObject> pooledBullets;

    public int AmountToPool { get => amountToPool; }
    public GameObject BulletToPool { get => bulletToPool; }
    public List<GameObject> PooledBullets { get => pooledBullets; }

    void Awake()
    {
        SharedInstance = this;

        pooledBullets = new List<GameObject>(); 
        GameObject tmp;
        for (int i = 0; i < amountToPool; i++)
        {
            tmp = Instantiate(bulletToPool, transform);
            tmp.SetActive(false);
            pooledBullets.Add(tmp);
        }
    }

    public GameObject GetPooledObject()
    {
        foreach (GameObject bullet in pooledBullets) 
        {
            if (!bullet.activeInHierarchy)
            {
                bullet.SetActive(true);
                return bullet;
            }
        }
        GameObject tmp = Instantiate(bulletToPool, transform);
        tmp.SetActive(true);
        return tmp;
    }

    public void DeactivatePooledObjects()
    {
        foreach (GameObject pooledBullet in pooledBullets)
        {
            pooledBullet.SetActive(false);
        }
    }
}
