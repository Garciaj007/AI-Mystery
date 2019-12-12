using UnityEngine;

public class IdleStateMachineBehaviour : StateMachineBehaviour
{
    private Unit aStarUnit = null;
    private int iter = 0;

    public override void OnStateEnter(Animator animator, AnimatorStateInfo animatorStateInfo, int layerIndex)
    {
        aStarUnit = animator.GetComponent<Unit>();
        aStarUnit.OnFinishedPathFollowing += FindNewRandomPoint;
        aStarUnit.StopFollowingPath();
        aStarUnit.SetNewTarget(GetRandomPoint(null));
        aStarUnit.OnNewPathRequested();
    }

    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        aStarUnit.OnFinishedPathFollowing -= FindNewRandomPoint;
    }

    private void FindNewRandomPoint()
    {
        aStarUnit.StopFollowingPath();
        aStarUnit.SetNewTarget(GetRandomPoint(aStarUnit.Target));
        aStarUnit.OnNewPathRequested();
    }

    private Transform GetRandomPoint(Transform currentTarget)
    {
        var randPoint = GameManager.Instance.RandomPoints[Random.Range(0, GameManager.Instance.RandomPoints.Count)];
        if (++iter > 10) return randPoint;
        if (currentTarget != null && randPoint != currentTarget) GetRandomPoint(currentTarget);
        return randPoint;
    }
}
