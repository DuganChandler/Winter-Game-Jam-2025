using UnityEngine;

public class BossIdle : StateMachineBehaviour
{
    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        int attackChoice = Random.Range(5,0);
        Debug.Log(attackChoice);
        switch (attackChoice)
        {
            case 1: 
                animator.SetBool("Chase", true);
                break;
            case 2: 
                animator.SetTrigger("Teleport");
                break;
            case 3: 
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
        }
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
      
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        
    }

}
