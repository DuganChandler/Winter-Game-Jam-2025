using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Animator))]
public class SelfieStickUI : MonoBehaviour
{
    private Animator m_animator;
    [SerializeField] private Image m_meterFill;
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
        if (m_meterFill != null) m_meterFill.transform.localPosition = Vector2.right * -582.9f * (1 - percentage); 
    }
}
