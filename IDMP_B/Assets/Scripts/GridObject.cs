using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CodeMonkey.Utils;


public class GridObject
{
        private int x, z;
        GridXZ<GridObject> grid;
        private GameObject building;


        public GridObject(GridXZ<GridObject> grid, int x, int z) {
            this.grid = grid;
            this.x = x;
            this.z = z;
        }

        public void setBuilding(GameObject newBuilding) {
            building = newBuilding;
            grid.TriggerGridObjectChanged();
        }

        //Demolish a building
        public void ClearTransform() {
        building = null;
            grid.TriggerGridObjectChanged();

        }

        public bool CanBuild() {
            return building == null;
        }

        public override string ToString() {
            return $"{x}, {z}";
        }
    

}
