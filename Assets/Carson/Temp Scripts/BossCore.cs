using System.Collections;
using Unity.Collections;
using UnityEngine;

public class BossCore : Entity
{
    /*[SerializeField] int maxHealth;
    [SerializeField] int currentHealth;*/
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
    public float freezeTime = 5f;
    public float proneTime = 2f;

    public static event System.Action OnPhaseChange;
    public static event System.Action OnDeactivateBullets;
    public static event System.Action OnBossDeactivateBullets;
    public static event System.Action OnBossRoar;
    public static event System.Action OnAttackChange;
    public static event System.Action<float> OnFreeze;
    [SerializeField] Transform player;
    Rigidbody rb;
    [SerializeField] Transform transform;
    int attackCount;
    float currentChaseTime;
    [SerializeField] float maxChaseTime;    
    bool frozen;
    public bool freezeFrame;
    public bool attacking = false;
    bool inFront;
    bool dead = false;
    int newAttacks;
    [SerializeField] int uniqueAttacks;
    int newShots;
    int chaseCount;
    int teleporCount;
    [SerializeField] BossBulletHellManager bossBulletHellManager;
    bool phaseChanging;
    bool startTriggered;

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
        proneThreshold = (int)maxHealth - proneDamage;
        animator = GetComponent<Animator>();
        //player = GameObject.FindGameObjectWithTag("Player").transform;
        rb = GetComponent<Rigidbody>();
        //transform = GetComponent<Transform>();
        newAttacks = 0;
        newShots = 0;
        chaseCount = 0;
        teleporCount = 0;
        freezeFrame = false;
    }

    // Update is called once per frame
    void Update()
    {
        
        if (currentHealth <= proneThreshold)
        {
            Debug.Log("wow");
            StartCoroutine(Prone());
            animator.speed = 1;
            proneThreshold = (int)currentHealth - proneDamage;

        }
        if (!dead)
        {
            if(newAttacks >= uniqueAttacks)
            {
                phaseChanging = true;
                animator.SetTrigger("Center");
                Debug.Log("Change Phase");
            }
        if (!frozen)
        {
            Vector3 direction = player.transform.position - transform.position;
            direction.y = 0f;
            transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.LookRotation(direction), 50 * Time.fixedDeltaTime); 
        }
        Vector3 toTarget = (player.position - transform.position).normalized;
    		
    	if (Vector3.Dot(toTarget, transform.forward) > 0) {
    		//Debug.Log("Target is in front of this game object.");
            inFront = true;
    	} else {
    		//Debug.Log("Target is not in front of this game object.");
            inFront = false;
    	}

            if (startTriggered)
            {
                if (!animator.GetBool("Prone") || !phaseChanging)
        {
            currentChaseTime += Time.deltaTime;
            if (currentChaseTime >= maxChaseTime)
            {
                rb.constraints = RigidbodyConstraints.FreezePositionY | RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;
                animator.SetBool("Chase", false);
                frozen = false;
                attacking = false;
                int attackChoice = Random.Range(3,0);
                
                switch (attackChoice)
                {
                    case 1: 
                        animator.SetTrigger("Shoot");
                        OnAttackChange?.Invoke();
                        frozen = true;
                        attacking = true;
                        
                        if(newShots<= bossBulletHellManager.GetPhases().Count)
                            {
                                newAttacks++;
                            }
                        newShots++;
                        Debug.Log("shoot");
                        break;
                    case 2: 
                        animator.SetTrigger("Teleport");
                        //OnAttackChange?.Invoke();
                        
                        if(teleporCount <= 2)
                            {
                                newAttacks++;
                            }
                        teleporCount++;
                        Debug.Log("teleport");
                        break;
                    case 3: 
                        animator.SetBool("Chase", true);
                        OnBossDeactivateBullets?.Invoke();
                        if(chaseCount < 1)
                            {
                                newAttacks++;
                            }
                        chaseCount++;
                        Debug.Log("chase");
                        break;
                        
                }
                //OnAttackChange?.Invoke();
                currentChaseTime = 0;
                Debug.Log("attacks: "+newAttacks);
            }
        }
            }
        if (GameManager.Instance.GameState == GameState.Gameplay)
        {
            startTriggered = true;
            
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
    public override void Damage(float amount)
    {
        if(freezeFrame)
        {
            currentHealth -= amount * 4;
            Debug.Log("Freeze Hit");
        }else if(attacking && inFront || animator.GetBool("Prone"))
        {
            currentHealth -= amount *3;
            Debug.Log("CRIT");
        }else if (!inFront)
        {
            currentHealth -= amount;
            Debug.Log("Back Hit");
        }
        
        if (currentHealth <= 0 && !dead)
        {
            animator.SetTrigger("Death");
            dead = true;
            GameManager.Instance.GameState = GameState.Win;
            //Die();
        }
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
        animator.SetTrigger("ProneTrigger");
        OnDeactivateBullets?.Invoke();
        OnBossDeactivateBullets?.Invoke();
        // bullet manager deactivate
        // timer = 0 and pause
        currentChaseTime = 0;
        frozen = true;
        rb.constraints = RigidbodyConstraints.FreezeAll;
        yield return new WaitForSeconds(proneTime);

        animator.SetBool("Prone",false);
        rb.constraints = RigidbodyConstraints.FreezePositionY | RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;

        //frozen = false;
    }
    public void ChangePhase()
    {
        newAttacks = 0;
        newShots = 0;
        chaseCount = 0;
        teleporCount = 0;
        OnPhaseChange?.Invoke();
        phaseChanging = false;
    }
    private IEnumerator Freeze()
    {
        animator.speed = 0;
        OnDeactivateBullets?.Invoke();
        OnBossDeactivateBullets?.Invoke();
        frozen = true;
        freezeFrame = true;
        rb.constraints = RigidbodyConstraints.FreezeAll;
        // bullet manager deactivate
        // timer = 0 and pause
        currentChaseTime = 0;
        OnFreeze?.Invoke(freezeTime);
        yield return new WaitForSeconds(freezeTime);
        rb.constraints = RigidbodyConstraints.FreezePositionY | RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;

        animator.speed = 1;
        //frozen = false;
        freezeFrame = false;
    }
    void HandleMeterFull()
    {
        Debug.Log("Meter is full!");
        StartCoroutine(Freeze());
    }
    void onRoar()
    {
        OnBossRoar?.Invoke();
    }



    //add timer
    public void TeleportShoot()
    {
        frozen = true;
        rb.constraints = RigidbodyConstraints.FreezePositionY | RigidbodyConstraints.FreezeRotation;
        OnAttackChange?.Invoke();
    }
    public void SetFrozen(bool value)
    {
        frozen = value;
    }

}
