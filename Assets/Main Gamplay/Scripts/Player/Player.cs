using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : Entity
{
    private PlayerMovement m_playerMovement;
    private PlayerAttack m_playerAttack;
    private PlayerInput m_playerInput;
    private SelfieStick m_selfieStick;
    private Animator m_animator;

    // Events
    public static event System.Action OnActivateSelfieMode;
    public static event System.Action OnDeactivateSelfieMode;
    public static event System.Action OnPlayerDeath;
    public static event System.Action<float, float> OnPlayerHealthChanged;

    // Input Events
    private InputAction selfieAction;
    public bool CanSelfie = true;
    public bool IFrameActive = false;

    protected new void Awake()
    {
        base.Awake();
        m_playerMovement = GetComponent<PlayerMovement>();
        m_playerAttack = GetComponent<PlayerAttack>();
        m_playerInput = GetComponent<PlayerInput>();
        m_selfieStick = GetComponentInChildren<SelfieStick>();
        m_animator = GetComponentInChildren<Animator>();

        // Input Actions
        selfieAction = m_playerInput.actions["Selfie"];

        GameManager.Instance.OnGameStateChanged += OnGameStateChanged;
        OnGameStateChanged(GameManager.Instance.GameState);
    }

    private void Start()
    {
        OnPlayerHealthChanged?.Invoke(currentHealth, maxHealth);
        m_playerInput.actions.Enable();
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
        if (!CanSelfie) return;
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
        m_animator.SetFloat("selfie", 1);
    }
    private void DeactivateSelfieMode()
    {
        OnDeactivateSelfieMode.Invoke();
        m_playerMovement.IsRotating = true;
        m_playerMovement.DodgeEnabled = true;
        m_playerMovement.TopMoveSpeed = priorSpeed;
        m_playerAttack.CanAttack = true;
        m_selfieStick.IsSelfieMode = false;
        m_animator.SetFloat("selfie", 0);
    }
    public override void Damage(float amount)
    {
        if (IFrameActive) return;
        currentHealth -= amount;
        OnPlayerHealthChanged?.Invoke(currentHealth, maxHealth);
        SoundManager.Instance.PlaySound("walter_hit");
        if (currentHealth <= 0)
        {
            Die();
        }
    }
    protected override void Die()
    {
        DisablePlayer();
        m_animator.Play("death");
        SoundManager.Instance.PlaySound("walter_death");
        GameManager.Instance.GameState = GameState.Gameover;
        OnPlayerDeath?.Invoke();
    }
    public void DoVictoryDance()
    {
        DisablePlayer();
        SoundManager.Instance.PlaySound("yeah");
        m_animator.Play("dance");
    }
    public void EnablePlayer()
    {
        m_playerInput.actions.Enable();
        m_playerMovement.enabled = true;
        m_playerAttack.enabled = true;
    }
    public void DisablePlayer()
    {
        m_playerInput.actions.Disable();
        m_playerMovement.enabled = false;
        m_playerAttack.enabled = false;
    }
    private void OnGameStateChanged(GameState state)
    {
        switch (state)
        {
            case GameState.Gameplay:
                EnablePlayer();
                break;
            case GameState.Win:
                DoVictoryDance();
                break;
            case GameState.MainMenu:
                m_animator.Play("dance");
                m_playerMovement.RotateTo(Vector3.back);
                goto default;
            default:
                DisablePlayer();
                break;

        }
        if (state != GameState.Gameplay)
        {
            DisablePlayer();
        }
        else
        {
            EnablePlayer();
        }
    }
}
