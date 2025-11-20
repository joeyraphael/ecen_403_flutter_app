using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class Node : MonoBehaviour
{
    public Node cameFrom;
    public List<Node> connections;

    public Vector3 position;

    public Vector3Int positionVector3Int;

    public float gScore;
    public float hScore;

    public float FScore()
    {
        return gScore + hScore;
    }
    /*
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;

        if (connections.Count > 0)
        {
            for (int i = 0; i < connections.Count; i++)
            {
                Gizmos.DrawLine(transform.position, connections[i].transform.position);
            }
        }
    } */

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //OnDrawGizmos();
    }
}
