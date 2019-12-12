using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System;

public class Pathfinding : MonoBehaviour {
	
	PathRequestManager requestManager;
	Grid grid;
	
    //get other scripts off this gameobject
	void Awake() {
		requestManager = GetComponent<PathRequestManager>();
		grid = GetComponent<Grid>();
	}
	
    //calls coroutine
	public void StartFindPath(Vector3 startPos, Vector3 targetPos) {
		StartCoroutine(FindPath(startPos,targetPos));
	}
	
    //since the execution of this function takes long, let other functions in different thread
	IEnumerator FindPath(Vector3 startPos, Vector3 targetPos) {
		
        //start timer
		Stopwatch sw = new Stopwatch();
		sw.Start();
		
		Vector3[] waypoints = new Vector3[0];
		bool pathSuccess = false;
		
        //get position of start and target node in world space
		Node startNode = grid.NodeFromWorldPoint(startPos);
		Node targetNode = grid.NodeFromWorldPoint(targetPos);
		startNode.parent = startNode;
		
		//if both nodes are walkable
		if (startNode.walkable && targetNode.walkable) {
            //heap is template cladss we defined
			Heap<Node> openSet = new Heap<Node>(grid.MaxSize);
            //hashset holds set of nodes 
			HashSet<Node> closedSet = new HashSet<Node>();
			openSet.Add(startNode);
			
            //heap is not empty, 
			while (openSet.Count > 0) {
                //pop off heap, and add node
				Node currentNode = openSet.RemoveFirst();
				closedSet.Add(currentNode);
				
                //if node = finish, we are at the target, return out of function
				if (currentNode == targetNode) {
					sw.Stop();
					//print ("Path found: " + sw.ElapsedMilliseconds + " ms");
					pathSuccess = true;
					break;
				}
				
                //for all neighbours of currentnode,
				foreach (Node neighbour in grid.GetNeighbours(currentNode)) {
                    //skip iteration if neighbour is unwalkable
					if (!neighbour.walkable || closedSet.Contains(neighbour)) {
						continue;
					}
					
                    //calculate weight, using gcost
					int newMovementCostToNeighbour = currentNode.gCost + GetDistance(currentNode, neighbour) + neighbour.movementPenalty;
                    //if the current node is closer, than the neightbour is, update values
					if (newMovementCostToNeighbour < neighbour.gCost || !openSet.Contains(neighbour)) {
						neighbour.gCost = newMovementCostToNeighbour;
						neighbour.hCost = GetDistance(neighbour, targetNode);
						neighbour.parent = currentNode;
						
						if (!openSet.Contains(neighbour))
							openSet.Add(neighbour);
						else 
							openSet.UpdateItem(neighbour);
					}
				}
			}
		}
		yield return null;
        //if bool true, retrace path
		if (pathSuccess) {
			waypoints = RetracePath(startNode,targetNode);
		}
		requestManager.FinishedProcessingPath(waypoints,pathSuccess);		
	}
		
	//trace path
	Vector3[] RetracePath(Node startNode, Node endNode) {
		List<Node> path = new List<Node>();
		Node currentNode = endNode;
		
        //while we are along the path, not at start, add node to path, and set node to parent
		while (currentNode != startNode) {
			path.Add(currentNode);
			currentNode = currentNode.parent;
		}
		Vector3[] waypoints = SimplifyPath(path);
        //reverse path so it is from start to end, not other way around
		Array.Reverse(waypoints);
		return waypoints;
		
	}
	
	Vector3[] SimplifyPath(List<Node> path) {
		List<Vector3> waypoints = new List<Vector3>();
		Vector2 directionOld = Vector2.zero;
		
        //iterate through path,
		for (int i = 1; i < path.Count; i ++) {
			Vector2 directionNew = new Vector2(path[i-1].gridX - path[i].gridX,path[i-1].gridY - path[i].gridY);
            //if we are going in the right direction, and not backtracking, add path iteration to waypoint
			if (directionNew != directionOld) {
				waypoints.Add(path[i].worldPosition);
			}
			directionOld = directionNew;
		}
		return waypoints.ToArray();
	}
	
    //get actual distance between nodes 
	int GetDistance(Node nodeA, Node nodeB) {
		int dstX = Mathf.Abs(nodeA.gridX - nodeB.gridX);
		int dstY = Mathf.Abs(nodeA.gridY - nodeB.gridY);
		
		if (dstX > dstY)
			return 14*dstY + 10* (dstX-dstY);
		return 14*dstX + 10 * (dstY-dstX);
	}
	
	
}
