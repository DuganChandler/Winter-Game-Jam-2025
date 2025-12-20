using UnityEngine;

public class BossMovement : MonoBehaviour
{
    Rigidbody bossRb;
    [SerializeField] Transform[] teleportPositions;
    [SerializeField] Transform centerPosition;
    Transform player;
    public float speed = 2f;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        bossRb = GetComponent<Rigidbody>();
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Teleport()
    {
        int teleportLocation = Random.Range(0, teleportPositions.Length);
        bossRb.position = teleportPositions[teleportLocation].position;
    }

    public void returnToCenter()
    {
        bossRb.position = centerPosition.position;
    }
}
