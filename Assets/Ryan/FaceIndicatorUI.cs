using UnityEngine;
using UnityEngine.UI;

public class FaceIndicatorUI : MonoBehaviour
{
    [Header("Faces")]
    [SerializeField] private Sprite[] faces;

    private Image m_faceImage;

    private void Awake()
    {
        m_faceImage = GetComponent<Image>();
        Player.OnPlayerHealthChanged += OnPlayerHit;
    }
    private void OnDestroy()
    {
        Player.OnPlayerHealthChanged -= OnPlayerHit;
    }

    public void SwapToFace(int i)
    {
        if (i < 0 || i >= faces.Length) return;
        m_faceImage.sprite = faces[i];
    }

    public void OnPlayerHit(float curr, float max)
    {
        SwapToFace(1);

        if (curr == 1) return;
        Invoke("ResetFace", 1f);
    }

    private void ResetFace()
    {
        SwapToFace(0);
    }
}
