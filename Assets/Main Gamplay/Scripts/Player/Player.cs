using UnityEngine;
using UnityEngine.InputSystem;

public class Player : Entity
{
    private PlayerMovement m_playerMovement;
    private PlayerAttack m_playerAttack;
    private PlayerInput m_playerInput;
    private SelfieStick m_selfieStick;

    // Events
    public static event System.Action OnActivateSelfieMode;
    public static event System.Action OnDeactivateSelfieMode;

    // Input Events
    private InputAction selfieAction;

    private void Awake()
    {
        m_playerMovement = GetComponent<PlayerMovement>();
        m_playerAttack = GetComponent<PlayerAttack>();
        m_playerInput = GetComponent<PlayerInput>();
        m_selfieStick = GetComponentInChildren<SelfieStick>();

        // Input Actions
        selfieAction = m_playerInput.actions["Selfie"];
    }

    private void OnEnable()
    {
        selfieAction.performed += OnSelfie;
        selfieAction.canceled += OnSelfie;
    }

    private void OnDisable()
    {
        selfieAction.performed -= OnSelfie;
        selfieAction.canceled -= OnSelfie;
    }

    #region InputActions
    public void OnSelfie(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            ActivateSelfieMode();
        }
        else if (context.canceled)
        {
            DeactivateSelfieMode();
        }
    }
    #endregion
    private float priorSpeed;
    private void ActivateSelfieMode()
    {
        OnActivateSelfieMode.Invoke();
        m_playerMovement.IsRotating = false;
        m_playerMovement.DodgeEnabled = false;
        priorSpeed = m_playerMovement.TopMoveSpeed;
        m_playerMovement.TopMoveSpeed = 2f;
        m_playerAttack.CanAttack = false;
        m_selfieStick.IsSelfieMode = true;
    }
    private void DeactivateSelfieMode()
    {
        OnDeactivateSelfieMode.Invoke();
        m_playerMovement.IsRotating = true;
        m_playerMovement.DodgeEnabled = true;
        m_playerMovement.TopMoveSpeed = priorSpeed;
        m_playerAttack.CanAttack = true;
        m_selfieStick.IsSelfieMode = false; 
    }
}
