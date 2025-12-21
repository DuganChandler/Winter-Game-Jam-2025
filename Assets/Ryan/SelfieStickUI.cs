using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Animator))]
public class SelfieStickUI : MonoBehaviour
{
    private Animator m_animator;
    [SerializeField] private Image m_meterFill;
    [SerializeField] private Sprite m_cameraFill;
    [SerializeField] private Sprite m_takenPhotoFill;
    private void Awake()
    {
        Setup();
    }
    private void Setup()
    {
        // References
        m_animator = GetComponent<Animator>();

        // Event Related
        Player.OnActivateSelfieMode += ShowSelfieUI;
        Player.OnDeactivateSelfieMode += HideSelfieUI;
        SelfieStick.OnTargetVisible += SetMeterLength;
        SelfieStick.OnMeterFull += OnFullMeter;
    }

    private void OnDestroy()
    {
        // Event Related
        Player.OnActivateSelfieMode -= ShowSelfieUI;
        Player.OnDeactivateSelfieMode -= HideSelfieUI;
        SelfieStick.OnTargetVisible -= SetMeterLength;
    }

    private void ShowSelfieUI()
    {
        m_animator.Play("show");
    }

    private void HideSelfieUI()
    {
        m_animator.Play("hide");
    }
    private void SetMeterLength(float percentage)
    {
        if (m_meterFill != null)
        {
            m_meterFill.transform.localPosition = Vector2.right * -582.9f * (1 - percentage);
            if (percentage < 0.1f)
            {
                m_meterFill.sprite = m_cameraFill;
            }
        }
    }
    private void OnFullMeter()
    {
        if (m_meterFill != null) m_meterFill.sprite = m_takenPhotoFill;
    }
}
