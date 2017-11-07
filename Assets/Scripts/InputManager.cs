using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{

	void Update()
    {
		if (Input.GetMouseButtonUp(0))
        {
            //raycast?
            //get node clicked on
            //tell node to tell adjacent nodes to change color
        }

        if (Input.GetKeyUp(KeyCode.Escape))
            Application.Quit();
	}
}
