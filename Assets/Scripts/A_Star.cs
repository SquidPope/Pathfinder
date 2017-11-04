using System.Collections.Generic;
using UnityEngine;

public class A_Star : MonoBehaviour
{
    //ToDo: Make sure we test for startNode == goalNode and just exit (in case I decide to choose starts and goals on the fly with a click)

    bool pathFound = false; //limit to once per run for now.

    List<Node> openList;
    List<Node> closedList;

    //ToDo: have NodeManager do this instead.
    void Start()
    {
        //ToDo: generate a map? (find a way to prevent it from making impossible ones?)
        openList = new List<Node>();
        closedList = new List<Node>();
    }

    void FindPath() //start and goal should probably be node objects and not just positions
    {
        
        //ToDo: remove hardcoding
        Node start = NodeManager.Instance.GetNode(0, 0);
        start.ChangeColor(false, true, false);
        Node goal = NodeManager.Instance.GetNode(2, 3);
        goal.ChangeColor(false, false, true);

        Node current = start;

        do
        {
            if (openList.Contains(current))
                openList.Remove(current);

            closedList.Add(current);

            List<Node> adjacentNodes;
            if (current.AdjacentNodes.Count > 0)
            {
                adjacentNodes = current.AdjacentNodes;
            }
            else
            {
                adjacentNodes = NodeManager.Instance.GetAdjacentNodes(current);
                current.AdjacentNodes = adjacentNodes;
            }

            if (adjacentNodes.Contains(goal))
            {
                goal.PreviousNode = current;
                current = goal;
                closedList.Add(current);
            }
            else
            {
                foreach (Node n in adjacentNodes)
                {
                    //ToDo: walkable check
                    if (n == null)
                        Debug.Log("broken foreach loop");

                    n.G = closedList.Count;
                    n.H = GetHeuristic(n, goal);
                    n.F = n.G + n.H;

                    if (!openList.Contains(n))
                    {
                        openList.Add(n);
                    }
                    //else check if f is lower with the current path
                    
                }

                //find node in openList where n.F is lowest
                Node lowestF = openList[0];
                for (int i = 1; i > openList.Count; i++)
                {
                    if (openList[i].F < lowestF.F)
                        lowestF = openList[i];
                }

                lowestF.PreviousNode = current;
                current = lowestF;
            }

        } while (openList.Count > 0 && !closedList.Contains(goal)); //openList having a count <= 0 means there isn't a path (or I've really screwed something)

        //highlight the calculated path
        /*
        foreach (Node node in closedList)
        {
            node.ChangeColor(true, false, false);
        }
        */
        while (current != start && current.PreviousNode != null)
        {
            current.ChangeColor(true, false, false);
            current = current.PreviousNode;
        }

        //should this function move the "character" or should it just return the path?
    }

    int GetHeuristic(Node current, Node goal)
    {
        float x = Mathf.Abs(current.GetPosition().x - goal.GetPosition().x);
        float y = Mathf.Abs(current.GetPosition().y - goal.GetPosition().y);

        //To scale use d * (d*x + d*y) where d is scale.
        return Mathf.RoundToInt(x + y);
    }

    void Update()
    {
        if (pathFound)
            return;

        if(Input.GetKeyUp(KeyCode.Space))
        {
            FindPath();
            pathFound = true;
        }
    }
}
