using UnityEngine;

public class Entity : MonoBehaviour, IDamagable
{
    [SerializeField]
    protected float maxHealth = 10f;
    protected float currentHealth;
    protected void Awake()
    {
        currentHealth = maxHealth;
    }
    public virtual void Damage(float amount)
    {
        currentHealth -= amount;
        if (currentHealth <= 0)
        {
            Die();
        }
    }
    protected virtual void Die()
    {
        // Handle entity death (e.g., play animation, disable object, etc.)
        Debug.Log($"{gameObject.name} has died.");
        Destroy(gameObject);
    }
}
