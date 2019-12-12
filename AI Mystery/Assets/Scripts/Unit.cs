using UnityEngine;
using System.Collections;

public class Unit : MonoBehaviour
{
    public delegate void OnFinishedPathFollowingDelegate();
    public event OnFinishedPathFollowingDelegate OnFinishedPathFollowing;

    [SerializeField] private Transform target;
    [SerializeField] private Vector3[] path;
    [SerializeField] private float speed = 20.0f;
    [SerializeField] private int targetIndex;

    private Coroutine pathFollowingCoroutine = null;

    public Transform Target { get => target; }

    //start of entire algorithm
    void Start()
    {
        OnFinishedPathFollowing += StopAllFollowingPath;
        //OnNewPathRequested();
    }

    public void OnNewPathRequested()
    {
        if (target != null) PathRequestManager.RequestPath(transform.position, target.position, OnPathFound);
    }

    //restarts coroutine
    public void OnPathFound(Vector3[] newPath, bool pathSuccessful)
    {
        if (pathSuccessful)
        {
            path = newPath;

            targetIndex = 0;

            if (pathFollowingCoroutine != null) StopCoroutine(pathFollowingCoroutine);
            pathFollowingCoroutine = StartCoroutine(FollowPath());
        }
    }

    //movement from one node to another
    IEnumerator FollowPath()
    {
        if(path == null || path.Length < 1)
        {
            OnFinishedPathFollowing?.Invoke();
            yield break;
        }
        Vector3 currentWaypoint = path[0];

        while (true)
        {
            if (transform.position == currentWaypoint)
            {
                targetIndex++;
                if (targetIndex >= path.Length)
                {
                    OnFinishedPathFollowing?.Invoke();
                    yield break;
                }
                currentWaypoint = path[targetIndex];
            }

            transform.position = Vector3.MoveTowards(transform.position, currentWaypoint, speed * Time.deltaTime);
            transform.LookAt(currentWaypoint);
            yield return null;

        }
    }

    public void OnDrawGizmos()
    {
        if (path != null)
        {
            for (int i = targetIndex; i < path.Length; i++)
            {
                Gizmos.color = Color.black;
                Gizmos.DrawCube(path[i], Vector3.one);

                if (i == targetIndex)
                {
                    Gizmos.DrawLine(transform.position, path[i]);
                }
                else
                {
                    Gizmos.DrawLine(path[i - 1], path[i]);
                }
            }
        }
    }

    public void StopFollowingPath()
    {
        if(pathFollowingCoroutine == null) return;
        StopCoroutine(pathFollowingCoroutine);
        pathFollowingCoroutine = null;
    }

    public void StopAllFollowingPath() => StopAllCoroutines();

    public void SetNewTarget(Transform target) => this.target = target;
}
