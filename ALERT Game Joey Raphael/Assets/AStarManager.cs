using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// Implementation of A* Pathfinding Algorithm
// Notation for scores:
// g = cost (distance) of the path from the start node to this node
// h = heuristic: estimated cost from this node to the target
// f = g + h (total estimated cost of a path through this node)
// Goal: reach the intended target using the path with the lowest total cost (optimal path)
//
// High-level algorithm:
// 1. Start with the start node in the "open set" (nodes to explore).
// 2. Repeatedly pick the node in the open set with the lowest f-score.
// 3. For that node, check all its neighbors and update their scores if a better path is found.
// 4. Stop when you reach the end node and reconstruct the path by following "cameFrom" pointers.


public class AStarManager : MonoBehaviour
{
    // Singleton-style instance for easy global access to the pathfinding manager
    public static AStarManager instance;

    private void Awake()
    {
        instance = this;
    }

    /// <summary>
    /// Generates a path (list of nodes) from start to end using the A* algorithm.
    /// Sets g and h scores for each node and reconstructs the optimal path if found.
    /// </summary>
    /// <param name="start">The starting node.</param>
    /// <param name="end">The target node.</param>
    /// <returns>List of nodes representing the path from start to end, or null if none found.</returns>
    public List<Node> GeneratePath(Node start, Node end)
    {
        // Nodes yet to be evaluated
        List<Node> openSet = new List<Node>();

        // Initialize all nodes' gScore to "infinity" (maximum float value)
        foreach (Node n in FindObjectsOfType<Node>())
        {
            n.gScore = float.MaxValue;
        }

        // Start node has cost 0 (we're already there)
        start.gScore = 0;

        // Heuristic estimate from start to end (straight-line distance in 2D)
        start.hScore = Vector2.Distance(start.position, end.position);

        // Add start node to open set to begin searching
        openSet.Add(start);

        // Main A* loop: keep searching while there are nodes to examine
        while (openSet.Count > 0)
        {
            // Index of the node in openSet with the lowest F-score
            int lowestF = default;

            // Find node with smallest F = g + h
            for (int i = 1; i < openSet.Count; i++)
            {
                if (openSet[i].FScore() < openSet[lowestF].FScore())
                {
                    lowestF = i;
                }
            }

            // Current node is the one with the lowest F-score
            Node currentNode = openSet[lowestF];

            // Remove current node from open set (we're now processing it)
            openSet.Remove(currentNode);

            // If we've reached the end node, reconstruct the path and return it
            if (currentNode == end)
            {
                List<Node> path = new List<Node>();

                // Start from the end node
                path.Insert(0, end);

                // Walk backwards through the "cameFrom" chain until we reach the start
                while (currentNode != start)
                {
                    currentNode = currentNode.cameFrom;
                    path.Add(currentNode);
                }

                // Reverse the list so it goes from start -> end
                path.Reverse();
                return path;
            }

            // Evaluate all neighbors connected to the current node
            foreach (Node connectedNode in currentNode.connections)
            {
                // Tentative gScore = cost so far + distance to neighbor
                float heldGScore = currentNode.gScore + Vector2.Distance(currentNode.position, connectedNode.position);

                // If this new path to connectedNode is better than any previously recorded one
                if (heldGScore < connectedNode.gScore)
                {
                    // Remember how we got to this node (for path reconstruction later)
                    connectedNode.cameFrom = currentNode;

                    // Update scores
                    connectedNode.gScore = heldGScore;
                    connectedNode.hScore = Vector2.Distance(connectedNode.position, end.position);

                    // If the neighbor isn't in the open set yet, add it so it gets processed
                    if (!openSet.Contains(connectedNode))
                    {
                        openSet.Add(connectedNode);
                    }
                }
            }
        }

        // If we exit the loop without returning, no path was found
        return null;
    }

    /// <summary>
    /// Finds the node corresponding to (or closest to) the given world position.
    /// Currently uses a naming convention under "NodeParent" in the hierarchy.
    /// </summary>
    /// <param name="pos">World position to look up.</param>
    /// <returns>The found node, or null if not found.</returns>
    public Node FindNearestNode(Vector2 pos)
    {
        Node foundNode = null;
        float minDistance = float.MaxValue;

        // Alternative implementation (commented out): brute-force search by distance
        /*
        foreach (Node node in FindObjectsOfType<Node>())
        {
            float currentDistance = Vector2.Distance(pos, node.transform.position);

            if (currentDistance < minDistance)
            {
                minDistance = currentDistance;
                foundNode = node;
            }
        }
        */

        // Current implementation: expects a "NodeParent" object with children named "Node: (x , y)"
        foundNode = GameObject.Find("NodeParent")
                              .transform
                              .Find($"Node: ({pos.x} , {pos.y})")
                              .GetComponent<Node>();

        return foundNode;
    }

    /// <summary>
    /// Finds the node that is furthest away from the given position.
    /// </summary>
    /// <param name="pos">Reference position.</param>
    /// <returns>The node with the maximum distance from pos.</returns>
    public Node FindFurthestNode(Vector2 pos)
    {
        Node foundNode = null;
        float maxDistance = default;

        // Loop through all nodes and track the one with the largest distance
        foreach (Node node in FindObjectsOfType<Node>())
        {
            float currentDistance = Vector2.Distance(pos, node.transform.position);

            if (currentDistance > maxDistance)
            {
                maxDistance = currentDistance;
                foundNode = node;
            }
        }

        return foundNode;
    }

    /// <summary>
    /// Returns an array of all Node components currently in the scene.
    /// </summary>
    public Node[] AllNodes()
    {
        return FindObjectsOfType<Node>();
    }
}
