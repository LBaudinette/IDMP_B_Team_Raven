using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingScript : MonoBehaviour
{
    protected BuildingSO buildingTypeSO;
    protected Vector3 originPos;   //Stores the origin indices in the grid 
    protected BuildingSO.Direction currentDir;
    protected GridXZ<GridObject> parentGrid;

    public List<LevelManager.ResourceType> output;

    public List<GridObject> adjacentObjects;
    public void InitValues(BuildingSO buildingSO, Vector3 origin, BuildingSO.Direction dir, GridXZ<GridObject> grid) {
        buildingTypeSO = buildingSO;
        originPos = origin;
        currentDir = dir;
        parentGrid = grid;
        output = new List<LevelManager.ResourceType>();

        if (parentGrid.GetGridObject((int)originPos.x, (int)originPos.z).secondaryBuilding.CompareTag("Iron Node")) {
            output.Add(LevelManager.ResourceType.Iron);
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


