using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CodeMonkey.Utils;


public class GridObject {
    public int x, z;
    GridXZ<GridObject> grid;
    private GameObject primaryBuilding;         //Hold buildings
    private GameObject secondaryBuilding;       //Holds level structures such as resource nodes


    public GridObject(GridXZ<GridObject> grid, int x, int z) {
        this.grid = grid;
        this.x = x;
        this.z = z;
    }

    public void setBuilding(GameObject newBuilding) {
       primaryBuilding = newBuilding;
        grid.TriggerGridObjectChanged();
    }

    public void setSecondary(GameObject secondary) {
        secondaryBuilding = secondary;
    }


    //Demolish a building
    public void ClearTransform() {
       primaryBuilding = null;
        grid.TriggerGridObjectChanged();

    }

    public GameObject GetBuildingObject() {
        return primaryBuilding;
    }

    public GameObject GetSecBuildingObject() {
        return secondaryBuilding;
    }


    public bool CanBuild() {
        return primaryBuilding == null && secondaryBuilding == null;
    }

    public override string ToString() {
        return $"{x}, {z}";
    }


}
