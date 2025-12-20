using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerAttack : MonoBehaviour
{
    [Header("Attack Parameters")]
    [SerializeField]
    private float m_attackDamage;

    [SerializeField, Range(0f, 10f)]
    private float m_attackDuration;

    [SerializeField, Range(0f, 10f)]
    private float m_attackCooldown;

    [Header("References")]
    [SerializeField]
    private HitRegion m_hitRegion;
    private PlayerInput m_playerInput;

    private float _attackTimer;
    private float _attackCompletionTime;
    private InputAction attack;

    private void Awake()
    {
        m_playerInput = GetComponent<PlayerInput>();

        // Input Actions
        attack = m_playerInput.actions["Attack"];
    }
    private void OnEnable()
    {
        attack.performed += OnAttack;
    }
    private void OnDisable()
    {
        attack.performed -= OnAttack;
    }   
    private void Update()
    {
        if (Time.time >= _attackTimer)
        {
            m_hitRegion.gameObject.SetActive(false);
        }
    }
    public void OnAttack(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            if (Time.time < _attackCompletionTime) return;
            _attackTimer = Time.time + m_attackDuration;
            _attackCompletionTime = Time.time + m_attackDuration + m_attackCooldown;
            m_hitRegion.DamageAmount = m_attackDamage;
            m_hitRegion.gameObject.SetActive(true);
        }
    }
}
