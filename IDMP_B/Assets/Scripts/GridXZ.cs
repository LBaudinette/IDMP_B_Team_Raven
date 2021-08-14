using System.Collections;
using System.Collections.Generic;
using System;
using UnityEditor;
using UnityEngine;
using CodeMonkey.Utils;

public class GridXZ<TGridObject>
{
    private int width;
    private int height;
    private float cellSize;
    private TGridObject[,] gridArray;
    private TextMesh[,] debugGrid;
    private Vector3 gridOrigin;

    public event ValueChangedDelegate OnGridValueChanged;

    public delegate void ValueChangedDelegate();


    // Start is called before the first frame update
    public GridXZ(int width, int height, float cellSize, Vector3 gridOrigin, Func<GridXZ<TGridObject>, int, int, TGridObject> createGridObject) {
        this.width = width;
        this.height = height;
        this.cellSize = cellSize;
        this.gridOrigin = gridOrigin;

        gridArray = new TGridObject[width, height];
        debugGrid = new TextMesh[width, height];

        //Draw and populate Grid
        for(int row = 0; row < height; row++) {
            for(int col = 0; col < width; col++) {

                gridArray[col, row] = createGridObject(this,col, row);

                //Draw line upwards
                Debug.DrawLine(GetWorldPos(col,row), GetWorldPos(col, row + 1), Color.white, 100f);

                //Draw line to right
                Debug.DrawLine(GetWorldPos(col, row), GetWorldPos(col + 1, row), Color.white, 100f);

                

                debugGrid[col,row] = UtilsClass.CreateWorldText(gridArray[col, row]?.ToString(), null, GetWorldPos(col,row) + new Vector3(cellSize, 0, cellSize) * 0.5f, 5, Color.red, TextAnchor.MiddleCenter);
                debugGrid[col, row].characterSize = 0.03f;
                debugGrid[col, row].fontSize = 150;


            }
        }
        Debug.DrawLine(GetWorldPos(0,height), GetWorldPos(width, height), Color.white, 100f);
        Debug.DrawLine(GetWorldPos(width, 0), GetWorldPos(width, height), Color.white, 100f);

    }

    //Returns the world position of a cell from the grid
    public Vector3 GetWorldPos(int x, int y) {
        return new Vector3(x, 0, y) * cellSize + gridOrigin;
    }

    //returns the cell indices in a Vector3 according to a world position
    public Vector3 GetXZCell(Vector3 worldPos) {
        //A worldPos of 8 and a cellSize of 3 would return the second cell as 8/3 = 2.66
        return new Vector3(Mathf.FloorToInt(worldPos.x - gridOrigin.x / cellSize), 
            0, 
            Mathf.FloorToInt(worldPos.z - gridOrigin.z / cellSize)) ;
    }

    public void SetGridObject(int x, int y, TGridObject value) {
        if(x >= 0 && y >= 0 && x <= width && y <= height){
            gridArray[x, y] = value;
            debugGrid[x, y].text = gridArray[x, y]?.ToString();
        }
    }

    public void TriggerGridObjectChanged() {
        OnGridValueChanged?.Invoke();
    }

    public void SetGridObject(Vector3 worldPos, TGridObject value) {
        Vector3 gridPos = GetXZCell(worldPos);
        SetGridObject((int)gridPos.x, (int)gridPos.z, value);
        Debug.Log(gridPos);
    }

    //returns a grid object using an x and y index
    public TGridObject GetGridObject(int x, int y) {
        if (x >= 0 && y >= 0 && x <= width && y <= height) {
            //Debug.Log($"X: {x} + Y: {y}");
            return gridArray[x, y];
        }
        else {
            return default;
        }
    }

    public TGridObject GetGridObject(Vector3 worldPos) {
        Vector3 cellCoord = GetXZCell(worldPos);
        return GetGridObject((int)cellCoord.x, (int)cellCoord.z);
    }

    public Vector3 getMouseWorldPos() {
        int layerMask = 1 << 3;
        layerMask = ~layerMask;

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if(Physics.Raycast(ray, out RaycastHit hit)) {
            return hit.point;
        }
        else {
            return Vector3.zero;
        }
    }

    
}
