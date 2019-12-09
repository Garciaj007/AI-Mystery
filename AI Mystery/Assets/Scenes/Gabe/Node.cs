using UnityEngine;
using System.Collections;

public class Node : IHeapItem<Node> {
	
    //a node is either walkable or it isn't
	public bool walkable;
	public Vector3 worldPosition;
	public int gridX;
	public int gridY;
	public int movementPenalty;

    //distance between current node and start node
	public int gCost;
    //heuristic
	public int hCost;
	public Node parent;
	int heapIndex;
	
    //constructor default values
	public Node(bool _walkable, Vector3 _worldPos, int _gridX, int _gridY, int _penalty) {
		walkable = _walkable;
		worldPosition = _worldPos;
		gridX = _gridX;
		gridY = _gridY;
		movementPenalty = _penalty;
	}

    //get addition of both costs
	public int fCost {
		get {
			return gCost + hCost;
		}
	}

    //property
	public int HeapIndex {
		get {
			return heapIndex;
		}
		set {
			heapIndex = value;
		}
	}

    //compare fcost and hcost of current node to another node
	public int CompareTo(Node nodeToCompare) {
		int compare = fCost.CompareTo(nodeToCompare.fCost);
		if (compare == 0) {
			compare = hCost.CompareTo(nodeToCompare.hCost);
		}
		return -compare;
	}
}
