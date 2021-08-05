using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CodeMonkey.Utils;

public class GridBuilder : MonoBehaviour
{
    private GridXZ<GridObject> grid;
    [SerializeField] private GameObject building;

    // Start is called before the first frame update
    void Start()
    {
        int width = 10;
        int height = 10;
        float cellSize = 2.0f;
        grid = new GridXZ<GridObject>(width, height, cellSize, transform.position, (gridXZ, x, y) => new GridObject(gridXZ, x, y));
        grid.OnGridValueChanged += Grid_OnGridValueChanged;
    }

    private void Grid_OnGridValueChanged(int value) {
        
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 mousePos = grid.getMouseWorldPos();
        if (Input.GetMouseButtonDown(0)) {
            Instantiate(building, grid.GetXZCell(mousePos), Quaternion.identity);
        } else if(Input.GetMouseButtonDown(1)){

        }
    }



    
}

public class GridObject {
    private int x, z;
    GridXZ<GridObject> grid;


    public GridObject(GridXZ<GridObject> grid, int x, int z) {
        this.grid = grid;
        this.x = x;
        this.z = z;
    }

    public override string ToString() {
        return $"{x}, {z}";
    }
}
