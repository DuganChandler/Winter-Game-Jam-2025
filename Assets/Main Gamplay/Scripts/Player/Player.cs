using UnityEngine;

public class Player : Entity
{
    private PlayerMovement m_playerMovement;
    private PlayerAttack m_playerAttack;

    private void Awake()
    {
        m_playerMovement = GetComponent<PlayerMovement>();
        m_playerAttack = GetComponent<PlayerAttack>();
    }   
}
