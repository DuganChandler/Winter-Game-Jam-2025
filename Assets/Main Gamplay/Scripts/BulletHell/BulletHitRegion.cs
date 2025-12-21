using UnityEngine;

[RequireComponent(typeof(Collider))]
public class BulletHitRegion : MonoBehaviour
{
    [SerializeField] float damageAmount = 1f;

    public float DamageAmount { get => damageAmount; set => damageAmount = value; }

    public void OnTriggerEnter(Collider other)
    {
        IDamagable damagable = other.GetComponentInParent<IDamagable>();        
        if (damagable == null) return;
        damagable.Damage(damageAmount);

        transform.parent.gameObject.SetActive(false);
    }
}
