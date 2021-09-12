using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControls : MonoBehaviour
{
    Vector3 pos;
    public GridXZ<GridObject> grid;
    // Start is called before the first frame update
    void Start()
    {
        pos.x = 0;
        pos.y = 0;
        pos.z = 0;
        UpdateGridPos();
    }

    // Update is called once per frame
    void Update()
    {
        CheckMousePos();
    }

    void CheckMousePos()
    {
        // get mouse position on grid
        Vector3 mousePos = grid.getMouseWorldPos();

        // if player inputs left click to move
        if (Input.GetMouseButtonDown(0))
        {
            Vector3 gridPos = grid.GetXZCell(mousePos);
            GridObject node = grid.GetGridObject(mousePos);
            //if grid node at mouse position is adjacent to player's current position
            if (Mathf.Abs(gridPos.x - pos.x) == 1 && Mathf.Abs(gridPos.z - pos.z) == 1)
            {
                //update player's position to adjacent grid node
                pos.x = gridPos.x;
                pos.z = gridPos.z;
                UpdateGridPos();

            }
        }
    }

    void UpdateGridPos()
    {
        // update gameobject transform to (x * cellSize + cellSize / 2, y, z * cellSize + cellSize / 2)
    }
}