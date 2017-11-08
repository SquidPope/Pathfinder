using System.Collections.Generic;
using UnityEngine;

public class Node : MonoBehaviour
{
    bool walkable = true;

    Vector2 mapPos;

    //A*
    public int f = 0;
    public int g = 0;
    public int h = 0;

    List<Node> adjacentNodes;
    Node previousNode;

    Renderer renderer;

    public bool Walkable
    {
        get { return walkable; }
        set
        {
            walkable = value;
            if (walkable)
                ChangeColor(Color.white);
            else
                ChangeColor(Color.gray);
        }
    }

    public Vector2 MapPos
    {
        get { return mapPos; }
        set { mapPos = value; }
    }

    public Node PreviousNode
    {
        get { return previousNode; }
        set { previousNode = value; }
    }

    void Awake()
    {
        renderer = GetComponent<Renderer>();
        //ChangeColor(false, false, false);

        adjacentNodes = new List<Node>();
    }

    //Trying out lazy loading
    public List<Node> AdjacentNodes
    {
        get { return adjacentNodes; }
        set { adjacentNodes = value; }
    }

    public Vector2 GetPosition()
    {
        return transform.position;
    }

    //Raycast for H?

    //Raycast for adjacent Nodes?

    //ToDo: on mouse click, give self to NodeManager as goalNode for a move - NodeManager keeps track of start on its own || on left click self is start?

    //ToDo: have a single enum handle states instead of bools, they're all mutually exclusive.
    public void ChangeColor(bool onPath)
    {
        if (!walkable)
        {
            return;
        }
        else
        {
            if (onPath)
                renderer.material.color = Color.green;
            else
                renderer.material.color = Color.red;
        }
    }

    public void Reset(bool keepWalls)
    {
        if (keepWalls)
        {
            if (walkable)
                renderer.material.color = Color.white;
        }
        else
        {
            walkable = true;
            renderer.material.color = Color.white;
        }
    }

    //ToDo: remove when not needed for debugging
    public void ChangeColor(Color c)
    {
        renderer.material.color = c;
    }
}
