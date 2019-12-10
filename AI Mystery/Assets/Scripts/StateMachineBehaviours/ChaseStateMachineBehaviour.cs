using UnityEngine;

public class ChaseStateMachineBehaviour : StateMachineBehaviour
{
    public override void OnStateUpdate(Animator animator, AnimatorStateInfo animatorStateInfo, int layerIndex)
    {
        var closestPlayer = animator.GetComponent<PlayerController>().ClosestPlayer;
        if (closestPlayer == null) return;
        var desired = closestPlayer.transform.position - animator.transform.position;
        desired = desired.normalized * animator.GetComponent<PlayerController>().Speed;
        animator.GetComponent<Rigidbody>().AddForce(desired);
    }
}
