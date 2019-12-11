﻿using UnityEngine;
using System.Collections;

public class Unit : MonoBehaviour {
    
	[SerializeField] private Transform target;
    [SerializeField] private float speed = 20.0f;
	[SerializeField] private Vector3[] path;
	[SerializeField] private int targetIndex;

    //start of entire algorithm
	void Start() {
        OnNewPathRequested();
	}

    public void OnNewPathRequested()
    {
        if (target != null)
            PathRequestManager.RequestPath(transform.position, target.position, OnPathFound);
    }

    //restarts coroutine
    public void OnPathFound(Vector3[] newPath, bool pathSuccessful) {
		if (pathSuccessful) {
			path = newPath;
			targetIndex = 0;
			StopCoroutine("FollowPath");
			StartCoroutine("FollowPath");
		}
	}

    //movement from one node to another
	IEnumerator FollowPath() {
		Vector3 currentWaypoint = path[0];

		while (true) {
			if (transform.position == currentWaypoint) {
				targetIndex ++;
				if (targetIndex >= path.Length) {
					yield break;
				}
				currentWaypoint = path[targetIndex];
			}

			transform.position = Vector3.MoveTowards(transform.position,currentWaypoint,speed * Time.deltaTime);
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
}
