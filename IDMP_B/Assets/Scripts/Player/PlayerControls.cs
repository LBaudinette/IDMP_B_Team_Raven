using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControls : MonoBehaviour
{
    int xPos;
    int zPos;
    private GridXZ<GridObject> grid;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void CheckMousePos()
    {
        // get mouse position on grid
        Vector3 mousePos = grid.getMouseWorldPos();

        // if player inputs left click to move
        if (Input.GetMouseButtonDown(0))
        {
            //if grid node at mouse position is adjacent to player's current position
            if (true)
            {
                //update player's position to adjacent grid node
                
            }
        }
    }
}
