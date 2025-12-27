using System;
using UnityEngine;

public class KrampusBehavior : MonoBehaviour
{
    public enum EnemyState
    {
        initializing,
        idle,
        chasing,
        melee,
        prone,
        frozen,
        teleporting,
        shooting,
        dead
    }

    public EnemyState currentState, prevState;

    Transform player;
    Animator animator;
    Rigidbody bossRb;
    public float speed = 2f;
    public float attackRange = 3f;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        animator = GetComponent<Animator>();
        bossRb = GetComponent<Rigidbody>();
        currentState = EnemyState.initializing;
        
    }

    // Update is called once per frame
    void Update()
    {
        switch (currentState)
        {
            case EnemyState.initializing:
                currentState = EnemyState.idle;
                prevState = EnemyState.idle;
                break;
            case EnemyState.chasing:
                Chasing();
                break;
            case EnemyState.melee:
                Melee();
                break;
            case EnemyState.idle:
                break;
            case EnemyState.prone:
                break;
            case EnemyState.frozen:
                break;
        }
    }

    public void Melee()
    {
        animator.SetTrigger("Melee");
        SoundManager.Instance.PlaySound("slam");
    }

    public void Chasing()
    {
        Vector3 target = new Vector3(player.position.x, bossRb.position.y,player.position.z);
        // added screen.width to fix boss moving at different speeds on different PCs
        Vector3 newPos = Vector3.MoveTowards(bossRb.position,target,speed * Time.fixedDeltaTime * Screen.width);
        bossRb.MovePosition(newPos);
        SoundManager.Instance.PlaySound("clop");
    }
    
}
