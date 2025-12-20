using UnityEngine;

[RequireComponent(typeof(Animator))]
public class SelfieStickUI : MonoBehaviour
{
    private Animator m_animator;
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
    }

    private void OnDestroy()
    {
        // Event Related
        Player.OnActivateSelfieMode -= ShowSelfieUI;
        Player.OnDeactivateSelfieMode -= HideSelfieUI;
    }

    private void ShowSelfieUI()
    {
        m_animator.Play("show");
    }

    private void HideSelfieUI()
    {
        m_animator.Play("hide");
    }
}
