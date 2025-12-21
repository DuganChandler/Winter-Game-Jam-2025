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
    public static event System.Action OnDeactivateBullets;
    public static event System.Action OnBossRoar;
    public static event System.Action OnAttackChange;
    Transform player;
    Rigidbody rb;
    Transform transform;
    int attackCount;
    float currentChaseTime;
    [SerializeField] float maxChaseTime;    

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
        transform = GetComponent<Transform>();
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
        Vector3 direction = player.transform.position - transform.position;
        direction.y = 0f;
        transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.LookRotation(direction), 50 * Time.fixedDeltaTime);

        if (!animator.GetBool("Prone"))
        {
            currentChaseTime += Time.deltaTime;
            if (currentChaseTime >= maxChaseTime)
            {
                animator.SetBool("Chase", false);
                int attackChoice = Random.Range(3,0);
                
                switch (attackChoice)
                {
                    case 1: 
                        animator.SetTrigger("Shoot");
                        OnAttackChange?.Invoke();
                        Debug.Log("shoot");
                        break;
                    case 2: 
                        animator.SetTrigger("Teleport");
                        OnAttackChange?.Invoke();
                        Debug.Log("teleport");
                        break;
                    case 3: 
                        animator.SetBool("Chase", true);
                        OnDeactivateBullets?.Invoke();
                        Debug.Log("chase");
                        break;
                        
                }
                //OnAttackChange?.Invoke();
                currentChaseTime = 0;
            }
        }

        
        /*int attackChoice = Random.Range(5,0);
        //Debug.Log(attackChoice);
        switch (attackChoice)
        {
            case 1: 
                animator.SetBool("Chase", true);
                break;
            case 2: 
                animator.SetTrigger("Teleport");
                break;
            /*case 3: 
                animator.SetTrigger("Line");
                break;
            case 4: 
                animator.SetTrigger("Cone");
                break;
            case 5: 
                animator.SetTrigger("Cardinal");
                break;
            default:
                break;
        }*/

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
        OnDeactivateBullets?.Invoke();
        // bullet manager deactivate
        // timer = 0 and pause
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
    void onRoar()
    {
        OnBossRoar?.Invoke();
    }



    //add timer
    void Timer()
    {
        
    }

}
