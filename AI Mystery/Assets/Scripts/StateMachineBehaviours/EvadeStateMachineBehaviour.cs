using UnityEngine;

public class EvadeStateMachineBehaviour : StateMachineBehaviour
{
    ChaseAndEvade chaseEvadeRef = null;
    GameObject closestPlayer = null;

    public override void OnStateEnter(Animator animator, AnimatorStateInfo animatorStateInfo, int layerIndex)
    {
        animator.GetComponent<Unit>().StopAllFollowingPath();
        chaseEvadeRef = animator.GetComponent<ChaseAndEvade>();
        closestPlayer = animator.GetComponent<PlayerController>().ClosestPlayer;
        chaseEvadeRef.chase = false;
    }

    public override void OnStateUpdate(Animator animator, AnimatorStateInfo animatorStateInfo, int layerIndex)
    {
        if (closestPlayer == null) return;
        chaseEvadeRef.target = closestPlayer.transform;
    }

    public override void OnStateExit(Animator animator, AnimatorStateInfo animatorStateInfo, int layerIndex)
    {
        chaseEvadeRef.target = null;
        chaseEvadeRef.chase = false;
    }
}
