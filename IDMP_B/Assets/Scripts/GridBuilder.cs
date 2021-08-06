using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CodeMonkey.Utils;

public class GridBuilder : MonoBehaviour {
    private GridXZ<GridObject> grid;
    [SerializeField] private List<BuildingSO> buildingList;
    private BuildingSO currentBuilding;

    //The current direction that the building we want to build is facing
    private BuildingSO.Direction currentDirection = BuildingSO.Direction.Down;

    [SerializeField] private int width = 10;
    [SerializeField] private int height = 10;
    [SerializeField] private float cellSize = 1.0f;

    // Start is called before the first frame update
    void Start() {
        width = 10;
        height = 10;
        cellSize = 1.0f;
        grid = new GridXZ<GridObject>(width, height, cellSize, transform.position, (gridXZ, x, y) => new GridObject(gridXZ, x, y));
        currentBuilding = buildingList[0];

        grid.OnGridValueChanged += Grid_OnGridValueChanged;
    }

    private void Grid_OnGridValueChanged() {

    }

    // Update is called once per frame
    void Update() {
        Vector3 mousePos = grid.getMouseWorldPos();

        if (Input.GetKeyDown("1")) {
            currentBuilding = buildingList[0];

            UtilsClass.CreateWorldTextPopup(currentBuilding.name, mousePos, 2);

        }
        else if (Input.GetKeyDown("2")) {
            currentBuilding = buildingList[1];

            UtilsClass.CreateWorldTextPopup(currentBuilding.name, mousePos, 2);

        }
        else if (Input.GetKeyDown("3")) {
            currentBuilding = buildingList[2];

            UtilsClass.CreateWorldTextPopup(currentBuilding.name, mousePos, 2);

        }


        if (Input.GetMouseButtonDown(0)) {
            bool canBuild = true;
            Vector3 gridCellIndices = grid.GetXZCell(mousePos);


            Vector3 rotationOffset = currentBuilding.GetAnglePosOffset(currentDirection);
            Vector3 buildingSpawnPos =
                grid.GetWorldPos((int)gridCellIndices.x, (int)gridCellIndices.z) + rotationOffset * cellSize;

            GridObject gridObject = grid.GetGridObject(mousePos);

            List<Vector3> occupiedGridCells = currentBuilding.GetGridPositionList(gridCellIndices, currentDirection);

            foreach (Vector3 gridPos in occupiedGridCells) {
                if (!grid.GetGridObject((int)gridPos.x, (int)gridPos.z).CanBuild()) {
                    canBuild = false;
                    break;
                }

            }

            if (canBuild) {
                GameObject buildingObj =
                    Instantiate(
                        currentBuilding.buildingPrefab,
                        buildingSpawnPos,
                        Quaternion.Euler(0, BuildingSO.GetDirectionAngle(currentDirection),
                        0));

                foreach (Vector3 gridPos in occupiedGridCells) {
                    //Set the building occupying the grid objects to the one we just instantiated
                    grid.GetGridObject((int)gridPos.x, (int)gridPos.z).setBuilding(buildingObj);
                }

                gridObject.setBuilding(buildingObj);
            }
            else {
                UtilsClass.CreateWorldTextPopup("Cannot build here!", mousePos);
            }


        }
        else if (Input.GetMouseButtonDown(1)) {
            currentDirection = BuildingSO.GetNextDirection(currentDirection);
            UtilsClass.CreateWorldTextPopup(currentDirection.ToString(), mousePos, 2);
        }



    }




}

