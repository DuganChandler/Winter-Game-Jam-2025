using UnityEngine;
using UnityEngine.InputSystem;

public class Player : Entity
{
    private PlayerMovement m_playerMovement;
    private PlayerAttack m_playerAttack;
    private PlayerInput m_playerInput;

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
            Debug.Log("Activate Selfie Mode");
            ActivateSelfieMode();
        }
        else if (context.canceled)
        {
            Debug.Log("Deactivate Selfie Mode");
            DeactivateSelfieMode();
        }
    }
    #endregion
    private void ActivateSelfieMode()
    {
        OnActivateSelfieMode.Invoke();
    }
    private void DeactivateSelfieMode()
    {
        OnDeactivateSelfieMode.Invoke();
    }
}
