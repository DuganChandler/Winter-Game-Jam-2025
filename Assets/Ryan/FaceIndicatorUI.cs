using UnityEngine;
using UnityEngine.UI;

public class FaceIndicatorUI : MonoBehaviour
{
    [Header("Faces")]
    [SerializeField] private Sprite[] faces;

    private Image m_faceImage;
    private bool isMogging = false;

    private void Awake()
    {
        m_faceImage = GetComponent<Image>();
        Player.OnPlayerHealthChanged += OnPlayerHit;
        SelfieStick.OnMeterFull += Mog;
        SelfieStick.OnMeterEmpty += UnMog;
    }
    private void OnDestroy()
    {
        Player.OnPlayerHealthChanged -= OnPlayerHit;
        SelfieStick.OnMeterFull -= Mog;
        SelfieStick.OnMeterEmpty -= UnMog;
    }

    public void SwapToFace(int i)
    {
        if (i < 0 || i >= faces.Length) return;
        m_faceImage.sprite = faces[i];
    }

    public void Mog()
    {
        SwapToFace(2);
        isMogging = true;
    }
    public void UnMog()
    {
        isMogging = false;
        ResetFace();
    }

    public void OnPlayerHit(float curr, float max)
    {
        SwapToFace(1);

        if (curr == 1) return;
        Invoke("ResetFace", 1f);
    }

    private void ResetFace()
    {
        if (isMogging)
        {
            SwapToFace(2);
            return;
        }
        SwapToFace(0);
    }
}
