using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class BuildingScript : MonoBehaviour {
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

        if (parentGrid.GetGridObject(
            (int)originPos.x, (int)originPos.z).secondaryBuilding?.CompareTag("Iron Node") ?? false) {
            output.Add(LevelManager.ResourceType.Iron);
        } else if(parentGrid.GetGridObject(
            (int)originPos.x, (int)originPos.z).secondaryBuilding?.CompareTag("Mineral Node") ?? false) {
            output.Add(LevelManager.ResourceType.Mineral);
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

    //This loops through all pipes connected to the staging grounds and returns any outputs
    public List<LevelManager.ResourceType> CheckAdjacent(List<int> visitedIDs) {
        Debug.Log("Check adjacent called");
        List<GameObject> adjacentCells = getAdjacentObjects().ToList();
        List<LevelManager.ResourceType> resources = new List<LevelManager.ResourceType>();
        resources.AddRange(output);
        visitedIDs.Add(gameObject.GetInstanceID());

        if (gameObject.CompareTag("Harvester"))
            return resources;
        else {
            foreach (GameObject building in adjacentCells) {
                if (visitedIDs.Contains(building.GetInstanceID()) || building.CompareTag("Rock"))
                    continue;
                resources.AddRange(building.GetComponent<BuildingScript>().CheckAdjacent(visitedIDs));
            }
            return resources;
        } 
    }

    protected IEnumerable<GameObject> getAdjacentObjects() {
        List<GameObject> adjacentBuildings = new List<GameObject>();
        GridObject up = parentGrid.GetGridObject((int)originPos.x, (int)originPos.z + 1);
        GridObject right = parentGrid.GetGridObject((int)originPos.x + 1, (int)originPos.z);
        GridObject down = parentGrid.GetGridObject((int)originPos.x, (int)originPos.z - 1);
        GridObject left = parentGrid.GetGridObject((int)originPos.x - 1, (int)originPos.z);

        if (up != default && up.primaryBuilding != null) {
            adjacentBuildings.Add(up.primaryBuilding);
        }
        if (right != default && right.primaryBuilding != null) {
            adjacentBuildings.Add(right.primaryBuilding);
        }
        if (down != default && down.primaryBuilding != null) {
            adjacentBuildings.Add(down.primaryBuilding);

        }
        if (left != default && left.primaryBuilding != null) {
            adjacentBuildings.Add(left.primaryBuilding);

        }

        return adjacentBuildings;
    }
}


