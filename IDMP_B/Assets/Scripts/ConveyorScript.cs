using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class ConveyorScript : BuildingScript
{
    //Change the gameobject depending on the rotation

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
        currentRotation = LShape;
    }

    // Update is called once per frame
    void Update()
    {
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


        //Check if adjacent cells have a primary building to attach to
        
        //isTop = up != default ? up.hasPrimary() : false;
        //isRight = right != default ? right.hasPrimary() : false;
        //isBottom = down != default ? down.hasPrimary() : false ;
        //isLeft = left != default ? left.hasPrimary() : false;

        isTop = checkValidCell(up);
        isRight = checkValidCell(right);
        isBottom = checkValidCell(down);
        isLeft = checkValidCell(left);

        //Debug.Log($"isTop: {isTop}, isRight: {isRight}, isLeft: {isLeft}, isBottom: {isBottom}");

        //Cross conveyor
        if (isBottom && isTop && isLeft && isRight) {
            fourWay.SetActive(true);
            currentRotation = fourWay;
        }
        else if (isTop && isLeft && isRight) {
            threeWay.SetActive(true);
            currentRotation = threeWay;
        }
        else if (isBottom && isLeft && isRight) { //Three directional conveyors
            threeWay.SetActive(true);
            threeWay.transform.rotation = Quaternion.Euler(transform.rotation.x, 180, transform.rotation.y);
            currentRotation = threeWay;
        }
        else if (isTop && isLeft && isBottom) {
            threeWay.SetActive(true);
            threeWay.transform.rotation = Quaternion.Euler(transform.rotation.x, 270, transform.rotation.y);
            currentRotation = threeWay;
        }
        else if (isTop && isRight && isBottom) {
            threeWay.SetActive(true);
            threeWay.transform.rotation = Quaternion.Euler(transform.rotation.x, 90, transform.rotation.y);
            currentRotation = threeWay;
        }
        else if (isTop && isLeft) {//L shaped conveyors
            LShape.SetActive(true);
            currentRotation = LShape;
        } 
        else if(isTop && isRight) {
            LShape.SetActive(true);
            LShape.transform.rotation = Quaternion.Euler(transform.rotation.x, 90, transform.rotation.y);
            currentRotation = LShape;
        }
        else if (isBottom && isRight) {
            LShape.SetActive(true);
            LShape.transform.rotation = Quaternion.Euler(transform.rotation.x, 180, transform.rotation.y);
            currentRotation = LShape;
        }
        else if (isBottom && isLeft) {
            LShape.SetActive(true);
            LShape.transform.rotation = Quaternion.Euler(transform.rotation.x, 270, transform.rotation.y);
            currentRotation = LShape;
        }
        else if(isBottom || isTop) {
            horizontal.SetActive(true);
            currentRotation = horizontal;
        }
        else if(isLeft || isRight) {
            horizontal.SetActive(true);
            horizontal.transform.rotation = Quaternion.Euler(transform.rotation.x, 90, transform.rotation.y);
            currentRotation = horizontal;
        }
        else {//if placed on a standalone square, just enable the cross
            fourWay.SetActive(true);
            currentRotation = fourWay;
        }


    }

   //Checks if there are any other buildings or converyors to attach to
    private bool checkValidCell(GridObject gridObj) {
        return gridObj != default && gridObj.hasPrimary() && !gridObj.primaryBuilding.CompareTag("Rock");
    }



}
