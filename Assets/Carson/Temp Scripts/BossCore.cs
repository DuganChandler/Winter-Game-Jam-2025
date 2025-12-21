using System.Collections;
using UnityEngine;

public class BossCore : MonoBehaviour
{
    [SerializeField] int maxHealth;
    [SerializeField] int currentHealth;
    [SerializeField] int proneDamage;
    int proneThreshold;

    [SerializeField] GameObject boss;
    [SerializeField] GameObject lineSpawner;
    [SerializeField] GameObject coneSpawner;
    [SerializeField] GameObject cardinalSpawner; 
    Animator animator;
    SelfieStick selfieStick;
    bool lineActive = false;
    bool coneActive = false;
    bool cardinalActive = false;

    public static event System.Action OnPhaseChange;
    public static event System.Action OnBossProne;
    Transform player;
    Rigidbody rb;

    void OnEnable()
    {
        SelfieStick.OnMeterFull += HandleMeterFull;
    }
    void OnDisable()
    {
        SelfieStick.OnMeterFull -= HandleMeterFull;
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        currentHealth = maxHealth;
        proneThreshold = maxHealth - proneDamage;
        animator = GetComponent<Animator>();
        player = GameObject.FindGameObjectWithTag("Player").transform;
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        if (currentHealth <= proneThreshold)
        {
            StartCoroutine(Prone());

        }
        if (currentHealth <= 0)
        {
            onDeath();
        }
        /*Vector3 direction = (player.position - rb.position).normalized;
        Quaternion lookRot = Quaternion.LookRotation(direction);
        rb.rotation = lookRot;  */
    }
    void onDeath()
    {
        boss.SetActive(false);
    }
    public void LineBullet()
    {
        if (lineActive)
        {
            lineSpawner.SetActive(false);
            lineActive = false;
        }
        else
        {
            lineSpawner.SetActive(true);
            lineActive = true;
        }
    }
    public void ConeBullet()
    {
        if (coneActive)
        {
            coneSpawner.SetActive(false);
            coneActive = false;
        }
        else
        {
            coneSpawner.SetActive(true);
            coneActive = true;
        }
    }
    public void CardinalBullet()
    {
        if (cardinalActive)
        {
            cardinalSpawner.SetActive(false);
            cardinalActive = false;
        }
        else
        {
            cardinalSpawner.SetActive(true);
            cardinalActive = true;
        }
    }
    private IEnumerator Prone()
    {
        animator.SetBool("Prone",true);
        OnBossProne?.Invoke();
        
        yield return new WaitForSeconds(10);

        animator.SetBool("Prone",false);
        proneThreshold = currentHealth - proneDamage;
        OnPhaseChange?.Invoke();
    }
    void HandleMeterFull()
    {
        Debug.Log("Meter is full!");
        StartCoroutine(Prone());
    }
}
