using System.Collections.Generic;
using System.Numerics;
using UnityEngine;
using UnityEngine.Tilemaps;

// Manages creation and connection of Node objects based on a Tilemap.
// These Nodes are then used by the A* pathfinding system.
public class NodeManager : MonoBehaviour
{
    // Reference to the temporary tilemap used to decide where nodes should be created
    Tilemap TempTilemap;

    // Prefab for an individual Node object
    [SerializeField] Node nodePrefab;

    // List holding all created nodes in the scene
    [SerializeField] List<Node> nodeList;

    /// <summary>
    /// Creates a Node for every "interactable" tile on the TempTilemap.
    /// Each tile that exists (HasTile) becomes a node in the graph.
    /// </summary>
    public void CreateNodes()
    {
        // Iterate over every cell position inside the tilemap bounds
        foreach (var pos in TempTilemap.cellBounds.allPositionsWithin)
        {
            Vector3Int localPos = new Vector3Int(pos.x, pos.y, pos.z);

            // Convert cell position to world-space position
            UnityEngine.Vector3 place = TempTilemap.CellToWorld(localPos);

            // If the tile exists at this position, create a node
            if (TempTilemap.HasTile(localPos))
            {
                // Instantiate a node at the tile's world position
                Node node = Instantiate(nodePrefab, place, UnityEngine.Quaternion.identity);

                // Name the node based on its grid position (used later for lookup)
                node.gameObject.name = $"Node: ({pos.x} , {pos.y})";

                // Parent the node under a "NodeParent" GameObject in the hierarchy
                node.transform.SetParent(GameObject.Find("NodeParent").transform);

                // Store its grid position and a world-space Vector3Int approximation
                node.position = localPos;
                node.positionVector3Int = Vector3Int.FloorToInt(place);

                // Add to the managed list of nodes
                nodeList.Add(node);
            }
        }

        // After all nodes are created, connect neighbors to form a graph
        CreateConnections();
    }

    /// <summary>
    /// Connects nodes that are within a distance threshold of each other.
    /// This defines the graph that A* will use to navigate.
    /// </summary>
    public void CreateConnections()
    {
        // Compare each node with every other node (naive O(n^2) approach)
        for (int i = 0; i < nodeList.Count; i++)
        {
            for (int j = i + 1; j < nodeList.Count; j++)
            {
                // If the nodes are close enough (adjacent or same cell in 2D space)
                if (UnityEngine.Vector2.Distance(nodeList[i].transform.position,
                                                 nodeList[j].transform.position) <= 1.0f)
                {
                    // Connect node i -> j and j -> i to make a bidirectional edge
                    ConnectNodes(nodeList[i], nodeList[j]);
                    ConnectNodes(nodeList[j], nodeList[i]);
                }
            }
        }
    }

    /// <summary>
    /// Clears the current list of nodes (does NOT destroy the actual GameObjects).
    /// </summary>
    public void ClearNodes()
    {
        nodeList.Clear();
    }

    /// <summary>
    /// Rebuilds the node graph from scratch:
    /// - Clears the node list
    /// - Re-creates nodes and their connections based on the tilemap
    /// </summary>
    public void UpdateNodes()
    {
        ClearNodes();
        CreateNodes();
    }

    /// <summary>
    /// Adds a directional connection from one node to another.
    /// </summary>
    /// <param name="from">Source node.</param>
    /// <param name="to">Destination node.</param>
    public void ConnectNodes(Node from, Node to)
    {
        // Don't create a self-connection
        if (from == to) { return; }

        from.connections.Add(to);
    }

    /// <summary>
    /// Unity Start method.
    /// Initializes the TempTilemap reference and generates nodes at startup.
    /// </summary>
    void Start()
    {
        // Find the tilemap named "TempTilemap" in the scene
        TempTilemap = GameObject.Find("TempTilemap").GetComponent<Tilemap>();

        // Create all nodes and their connections (currently not optimized)
        CreateNodes();

        // Example usage/testing (commented out):
        // Node startingNode = AStarManager.instance.FindNearestNode(new UnityEngine.Vector2(0, 0));
        // Node endingNode = AStarManager.instance.FindNearestNode(new UnityEngine.Vector2(3, 5));
        // List<Node> pathOfNodes = AStarManager.instance.GeneratePath(startingNode, endingNode);
    }

    /// <summary>
    /// Unity Update method (currently unused).
    /// </summary>
    void Update()
    {
        
    }
}
