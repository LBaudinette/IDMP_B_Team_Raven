using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingScript : MonoBehaviour
{
    private BuildingSO buildingTypeSO;
    private Vector3 originPos;   //Stores the origin indices in the grid 
    private BuildingSO.Direction currentDir;
    private GridXZ<GridObject> parentGrid;

    //[SerializeField]
    public HashSet<int> output;    //Use HashSet as it only allows unique elements

    public void InitValues(BuildingSO buildingSO, Vector3 origin, BuildingSO.Direction dir, GridXZ<GridObject> grid) {
        buildingTypeSO = buildingSO;
        originPos = origin;
        currentDir = dir;
        parentGrid = grid;
        output = new HashSet<int>();

        //If this building is on top of a iron node, set its output to iron
        if (parentGrid.GetGridObject(originPos).GetSecBuildingObject().CompareTag("IronNode")){
            output.Add(1);
            Debug.Log("OUTPUT ADDED");
        }
    }

    public void DestroyThis() {
        Debug.Log("DESTROYED");

        Destroy(gameObject);
    }

    public List<Vector3> GetGridPositionList() {
        return buildingTypeSO.GetGridPositionList(originPos, currentDir);
    }

    public GridXZ<GridObject> GetGrid() {
        return parentGrid;
    }

    public Vector3 GetOrigin() {
        return originPos;
    }
}


