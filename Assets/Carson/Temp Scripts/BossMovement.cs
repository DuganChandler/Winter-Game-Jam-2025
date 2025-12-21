using UnityEngine;

public class BossMovement : MonoBehaviour
{
    Rigidbody bossRb;
    [SerializeField] Transform[] teleportPositions;
    [SerializeField] Transform centerPosition;
    Transform player;
    public float speed = 2f;
    public static event System.Action OnBossCenter;
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
        bossRb.GetComponent<Animator>().SetTrigger("Shoot");
    }

    public void returnToCenter()
    {
        bossRb.position = centerPosition.position;
        OnBossCenter?.Invoke();
        //bullet manager. next
    }
}
