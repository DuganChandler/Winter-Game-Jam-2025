using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEngine.Rendering.DebugUI;

public class PlayerMovement : MonoBehaviour
{
    [Header("Movement Parameters")]
    [SerializeField, Range(1f,10f)]
    private float m_topMoveSpeed = 5f;

    [SerializeField, Range(1f, 10f)]
    private float m_rotationSpeed = 5f;

    [SerializeField, Range(0f, 1f)]
    private float m_speedUp = 0.2f;

    [Header("Dodge Parameters")]

    [SerializeField, Range(1f, 20f)]
    private float m_dodgeSpeed = 10f;

    [SerializeField]
    private float m_dodge_cooldown = 1f;

    [SerializeField]
    private bool m_backstep_when_stationary = true;
    
    // GameObjects References
    private Rigidbody m_rb;

    // Callable Events
    private event System.Action<InputValue> OnMoveAction;
    private event System.Action<InputValue, float> OnDodgeAction;

    // Bools
    private bool _isMoving;

    // Storage
    private Vector3 _moveVelocity;
    private Vector3 velocity = Vector3.zero;
    private Vector3 _facingDirection = Vector3.forward;
    private float _dodgeTimer = 0f;

    private void Awake()
    {
        Setup();
    }
    private void FixedUpdate()
    {
        Vector3 targetVelocity = new Vector3(_moveVelocity.x, m_rb.linearVelocity.y, _moveVelocity.z);
        m_rb.linearVelocity = Vector3.SmoothDamp(m_rb.linearVelocity, targetVelocity, ref velocity, 0.2f);
    }
    private void Setup()
    {
        m_rb = GetComponent<Rigidbody>();
    }
    public void OnMove(InputValue value) 
    {
        Vector2 _moveInput = value.Get<Vector2>();
        _isMoving = value.Get<Vector2>() != Vector2.zero;
        _moveVelocity = new Vector3(_moveInput.x * m_topMoveSpeed, 0, _moveInput.y * m_topMoveSpeed);
        
        if (_isMoving)
        {
            if (OnMoveAction != null) OnMoveAction.Invoke(value);
            Rotate();
        }
    }
    public void OnDodge(InputValue value)
    {
        if (value.isPressed)
        {
            if (Time.time < _dodgeTimer) return;
            int predicate = m_backstep_when_stationary && !_isMoving ? -1 : 1;
            Vector3 dodgeVelocity = predicate * _facingDirection * m_dodgeSpeed;
            m_rb.AddForce(dodgeVelocity, ForceMode.VelocityChange);
            _dodgeTimer = Time.time + m_dodge_cooldown;
            if (OnDodgeAction != null) OnDodgeAction.Invoke(value, m_dodge_cooldown);
        }
    }
    private void Rotate()
    {
        _facingDirection = _moveVelocity.normalized;
        Quaternion targetRotation = Quaternion.LookRotation(_facingDirection);
        targetRotation = Quaternion.RotateTowards(
            transform.rotation,
            targetRotation,
            360 * Time.fixedDeltaTime * m_rotationSpeed
            );
        m_rb.MoveRotation(targetRotation);
    }
}
