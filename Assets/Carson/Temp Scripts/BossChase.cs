using Unity.Mathematics;
using UnityEngine;

public class BossChase : StateMachineBehaviour
{
    Transform player;
    Rigidbody rb;
    Transform transform;
    public float speed = 2f;
    public float attackRange = 3f;
    Vector3 facingDirection = Vector3.forward;
    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        rb = animator.GetComponent<Rigidbody>();
        transform = animator.GetComponent<Transform>();
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        //look at player
        Vector3 direction = (player.position - rb.position).normalized;
        direction.y = 0f;

        if (direction.sqrMagnitude > 0.001f)
        {
            Debug.Log("eee");
            Quaternion targetRotation = Quaternion.LookRotation(direction);
            rb.MoveRotation(
                Quaternion.RotateTowards(
                    rb.rotation,
                    targetRotation,
                    360f * Time.deltaTime
                )
            );
        }
        /*Vector3 direction = (player.position - rb.position).normalized;
        Quaternion lookRot = Quaternion.LookRotation(direction);
        rb.rotation = lookRot;*/

        //move to player
        Vector3 target = new Vector3(player.position.x, rb.position.y,player.position.z);
        Vector3 newPos = Vector3.MoveTowards(rb.position,target,speed * Time.fixedDeltaTime);
        rb.MovePosition(newPos);

        

        if (Vector3.Distance(player.position, rb.position) <= attackRange)
        {
            animator.SetTrigger("Melee");
        }
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
       animator.ResetTrigger("Melee");
       animator.SetBool("Chase", false);
    }

}
