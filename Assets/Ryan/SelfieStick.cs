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
    public static event System.Action OnMeterEmpty;
    private float detectionFillAmount;
    public bool IsSelfieMode = false;
    public bool TakenPhoto = false;
    private float perm;

    private void Awake()
    {
        m_selfieCamera = GetComponent<Camera>();
        detectionFillAmount = 0f;
        BossCore.OnFreeze += OnFreeze;
        perm = detectionFallOffSpeed;
    }
    private void OnDestroy()
    {
        BossCore.OnFreeze -= OnFreeze;
    }

    private void Update()
    {
        if (!TakenPhoto && IsSelfieMode && IsTargetVisible())
        {
            // Do stuff to the target
            detectionFillAmount += detectionGainSpeed * Time.deltaTime;
            if (OnTargetVisible != null) OnTargetVisible.Invoke(detectionFillAmount / detectionMaxFill);

            if (detectionFillAmount >= detectionMaxFill)
            {
                OnMeterFull?.Invoke();
                SoundManager.Instance.PlaySound("take_photo");
                TakenPhoto = true;
            }
        }
        else
        {
            detectionFillAmount -= detectionFallOffSpeed * Time.deltaTime;
            if (detectionFillAmount < 0f)
            {
                detectionFillAmount = 0f;
                detectionFallOffSpeed = perm;
                OnMeterEmpty?.Invoke();
                TakenPhoto = false;
            }
            OnTargetVisible?.Invoke(detectionFillAmount / detectionMaxFill);
        }
    }
    private bool IsTargetVisible()
    {
        Plane[] planes = GeometryUtility.CalculateFrustumPlanes(m_selfieCamera);
        return planes.All(planes => planes.GetDistanceToPoint(target.transform.position) >= 0);
    }
    private void OnFreeze(float time)
    {
        detectionFallOffSpeed = detectionMaxFill / time;
    }
}
