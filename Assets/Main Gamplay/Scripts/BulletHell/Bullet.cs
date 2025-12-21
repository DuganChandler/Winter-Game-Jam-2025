using UnityEngine;

public class Bullet : MonoBehaviour
{
    private float lifeTime;
    private float speed;
    private float rotation;

    private Vector3 spawnpoint;
    private float timer;

    public float LifeTime { get => lifeTime; set => lifeTime = value; } 
    public float Speed { get => speed; set => speed = value; } 
    public float Rotation { get => rotation; set => rotation = value; }

    void OnEnable()
    {
        timer = 0f;
    }

    void Update()
    {
        timer += Time.deltaTime;
        if (timer >= lifeTime)
        {
            gameObject.SetActive(false);
            return;
        } 
        MoveBullet();
    }

    private void MoveBullet()
    {
        transform.position += speed * Time.deltaTime * transform.forward;
    }

}
