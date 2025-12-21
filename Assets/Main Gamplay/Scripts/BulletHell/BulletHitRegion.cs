using UnityEngine;

[RequireComponent(typeof(Collider))]
public class BulletHitRegion : MonoBehaviour
{
    [SerializeField] float damageAmount = 1f;

    public float DamageAmount { get => damageAmount; set => damageAmount = value; }

    public void OnTriggerEnter(Collider other)
    {
        IDamagable damagable = other.GetComponent<IDamagable>();        
        if (damagable == null) return;
        Player player = other.GetComponent<Player>();
        if (player != null)
        {
            if (player.IFrameActive) return;
        }

        damagable.Damage(damageAmount);

        transform.parent.gameObject.SetActive(false);
    }
}
