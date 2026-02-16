using System.Collections.Generic;
using UnityEngine;

public class NodeController : MonoBehaviour
{
    private List<Transform> nodes = new List<Transform>();

    void Start()
    {
        foreach (Transform child in transform)
        {
            nodes.Add(child);
        }
    }

    public Transform GetNextNode(Transform ignoredNode)
    {
        Transform nextNode = nodes[Random.Range(0, nodes.Count)];

        while (nextNode == ignoredNode)
        {
            nextNode = nodes[Random.Range(0, nodes.Count)];
        }

        return nextNode;
    }
}
