using UnityEngine;
using UnityEngine.UI;

public enum NodeType { empty, start, goal, slow, wall }
public class InputManager : MonoBehaviour
{
    [SerializeField]
    Toggle KeepMap;

    public Node start;
    public Node goal;

    static InputManager instance;
    public static InputManager Instance
    {
        get
        {
            if (instance == null)
            {
                GameObject go = GameObject.FindGameObjectWithTag("InputManager");
                instance = go.GetComponent<InputManager>();
            }

            return instance;
        }
    }

	void Update()
    {
		if (Input.GetMouseButtonUp(0))
        {
            RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero, 0f);
            if (hit.collider != null)
            {
                if (start != null)
                    ChangeNodeType(start, NodeType.empty);

                Node node = hit.collider.gameObject.GetComponent<Node>();
                ChangeNodeType(node, NodeType.start);
            }
        }

        if (Input.GetMouseButtonUp(1))
        {
            RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero, 0f);
            if (hit.collider != null)
            {
                if (goal != null)
                    ChangeNodeType(goal, NodeType.empty);

                Node node = hit.collider.gameObject.GetComponent<Node>();
                ChangeNodeType(node, NodeType.goal);
            }
        }

        if (Input.GetKeyUp(KeyCode.X))
        {
            RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero, 0f);
            if (hit.collider != null)
            {
                Node node = hit.collider.gameObject.GetComponent<Node>();
                node.Walkable = !node.Walkable;
            }
        }
        
        //ToDo: Have UI elements to do this
        if (Input.GetKeyUp(KeyCode.C))
        {
            RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero, 0f);
            if (hit.collider != null)
            {
                Node node = hit.collider.gameObject.GetComponent<Node>();
                node.Cost = 1;
            }
        }

        if (Input.GetKeyUp(KeyCode.Z))
        {
            RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero, 0f);
            if (hit.collider != null)
            {
                Node node = hit.collider.gameObject.GetComponent<Node>();
                node.Cost = 5;
            }
        }

        if (Input.GetKeyUp(KeyCode.Escape))
            Application.Quit();
	}

    public void Reset()
    {
        A_Star.Instance.Reset(KeepMap.isOn);
        GreedyBestFirst.Instance.Reset(KeepMap.isOn);
    }

    public void RunAStar()
    {
        A_Star.Instance.FindPath();
    }

    public void RunGreedyBestFirst()
    {
        GreedyBestFirst.Instance.FindPath();
    }

    public void ChangeNodeType(Node node, NodeType type)
    {
        if (node.Type == NodeType.start)
            start = null;
        if (node.Type == NodeType.goal)
            goal = null;

        node.Type = type;
        node.Walkable = true;

        switch (type)
        {
            case NodeType.empty:
                {
                    node.Cost = 1;
                    break;
                }
            case NodeType.goal:
                {
                    goal = node;
                    break;
                }
            case NodeType.slow:
                {
                    node.Cost = 5; //ToDo: remove hardcoding
                    break;
                }
            case NodeType.start:
                {
                    start = node;
                    break;
                }
            case NodeType.wall:
                {
                    node.Walkable = false;
                    break;
                }
            default:
                {
                    break;
                }
        }

        node.ChangeColorByType();
    }
}
