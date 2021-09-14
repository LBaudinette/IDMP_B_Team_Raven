using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControls : MonoBehaviour
{
    Vector3 pos;
    Vector3 origin;
    public GridBuilder gb;
    float cellSize;

    // Start is called before the first frame update
    void Start()
    {
        origin = gb.transform.position;
        pos = Vector3.zero;
        cellSize = gb.GetCellSize();
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
        // if player inputs left click to move
        if (Input.GetMouseButtonDown(0))
        {
            Vector3 mousePos = gb.GetMouseWorldPos();
            Vector3 gridPos = gb.GetXZCell(mousePos);
            GridObject gridObj = gb.GetGridObject(mousePos);
            if (gridObj != default)
            {
                //if grid node at mouse position is adjacent to player's current position
                float xDiff = gridPos.x - pos.x;
                float zDiff = gridPos.z - pos.z;
                if ((Mathf.Abs(xDiff) == 1 && zDiff == 0) || (Mathf.Abs(zDiff) == 1 && xDiff == 0))
                {
                    //update player's position to adjacent grid node
                    pos.x = gridPos.x;
                    pos.z = gridPos.z;
                    UpdateGridPos();

                }
            }
        }
    }

    void UpdateGridPos()
    {
        // update gameobject transform to (x * cellSize + cellSize / 2, y, z * cellSize + cellSize / 2)]
        this.gameObject.transform.position = (new Vector3(pos.x, 0, pos.z) + origin) * cellSize + new Vector3(cellSize / 2, 0, cellSize / 2);
    }
}