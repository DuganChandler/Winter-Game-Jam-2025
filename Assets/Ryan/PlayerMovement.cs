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
    // GameObjects References
    private Rigidbody m_rb;

    // Callable Events
    private event System.Action<InputValue> OnMoveAction;

    // Bools
    private bool _isMoving;

    // Storage
    private Vector3 _moveVelocity;
    private Vector3 velocity = Vector3.zero;

    private void Awake()
    {
        Setup();
    }
    private void FixedUpdate()
    {
        Vector3 targetVelocity = new Vector3(_moveVelocity.x, m_rb.linearVelocity.y, _moveVelocity.z);
        m_rb.linearVelocity = Vector3.SmoothDamp(m_rb.linearVelocity, targetVelocity, ref velocity, 0.2f);
    }
    private void LateUpdate()
    {
        
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
        if (OnMoveAction != null) OnMoveAction.Invoke(value);
        if (_isMoving) Rotate();
    }
    private void Rotate()
    {
        Quaternion targetRotation = Quaternion.LookRotation(_moveVelocity.normalized);
        targetRotation = Quaternion.RotateTowards(
            transform.rotation,
            targetRotation,
            360 * Time.fixedDeltaTime * m_rotationSpeed
            );
        m_rb.MoveRotation(targetRotation);
    }
}
