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

    private GameObject currentRotation;

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
        changeVisual();
    }

    private void changeVisual() {
        currentRotation.SetActive(false);
        bool isTop, isRight, isLeft, isBottom;

        GridObject up = parentGrid.GetGridObject((int)originPos.x, (int)originPos.z + 1);
        GridObject right = parentGrid.GetGridObject((int)originPos.x + 1, (int)originPos.z);
        GridObject down = parentGrid.GetGridObject((int)originPos.x, (int)originPos.z - 1);
        GridObject left = parentGrid.GetGridObject((int)originPos.x - 1, (int)originPos.z);

        //if(top has primary building)
        //flag top
        //flag directions

        //Check if adjacent cells have a primary building to attach to
        isTop = up.hasPrimary();
        isRight = right.hasPrimary();
        isBottom = down.hasPrimary();
        isLeft = left.hasPrimary();

        //Cross conveyor
        if (isBottom && isTop && isLeft && isRight) {
            fourWay.SetActive(true);
        }
        else if (isTop && isLeft && isRight) {
            threeWay.SetActive(true);
        }
        else if (isBottom && isLeft && isRight) { //Three directional conveyors
            threeWay.SetActive(true);
            threeWay.transform.rotation = Quaternion.Euler(transform.rotation.x, 180, transform.rotation.y);
        }
        else if (isTop && isLeft && isBottom) {
            threeWay.SetActive(true);
            threeWay.transform.rotation = Quaternion.Euler(transform.rotation.x, 270, transform.rotation.y);
        }
        else if (isTop && isRight && isBottom) {
            threeWay.SetActive(true);
            threeWay.transform.rotation = Quaternion.Euler(transform.rotation.x, 90, transform.rotation.y);
        }
        else if (isTop && isLeft) {//L shaped conveyors
            LShape.SetActive(true);
        } 
        else if(isTop && isRight) {
            LShape.SetActive(true);
            LShape.transform.rotation = Quaternion.Euler(transform.rotation.x, 90, transform.rotation.y);
        }
        else if (isBottom && isRight) {
            LShape.SetActive(true);
            LShape.transform.rotation = Quaternion.Euler(transform.rotation.x, 180, transform.rotation.y);
        }
        else if (isBottom && isLeft) {
            LShape.SetActive(true);
            LShape.transform.rotation = Quaternion.Euler(transform.rotation.x, 270, transform.rotation.y);
        } 
        else if(isBottom && isTop) {
            horizontal.SetActive(true);
        } 
        else if(isLeft && isRight) {
            horizontal.SetActive(true);
            horizontal.transform.rotation = Quaternion.Euler(transform.rotation.x, 90, transform.rotation.y);
        }


    }

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
