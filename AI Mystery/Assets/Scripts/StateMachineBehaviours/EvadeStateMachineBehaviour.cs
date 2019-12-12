using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EvadeStateMachineBehaviour : StateMachineBehaviour
{
    public override void OnStateEnter(Animator animator, AnimatorStateInfo animatorStateInfo, int layerIndex)
    {
        //var closestPlayer = animator.GetComponent<PlayerController>().ClosestPlayer;
        //if (closestPlayer == null) return;
        //var desired = closestPlayer.transform.position - animator.transform.position;
        //desired = desired.normalized * animator.GetComponent<PlayerController>().Speed;
        //animator.GetComponent<Rigidbody>().AddForce(desired);

        animator.GetComponent<Unit>().StopFollowingPath();
        var chaseAndEvadeRef = animator.GetComponent<ChaseAndEvade>();
    }
}
