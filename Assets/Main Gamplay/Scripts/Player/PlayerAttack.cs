using System.Collections;
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

    private InputAction attack;
    private Animator m_animator;
    private PlayerMovement m_playerMovement;
    private Player m_player;

    [SerializeField] private bool _canAttack = true;
    public bool CanAttack
    {
        get 
        { 
            return _canAttack; 
        }
        set
        {
            _canAttack = value;
        }
    }
    private bool _isAttacking;

    private void Awake()
    {
        m_playerInput = GetComponent<PlayerInput>();
        m_animator = GetComponentInChildren<Animator>();
        m_playerMovement = GetComponent<PlayerMovement>();
        m_player = GetComponent<Player>();

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
    public void OnAttack(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            if (!CanAttack) return;
            CanAttack = false;
            m_playerMovement.CanMove = false;
            m_playerMovement.CanDodge = false;
            m_player.CanSelfie = false;
            m_hitRegion.DamageAmount = m_attackDamage;
            m_animator.Play("slash");
            //m_hitRegion.gameObject.SetActive(true);
            _isAttacking = true;
            StartCoroutine(OnAttackEnd());
        }
    }

    private IEnumerator OnAttackEnd()
    {
        yield return new WaitUntil(() => m_animator.GetCurrentAnimatorStateInfo(0).IsName("slash"));
        yield return new WaitUntil(() => m_animator.GetCurrentAnimatorStateInfo(0).IsName("idle"));
        yield return new WaitForSeconds(m_attackCooldown);
        //m_hitRegion.gameObject.SetActive(false);
        m_playerMovement.CanMove = true;
        m_playerMovement.CanDodge = true;
        m_player.CanSelfie = true;
        CanAttack = true;
        _isAttacking = false;
    }
}
