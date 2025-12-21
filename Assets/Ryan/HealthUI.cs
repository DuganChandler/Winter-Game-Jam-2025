using UnityEngine;
using UnityEngine.UI;

public class HealthUI : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Sprite m_heartEmpty;
    [SerializeField] private Sprite m_heartFull;
    [SerializeField] private Transform m_heartContainer;
    private void Awake()
    {
        Player.OnPlayerHealthChanged += UpdateHealthUI;
    }

    private void OnDestroy()
    {
        Player.OnPlayerHealthChanged -= UpdateHealthUI;
    }

    private void UpdateHealthUI(float currHealth, float maxHealth)
    {
        // Ensure Correct Numbers of hearts
        if (maxHealth > m_heartContainer.childCount)
        {
            while (maxHealth > m_heartContainer.childCount)
            {
                GameObject newHeart = new GameObject("Heart", typeof(Image));
                newHeart.transform.SetParent(m_heartContainer, false);
                Image heartImage = newHeart.GetComponent<Image>();
                heartImage.sprite = m_heartEmpty;
            }
        }
        else if (maxHealth < m_heartContainer.childCount)
        {
            while (maxHealth < m_heartContainer.childCount)
            {
                Destroy(m_heartContainer.GetChild(m_heartContainer.childCount - 1));
            }
        }

        // Update heart fill states
        for (int i = 0; i < m_heartContainer.childCount; i++)
        {
            Image heartImage = m_heartContainer.GetChild(i).GetComponent<Image>();
            if (i < currHealth)
            {
                heartImage.sprite = m_heartFull;
            }
            else
            {
                heartImage.sprite = m_heartEmpty;
            }
        }
    }
}
