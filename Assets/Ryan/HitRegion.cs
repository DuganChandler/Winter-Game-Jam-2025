using UnityEngine;

[RequireComponent(typeof(Collider))]
public class HitRegion : MonoBehaviour
{
    [SerializeField]
    private float damageAmount = 1f;
    public float DamageAmount { get => damageAmount; set => damageAmount = value; }
    public void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out IDamagable damagable))
        {
            damagable.Damage(damageAmount);
        }
    }
    public void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.TryGetComponent(out IDamagable damagable))
        {
            damagable.Damage(damageAmount);
        }
    }
}
