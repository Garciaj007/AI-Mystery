using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdleStateMachineBehaviour : StateMachineBehaviour
{
    private Unit aStarUnit = null;
    private int iter = 0;

    public override void OnStateEnter(Animator animator, AnimatorStateInfo animatorStateInfo, int layerIndex)
    {
        aStarUnit = animator.GetComponent<Unit>();
        aStarUnit.onFinishedPathFollowing += FindNewRandomPoint;
        FindNewRandomPoint();
        aStarUnit.OnNewPathRequested();
    }

    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {

        aStarUnit.onFinishedPathFollowing -= FindNewRandomPoint;
    }

    private void FindNewRandomPoint()
    {
        aStarUnit.SetNewTarget(GetRandomPoint(null));
    }

    private Transform GetRandomPoint(Transform currentTarget)
    {
        var randPoint = GameManager.Instance.RandomPoints[Random.Range(0, GameManager.Instance.RandomPoints.Count)];
        if (++iter > 10) return randPoint;
        if (currentTarget != null && randPoint != currentTarget) GetRandomPoint(currentTarget);
        return randPoint;
    }
}
