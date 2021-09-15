using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConveyorScript : MonoBehaviour
{
    //Change the gameobject depending on the rotation

    //Reference to building script that tracks rotation
    private BuildingScript buildingScript;
    private GridXZ<GridObject> parentGrid;
    //Gameobjects for visuals. These a rotated depending on currentDir, and changed
    //depending on how many adjacent conveyors there are
    public GameObject horizontal;
    public GameObject LShape;
    public GameObject threeWay;
    public GameObject fourWay;

    // Start is called before the first frame update
    void Start()
    {
        buildingScript = GetComponent<BuildingScript>();
        if(buildingScript == null) {
            Debug.Log("NO SCRIPT");
        }
        parentGrid = buildingScript.GetGrid();
    }

    // Update is called once per frame
    void Update()
    {
        //Every frame check adjacent cells for conveyor belts
        //Change model and output of building script depending on direction
        checkAdjacentCells();
        if (Input.GetButtonDown("p")) {
            foreach(var element in buildingScript.output) {
                Debug.Log($"ELEMENT: {element}");
            }
        }
    }

    private void checkAdjacentCells() {
        //Check cells to the left, right, up, and down
        //If they have the "conveyor" tag, add it to the output hashset
        Vector3 origin = buildingScript.GetOrigin();
        //Debug.Log("NAME: " + parentGrid.GetGridObject(1,1).CanBuild());
        GridObject up = parentGrid.GetGridObject((int)origin.x, (int)origin.z + 1);
        GridObject right = parentGrid.GetGridObject((int)origin.x + 1, (int)origin.z);
        GridObject down = parentGrid.GetGridObject((int)origin.x, (int)origin.z - 1);
        GridObject left = parentGrid.GetGridObject((int)origin.x - 1, (int)origin.z);


        if(up != default) {


        } 
        if (right != default && gameObject.CompareTag("Conveyor")) {

        }
        if (down != default && gameObject.CompareTag("Conveyor")) {

        }
        if (left != default && gameObject.CompareTag("Conveyor")) {

        }

        //for each resource in adjacent conveyor
        //add to current resources
    }
}
