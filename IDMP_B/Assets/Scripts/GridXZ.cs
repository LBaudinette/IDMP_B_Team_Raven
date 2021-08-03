using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using CodeMonkey.Utils;

public class GridXZ
{
    private int width;
    private int height;
    private float cellSize;
    private int[,] gridArray;
    private TextMesh[,] debugGrid;

    // Start is called before the first frame update
    public GridXZ(int width, int height, float cellSize) {
        this.width = width;
        this.height = height;
        this.cellSize = cellSize;

        gridArray = new int[width, height];
        debugGrid = new TextMesh[width, height];

        //Draw Grid
        for(int row = 0; row < height; row++) {
            for(int col = 0; col < width; col++) {
                //Draw line upwards
                Debug.DrawLine(GetWorldPos(col,row), GetWorldPos(col, row + 1), Color.white, 100f);

                //Draw line to right
                Debug.DrawLine(GetWorldPos(col, row), GetWorldPos(col + 1, row), Color.white, 100f);

                

                debugGrid[col,row] = UtilsClass.CreateWorldText(gridArray[col, row].ToString(), null, GetWorldPos(col,row) + new Vector3(cellSize, 0, cellSize) * 0.5f, 5, Color.red, TextAnchor.MiddleCenter);
                debugGrid[col, row].characterSize = 0.03f;
                debugGrid[col, row].fontSize = 150;


            }
        }
        Debug.DrawLine(GetWorldPos(0,height), GetWorldPos(width, height), Color.white, 100f);
        Debug.DrawLine(GetWorldPos(width, 0), GetWorldPos(width, height), Color.white, 100f);

        SetValue(2, 1, 69);
    }

    //Returns the world position of a cell from the grid
    private Vector3 GetWorldPos(int x, int y) {
        return new Vector3(x, 0, y) * cellSize;
    }

    //returns the cell according to a world position
    private Vector3 GetXZ(Vector3 worldPos) {
        //A worldPos of 8 and a cellSize of 3 would return the second cell as 8/3 = 2.66
        return new Vector3(Mathf.FloorToInt(worldPos.x / cellSize), 0, Mathf.FloorToInt(worldPos.z / cellSize));
    }

    public void SetValue(int x, int y, int value) {
        if(x >= 0 && y >= 0 && x <= width && y <= height){
            gridArray[x, y] = value;
            debugGrid[x, y].text = gridArray[x, y].ToString();
        }
    }

    public void SetValue(Vector3 worldPos, int value) {
        Vector3 gridPos = GetXZ(worldPos);
        SetValue((int)gridPos.x, (int)gridPos.z, value);
        Debug.Log(gridPos);
    }

}
