using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class ConveyorScript : BuildingScript
{
    //Change the gameobject depending on the rotation

    //Reference to building script that tracks rotation
    //private BuildingScript buildingScript;
    //private GridBuilder parentGrid;
    //Gameobjects for visuals. These a rotated depending on currentDir, and changed
    //depending on how many adjacent conveyors there are
    public GameObject horizontal;
    public GameObject LShape;
    public GameObject threeWay;
    public GameObject fourWay;

    public LinkedList<BuildingScript> adjacentBuildings; //Holds adjacent buildings
    // Start is called before the first frame update
    void Start()
    {
        //buildingScript = GetComponent<BuildingScript>();
        //parentGrid = GameObject.FindWithTag("Grid").GetComponent<GridBuilder>() ;
        //if(parentGrid == null) {
        //    Debug.Log("NOT FOUND");

        //}
        //else {
        //    Debug.Log("FOUND");
        //}
    }

    // Update is called once per frame
    void Update()
    {
        //Every frame check adjacent cells for conveyor belts
        //Change model and output of building script depending on direction
        //checkAdjacentCells();
        //if (Input.GetButtonDown("p")) {
        //    foreach(var element in buildingScript.output) {
        //        Debug.Log($"ELEMENT: {element}");
        //    }
        //}
    }

    //private void checkAdjacentCells() {
    //    //Check cells to the left, right, up, and down
    //    //If they have the "conveyor" tag, add it to the output hashset
    //    //Debug.Log("NAME: " + parentGrid.GetGridObject(1,1).CanBuild());
    //    GridObject up = parentGrid.GetGridObject((int)originPos.x, (int)originPos.z + 1);
    //    GridObject right = parentGrid.GetGridObject((int)originPos.x + 1, (int)originPos.z);
    //    GridObject down = parentGrid.GetGridObject((int)originPos.x, (int)originPos.z - 1);
    //    GridObject left = parentGrid.GetGridObject((int)originPos.x - 1, (int)originPos.z);


    //    adjacentBuildings.Clear();

    //    if(up != default && up.primaryBuilding != null) {
    //        //buildingScript.output.AddRange(up.primaryBuilding.GetComponent<BuildingScript>().output);
    //        adjacentBuildings.AddLast(up.primaryBuilding.GetComponent<BuildingScript>());
    //    } 
    //    if (right != default && right.primaryBuilding != null) {
    //        //buildingScript.output.AddRange(right.primaryBuilding.GetComponent<BuildingScript>().output);
    //        adjacentBuildings.AddLast(right.primaryBuilding.GetComponent<BuildingScript>());

    //    }
    //    if (down != default && down.primaryBuilding != null) {
    //        //buildingScript.output.AddRange(down.primaryBuilding.GetComponent<BuildingScript>().output);
    //        adjacentBuildings.AddLast(down.primaryBuilding.GetComponent<BuildingScript>());

    //    }
    //    if (left != default && left.primaryBuilding != null) {
    //        //buildingScript.output.AddRange(left.primaryBuilding.GetComponent<BuildingScript>().output);
    //        adjacentBuildings.AddLast(left.primaryBuilding.GetComponent<BuildingScript>());

    //    }

    //    //for each resource in adjacent conveyor
    //    //add to current resources
    //}

    //A recursive
    public void updateOutput() {
        //List<>
        //Debug.Log("NAME: " + parentGrid.GetGridObject(1,1).CanBuild());
        GridObject up = parentGrid.GetGridObject((int)originPos.x, (int)originPos.z + 1);
        GridObject right = parentGrid.GetGridObject((int)originPos.x + 1, (int)originPos.z);
        GridObject down = parentGrid.GetGridObject((int)originPos.x, (int)originPos.z - 1);
        GridObject left = parentGrid.GetGridObject((int)originPos.x - 1, (int)originPos.z);

        if (up != default && up.primaryBuilding != null) {
            
        }
        if (right != default && right.primaryBuilding != null) {
            

        }
        if (down != default && down.primaryBuilding != null) {
            adjacentBuildings.AddLast(down.primaryBuilding.GetComponent<BuildingScript>());

        }
        if (left != default && left.primaryBuilding != null) {
            adjacentBuildings.AddLast(left.primaryBuilding.GetComponent<BuildingScript>());

        }
        List<(int, int)> visited = new List<(int, int)>();


    }

    



}
