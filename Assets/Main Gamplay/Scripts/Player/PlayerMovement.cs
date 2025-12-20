using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [Header("Movement Parameters")]
    [SerializeField, Range(1f,10f)]
    private float m_topMoveSpeed = 5f;
    public float TopMoveSpeed { get { return m_topMoveSpeed; } set { m_topMoveSpeed = value; } }

    [SerializeField, Range(1f, 10f)]
    private float m_rotationSpeed = 5f;

    [Header("Dodge Parameters")]

    [SerializeField, Range(1f, 50f)]
    private float m_dodgeSpeed = 20f;

    [SerializeField]
    private float m_dodge_cooldown = 1f;

    [SerializeField]
    private bool m_backstep_when_stationary = true;
    
    // GameObjects References
    private Rigidbody m_rb;
    private PlayerInput m_playerInput;

    // Callable Events
    private static event System.Action<Vector2> OnMoveAction;
    private static event System.Action<float> OnDodgeAction;

    // Bools
    private bool _isMoving;
    public bool IsRotating = true;
    public bool DodgeEnabled = true;

    // Storage
    private Vector3 _moveVelocity;
    private Vector3 velocity = Vector3.zero;
    private Vector3 _facingDirection = Vector3.forward;
    private float _dodgeTimer = 0f;

    // Input Events
    private InputAction moveAction;
    private InputAction dodgeAction;

    private void Awake()
    {
        Setup();
    }
    private void FixedUpdate()
    {
        Move();
        Rotate();
    }
    private void Setup()
    {
        m_rb = GetComponent<Rigidbody>();
        m_playerInput = GetComponent<PlayerInput>();

        // Input Actions
        moveAction = m_playerInput.actions["Move"];
        dodgeAction = m_playerInput.actions["Dodge"];
    }
    private void OnEnable()
    {
        dodgeAction.performed += OnDodge;
    }
    private void OnDisable()
    {
        dodgeAction.performed -= OnDodge;
    }
    private void Move() 
    {
        Vector2 _moveInput = moveAction.ReadValue<Vector2>();
        _isMoving = _moveInput != Vector2.zero;
        Vector3 targetVelocity = new Vector3(_moveInput.x * m_topMoveSpeed, m_rb.linearVelocity.y, _moveInput.y * m_topMoveSpeed);
        m_rb.linearVelocity = Vector3.SmoothDamp(m_rb.linearVelocity, targetVelocity, ref velocity, 0.2f);

        if (_isMoving)
        {
            if (OnMoveAction != null) OnMoveAction.Invoke(_moveInput);
            _facingDirection = targetVelocity.normalized;
        }
    }
    public void OnDodge(InputAction.CallbackContext context)
    {
        if (!DodgeEnabled) return;
        if (!context.performed) return;
        if (Time.time < _dodgeTimer) return;
        int predicate = m_backstep_when_stationary && !_isMoving ? -1 : 1;
        Vector3 dodgeVelocity = predicate * _facingDirection * m_dodgeSpeed;
        m_rb.AddForce(dodgeVelocity, ForceMode.VelocityChange);
        _dodgeTimer = Time.time + m_dodge_cooldown;
        if (OnDodgeAction != null) OnDodgeAction.Invoke(m_dodge_cooldown);
    }
    private void Rotate()
    {
        if (!IsRotating) return;
        Quaternion targetRotation = Quaternion.LookRotation(_facingDirection);
        targetRotation = Quaternion.RotateTowards(
            transform.rotation,
            targetRotation,
            360 * Time.fixedDeltaTime * m_rotationSpeed
            );
        m_rb.MoveRotation(targetRotation);
    }
}
