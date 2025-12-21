using Unity.Cinemachine;
using UnityEngine;

public class PlayerCamera: MonoBehaviour
{
    [SerializeField] private CinemachineCamera m_followCamera;
    [SerializeField] private CinemachineCamera m_zoomedCamera;
    private bool zoomedIn = false;
    private void Awake()
    {
        ZoomIn();
    }
    public void Update()
    {
        if (GameManager.Instance.GameState == GameState.Gameplay 
            || GameManager.Instance.GameState == GameState.Pause)
        {
            if (zoomedIn)
            {
                ZoomOut();
            }
        }
        else if (!zoomedIn)
        {
            ZoomIn();
        }
    }
    public void ZoomOut()
    {
        zoomedIn = false;
        m_followCamera.Priority = 10;
        m_zoomedCamera.Priority = 5;
    }
    public void ZoomIn()
    {
        zoomedIn = true;
        m_followCamera.Priority = 5;
        m_zoomedCamera.Priority = 10;
    }
}
