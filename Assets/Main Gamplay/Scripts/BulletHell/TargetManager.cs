using UnityEngine;

public class TargetManager : MonoBehaviour
{
    [SerializeField] private Transform playerTarget;

    public Transform PlayerTarget { get => playerTarget; }
}
