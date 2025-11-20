using System.Collections.Generic;
using System.Numerics;
using UnityEngine;
using UnityEngine.Tilemaps;

public class NodeManager : MonoBehaviour
{
    Tilemap TempTilemap;
    [SerializeField] Node nodePrefab;
    [SerializeField] List<Node> nodeList;
    public void CreateNodes() // Define a node for every interactable tile in game
    {
        foreach (var pos in TempTilemap.cellBounds.allPositionsWithin)
        {
            Vector3Int localPos = new Vector3Int(pos.x, pos.y, pos.z);
            UnityEngine.Vector3 place = TempTilemap.CellToWorld(localPos);
            //Debug.Log($"{pos.x}, {pos.y}, {pos.z}");
            //Debug.Log(place);
            if (TempTilemap.HasTile(localPos)) // if tile is interactable, create a node for every tile
            {
                Node node = Instantiate(nodePrefab, place, UnityEngine.Quaternion.identity);
                node.gameObject.name = $"Node: ({pos.x} , {pos.y})";
                node.transform.SetParent(GameObject.Find("NodeParent").transform);
                node.position = localPos;
                node.positionVector3Int = Vector3Int.FloorToInt(place);
                nodeList.Add(node);
            }
        }
        CreateConnections();
    }
    public void CreateConnections() // Define a list of node connections to refer to for the A* Star manager to determine our paths
    {
        for (int i = 0; i < nodeList.Count; i++)
        {
            for (int j = i + 1; j < nodeList.Count; j++)
            {
                if (UnityEngine.Vector2.Distance(nodeList[i].transform.position, nodeList[j].transform.position) <= 1.0f)
                {
                    ConnectNodes(nodeList[i], nodeList[j]);
                    ConnectNodes(nodeList[j], nodeList[i]);
                }
            }
        }
    }

    public void ClearNodes()
    {
        nodeList.Clear();
    }

    public void UpdateNodes()
    {
        ClearNodes();
        CreateNodes();
    }

    public void ConnectNodes(Node from, Node to)
    {
        if (from == to) { return; }
        from.connections.Add(to);
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        TempTilemap = GameObject.Find("TempTilemap").GetComponent<Tilemap>();
        CreateNodes(); // very unoptimized
        //Node startingNode = AStarManager.instance.FindNearestNode(new UnityEngine.Vector2(0, 0));
        //Node endingNode = AStarManager.instance.FindNearestNode(new UnityEngine.Vector2(3, 5));
        //List<Node> pathOfNodes = AStarManager.instance.GeneratePath(startingNode, endingNode);

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
