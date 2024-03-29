﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathStateMachineBeahviour : StateMachineBehaviour
{
    public override void OnStateEnter(Animator animator, AnimatorStateInfo animatorStateInfo, int layerIndex)
    {
        animator.gameObject.GetComponent<PlayerController>()?.Kill();
    }
}
