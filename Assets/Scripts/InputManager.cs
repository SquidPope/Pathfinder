using UnityEngine;
using UnityEngine.UI;

public class InputManager : MonoBehaviour
{
    //ToDo: some way to let the user place walls on the map?
    //ToDo: some way to let the user place the start and end points on the map

    [SerializeField]
    Toggle KeepMap;

	void Update()
    {
		if (Input.GetMouseButtonUp(0))
        {
            RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero, 0f);
            if (hit.collider != null)
            {
                if (A_Star.Instance.start != null)
                    A_Star.Instance.start.ChangeColor(Color.white);

                Node node = hit.collider.gameObject.GetComponent<Node>();
                node.Walkable = true;
                node.ChangeColor(Color.blue);
                A_Star.Instance.start = node;
            }
        }

        if (Input.GetMouseButtonUp(1))
        {
            RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero, 0f);
            if (hit.collider != null)
            {
                if (A_Star.Instance.goal != null)
                    A_Star.Instance.goal.ChangeColor(Color.white);

                Node node = hit.collider.gameObject.GetComponent<Node>();
                node.Walkable = true;
                node.ChangeColor(Color.blue);
                A_Star.Instance.goal = node;
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

    //ToDo: option to remove walls
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
}
