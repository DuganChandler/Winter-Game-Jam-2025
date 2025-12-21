using UnityEngine;

[RequireComponent(typeof(Collider))]
public class HitRegion : MonoBehaviour
{
    [SerializeField]
    private float damageAmount = 1f;
    public float DamageAmount { get => damageAmount; set => damageAmount = value; }
    public void OnTriggerEnter(Collider other)
    {
        IDamagable damagable = other.GetComponentInParent<IDamagable>();
        if (damagable != null)
        {
            damagable.Damage(damageAmount);
        }
    }
    public void OnCollisionEnter(Collision collision)
    {
        IDamagable damagable = collision.gameObject.GetComponentInParent<IDamagable>();
        if (damagable != null)
        {
            damagable.Damage(damageAmount);
        }
    }
}
