using UnityEngine;

public class Entity : MonoBehaviour, IDamagable
{
    [SerializeField]
    private float maxHealth = 10f;
    private float currentHealth;
    private void Awake()
    {
        currentHealth = maxHealth;
    }
    public void Damage(float amount)
    {
        currentHealth -= amount;
        if (currentHealth <= 0)
        {
            Die();
        }
    }
    private void Die()
    {
        // Handle entity death (e.g., play animation, disable object, etc.)
        Debug.Log($"{gameObject.name} has died.");
        Destroy(gameObject);
    }
}
