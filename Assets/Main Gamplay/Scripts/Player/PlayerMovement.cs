using System.Collections;
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

    [SerializeField, Range(1f, 10f)]
    private float m_dodgeSpeed = 8f;

    [SerializeField]
    private float m_dodgeCooldown = 0.4f;

    [SerializeField]
    private bool m_backstep_when_stationary = true;

    // GameObjects References
    [Header("References")]
    [SerializeField] private Transform m_visual;
    private Rigidbody m_rb;
    private PlayerInput m_playerInput;
    private Animator m_animator;
    private PlayerAttack m_playerAttack;
    private Player m_player;

    // Callable Events
    public static event System.Action<Vector2> OnMoveAction;
    public static event System.Action OnDodgeAction;

    // Bools
    private bool _isMoving;
    public bool IsRotating = true;
    public bool DodgeEnabled = true;
    public bool CanMove = true;
    public bool CanDodge = true;

    // Storage
    private Vector3 _moveVelocity;
    private Vector3 velocity = Vector3.zero;
    private Vector3 _facingDirection = Vector3.forward;

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
        m_animator = GetComponentInChildren<Animator>();
        m_playerAttack = GetComponent<PlayerAttack>();
        m_player = GetComponent<Player>();

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
        if (!CanMove) return;
        Vector2 _moveInput = moveAction.ReadValue<Vector2>();
        _isMoving = _moveInput != Vector2.zero;
        Vector3 targetVelocity = new Vector3(_moveInput.x * m_topMoveSpeed, m_rb.linearVelocity.y, _moveInput.y * m_topMoveSpeed);
        m_rb.linearVelocity = Vector3.SmoothDamp(m_rb.linearVelocity, targetVelocity, ref velocity, 0.1f);

        if (_isMoving)
        {
            if (OnMoveAction != null) OnMoveAction.Invoke(_moveInput);
            _facingDirection = targetVelocity.normalized;
            m_animator.Play("running");
        }
        else
        {
            m_animator.Play("idle");
        }
    }
    public void OnDodge(InputAction.CallbackContext context)
    {
        if (!DodgeEnabled) return;
        if (!context.performed) return;
        if (!CanDodge) return;
        CanDodge = false;
        CanMove = false;
        m_playerAttack.CanAttack = false;
        m_player.CanSelfie = false;

        int predicate = m_backstep_when_stationary && !_isMoving ? -1 : 1;
        Vector3 dodgeVelocity = predicate * _facingDirection * m_dodgeSpeed;
        m_animator.Play("dodge");
        m_rb.AddForce(dodgeVelocity, ForceMode.VelocityChange);
        StartCoroutine(OnDodgeEnd());
    }
    public IEnumerator OnDodgeEnd()
    {
        yield return new WaitUntil(() => m_animator.GetCurrentAnimatorStateInfo(0).IsName("dodge"));
        //m_player.IFrameActive = false;
        //yield return new WaitForSeconds(0.2f);
        m_player.IFrameActive = true;
        yield return new WaitForSeconds(0.5f);
        m_player.IFrameActive = false;
        yield return new WaitUntil(() => m_animator.GetCurrentAnimatorStateInfo(0).IsName("idle"));
        CanMove = true;
        
        m_player.CanSelfie = true;
        m_playerAttack.CanAttack = true;

        // Dodge CoolDown
        yield return new WaitForSeconds(m_dodgeCooldown);
        CanDodge = true;
    }
    private void Rotate()
    {
        if (!IsRotating) return;
        Quaternion targetRotation = Quaternion.LookRotation(_facingDirection);
        //targetRotation = Quaternion.RotateTowards(
        //    transform.rotation,
        //    targetRotation,
        //    360 * Time.fixedDeltaTime * m_rotationSpeed
        //    );
        m_visual.rotation = targetRotation;
    }

    public void RotateTo(Vector3 newDirection)
    {
        _facingDirection = newDirection;
        Quaternion targetRotation = Quaternion.LookRotation(_facingDirection);
        m_visual.rotation = targetRotation;
    }
}
