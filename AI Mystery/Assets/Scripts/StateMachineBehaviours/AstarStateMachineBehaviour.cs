using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AstarStateMachineBehaviour : StateMachineBehaviour
{
    private Unit unit;

    public override void OnStateEnter(Animator animator, AnimatorStateInfo animatorStateInfo, int layerIndex)
    {
        unit = animator.GetComponent<Unit>();

    }

    public override void OnStateUpdate(Animator animator, AnimatorStateInfo animatorStateInfo, int layerIndex)
    {

    }
}
