using UnityEngine;

public class InputManager : MonoBehaviour
{
    //ToDo: some way to let the user place walls on the map?
    //ToDo: some way to let the user place the start and end points on the map

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

        if (Input.GetKeyUp(KeyCode.Escape))
            Application.Quit();
	}

    //ToDo: option to remove walls
    public void Reset()
    {
        A_Star.Instance.Reset();
        GreedyBestFirst.Instance.Reset();
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
