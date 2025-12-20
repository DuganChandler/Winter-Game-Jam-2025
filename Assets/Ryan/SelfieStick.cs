using UnityEngine;
using System.Linq;

[RequireComponent(typeof(Camera))]
public class SelfieStick : MonoBehaviour
{
    [Header("Specifications")]
    [SerializeField, Range(1f, 30f)] private float detectionMaxFill = 10f;
    [SerializeField, Range(0.01f, 10f)] private float detectionGainSpeed = 0.25f;
    [SerializeField, Range(0.01f, 10f)] private float detectionFallOffSpeed = 0.1f;
    [SerializeField] private GameObject target;
    private Camera m_selfieCamera;
    public static event System.Action<float> OnTargetVisible;
    public static event System.Action OnMeterFull;
    private float detectionFillAmount;
    public bool IsSelfieMode = false;

    private void Awake()
    {
        m_selfieCamera = GetComponent<Camera>();
        detectionFillAmount = 0f;
    }
    private void Update()
    {
        if (IsSelfieMode && IsTargetVisible())
        {
            // Do stuff to the target
            detectionFillAmount += detectionGainSpeed * Time.deltaTime;
            if (OnTargetVisible != null) OnTargetVisible.Invoke(detectionFillAmount / detectionMaxFill);

            if (detectionFillAmount >= detectionMaxFill)
            {
                if (OnMeterFull != null) OnMeterFull.Invoke();
                detectionFillAmount = 0f;
            }
        }
        else
        {
            detectionFillAmount -= detectionFallOffSpeed * Time.deltaTime;
            if (detectionFillAmount < 0f) detectionFillAmount = 0f;
            if (OnTargetVisible != null) OnTargetVisible.Invoke(detectionFillAmount / detectionMaxFill);
        }
    }
    private bool IsTargetVisible()
    {
        Plane[] planes = GeometryUtility.CalculateFrustumPlanes(m_selfieCamera);
        return planes.All(planes => planes.GetDistanceToPoint(target.transform.position) >= 0);
    }
}
