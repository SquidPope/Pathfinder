using System.Collections.Generic;
using UnityEngine;

public class A_Star : MonoBehaviour
{
    //ToDo: Make sure we test for startNode == goalNode and just exit (in case I decide to choose starts and goals on the fly with a click)

    //If we race algorithms, we should have a 'timer' showing how many miliseconds (and steps) each takes, as well as the final path
    //Add a way to see all the nodes that were checked as well as the final path.

    List<Node> openList;
    List<Node> closedList;

    public Node start;
    public Node goal;

    static A_Star instance;
    public static A_Star Instance
    {
        get
        {
            if (instance == null)
            {
                GameObject go = GameObject.FindGameObjectWithTag("A_Star");
                instance = go.GetComponent<A_Star>();
            }

            return instance;
        }
    }

    void Start()
    {
        openList = new List<Node>();
        closedList = new List<Node>();

    }

    void FindPath() //start and goal should probably be node objects and not just positions
    {
        if (start == null)
            start = NodeManager.Instance.GetNode(0, 0);

        if (goal == null)
            goal = NodeManager.Instance.GetNode(2, 3);

        start.ChangeColor(Color.blue);

        Node current = start;
        int nodesTraveled = 0;
        bool pathFound = false;

        int failsafe = 0;
        do
        {
            Node lowestF = GetNodeWithLowestF();
            nodesTraveled++;

            if (lowestF != null)
                current = lowestF;

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
                pathFound = true;
                break;
            }
            else
            {
                foreach (Node n in adjacentNodes)
                {
                    if (closedList.Contains(n))
                        continue;

                    if (!n.Walkable)
                    {
                        closedList.Add(n);
                        continue;
                    }

                    if (!openList.Contains(n))
                    {
                        openList.Add(n);
                        n.g = current.g + 1;
                        n.h = GetHeuristic(n, goal);

                        n.f = n.h + n.g;
                        n.PreviousNode = current;
                    }
                    else
                    {
                        //Using n's H value here because the heuristic shouldn't change.
                        int f = n.h + current.g + 1;
                        if (n.f >= f)
                        {
                            n.PreviousNode = current;
                            n.g = current.g + 1;
                            n.f = f;
                        }
                        else
                        {
                            //if n.F < f it means the other path to this node is better.
                            Debug.Log("alternate path better");
                        }

                    } 
                }
            }
            failsafe++;

        } while (openList.Count > 0); //openList having a count <= 0 means there isn't a path (or I've really screwed something)

        //highlight the calculated path
        while (current != start && current.PreviousNode != null)
        {
            if (current == null)
                break;

            current.ChangeColor(pathFound);
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

    Node GetNodeWithLowestF()
    {
        if (openList.Count <= 0)
            return null;

        Node lowestF = openList[0];
        if (openList.Count > 0)
        {
            for (int i = 0; i > openList.Count; i++)
            {
                if (openList[i].f < lowestF.f)
                    lowestF = openList[i];
            }
        }

        return lowestF;
    }

    void Update()
    {
        //ToDo: decide if we want to limit how frequently this can be called.
        if(Input.GetKeyUp(KeyCode.Space))
        {
            FindPath();
        }
    }
}
