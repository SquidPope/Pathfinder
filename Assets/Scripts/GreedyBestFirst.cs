using System.Collections.Generic;
using UnityEngine;

public class GreedyBestFirst : MonoBehaviour
{
    /*
     * Like A* but only cares about heuristic
     * 
     */

    List<Node> openList;
    List<Node> closedList;

    public Node start;
    public Node goal;

    static GreedyBestFirst instance;
    public static GreedyBestFirst Instance
    {
        get
        {
            if (instance == null)
            {
                GameObject go = GameObject.FindGameObjectWithTag("GreedyBestFirst");
                instance = go.GetComponent<GreedyBestFirst>();
            }

            return instance;
        }
    }

    void Start()
    {
        closedList = new List<Node>();
        openList = new List<Node>();
    }

    public void FindPath()
    {

        if (start == null)
            start = NodeManager.Instance.GetNode(0, 0);

        if (goal == null)
            goal = NodeManager.Instance.GetNode(2, 3);

        Node current = start;
        bool pathFound = false;

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
                        openList.Add(n);

                    if (n.h <= 0)
                    {
                        n.h = GetHeuristic(n, goal);
                    }
                }
            }

            Node lowestH = GetLowestH();

            if (lowestH != null)
            {
                lowestH.PreviousNode = current;
                current = lowestH;
            }

        } while (openList.Count > 0);

        //highlight the calculated path
        while (current != start && current.PreviousNode != null)
        {
            if (current == null)
                break;

            if (current != goal)
                current.ChangeColor(pathFound);

            current = current.PreviousNode;
        }
    }

    Node GetLowestH()
    {
        if (openList.Count <= 0)
            return null;

        Node lowestH = openList[0];
        if (openList.Count > 0)
        {
            for (int i = 0; i > openList.Count; i++)
            {
                if (openList[i].h < lowestH.h)
                    lowestH = openList[i];
            }
        }

        return lowestH;
    }

    int GetHeuristic(Node current, Node goal)
    {
        float x = Mathf.Abs(current.GetPosition().x - goal.GetPosition().x);
        float y = Mathf.Abs(current.GetPosition().y - goal.GetPosition().y);

        //To scale use d * (d*x + d*y) where d is scale.
        return Mathf.RoundToInt(x + y);
    }

    public void Reset(bool keepMap)
    {
        for (int i = 0; i < closedList.Count; i++)
        {
            if (keepMap)
            {
                if (closedList[i] == start || closedList[i] == goal)
                    continue;
            }
            closedList[i].Reset(keepMap);
        }

        openList.Clear();
        closedList.Clear();
    }
}
