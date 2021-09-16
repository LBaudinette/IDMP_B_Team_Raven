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

        if (parentGrid.GetGridObject((int)originPos.x, (int)originPos.z).secondaryBuilding?.CompareTag("Iron Node") ?? false) {
            output.Add(LevelManager.ResourceType.Iron);
            output.Add(LevelManager.ResourceType.Copper);
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

    public List<LevelManager.ResourceType> CheckAdjacent(List<int> visitedIDs) {

        //newResources.AddRange(buildingScript.output);
        //Get surrounding pipes
        //go to first pipe found
        //call checkAdjacent()
        List<GameObject> adjacentCells = getAdjacentObjects().ToList();
        List<LevelManager.ResourceType> resources = new List<LevelManager.ResourceType>();
        resources.AddRange(output);
        visitedIDs.Add(gameObject.GetInstanceID());

        foreach (GameObject building in adjacentCells) {
            if (visitedIDs.Contains(building.GetInstanceID()))
                continue;
            resources.AddRange(building.GetComponent<BuildingScript>().CheckAdjacent(visitedIDs));
        }

        //Debug.Log("RECUR");
        return resources;
    }

    protected IEnumerable<GameObject> getAdjacentObjects() {
        //Create list of adjacent buildings. Removes null at end
        return new List<GameObject> {
            parentGrid.GetGridObject((int)originPos.x, (int)originPos.z + 1).primaryBuilding,
            parentGrid.GetGridObject((int)originPos.x + 1, (int)originPos.z).primaryBuilding,
            parentGrid.GetGridObject((int)originPos.x, (int)originPos.z - 1).primaryBuilding,
            parentGrid.GetGridObject((int)originPos.x - 1, (int)originPos.z).primaryBuilding
        }.Where(o => o != null);
    }
}


