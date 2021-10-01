using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CodeMonkey.Utils;


public class GridObject {
    public int x, z;
    GridXZ<GridObject> grid;
    public GameObject? primaryBuilding{ get; set; }          //Hold buildings
    public GameObject? secondaryBuilding { get; set; }       //Holds level structures such as resource nodes


    public GridObject(GridXZ<GridObject> grid, int x, int z) {
        this.grid = grid;
        this.x = x;
        this.z = z;
    }


    //Demolish a building
    public void ClearTransform() {
       primaryBuilding = null;
        grid.TriggerGridObjectChanged();

    }
    public bool CanBuild() {
        return primaryBuilding == null && secondaryBuilding == null;
    }

    //Returns true if the grid object has a primary building
    public bool hasPrimary() {
        return primaryBuilding != null;
    }
    public override string ToString() {
        return $"{x}, {z}";
    }


}
