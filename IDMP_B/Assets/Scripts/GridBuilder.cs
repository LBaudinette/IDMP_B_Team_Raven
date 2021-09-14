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

    [SerializeField] private Material buildableMat;
    [SerializeField] private Material notBuildableMat;

    private Material currentBuildingMat;
    private GameObject currentGhostBuilding;

    private BuildingMode currentMode = BuildingMode.Idle;

    private Coroutine coroutine;

    private enum BuildingMode {
        Build, Destroy, Idle
    }

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
        if (Input.GetKeyDown(",") && currentMode != BuildingMode.Build) {
            StartBuildMode();
            currentMode = BuildingMode.Build;
            UtilsClass.CreateWorldTextPopup(currentMode.ToString(), mousePos, 2);

        }
        else if (Input.GetKeyDown(".")) {
            StartDestroyMode();
            currentMode = BuildingMode.Destroy;
            UtilsClass.CreateWorldTextPopup(currentMode.ToString(), mousePos, 2);

        }
        else if (Input.GetKeyDown("/")) {
            currentMode = BuildingMode.Idle;
            UtilsClass.CreateWorldTextPopup(currentMode.ToString(), mousePos, 2);

        }


        switch (currentMode) {
            case BuildingMode.Build:
                BuildMode(mousePos);
                break;
            case BuildingMode.Destroy:
                DestroyMode(mousePos);
                break;
            case BuildingMode.Idle:
                break;
        }

    }

    private void StartBuildMode() {
        //Instantiate a ghost version of the current building
        Vector3 mousePos = grid.getMouseWorldPos();
        currentGhostBuilding =
                   Instantiate(
                       currentBuilding.buildingPrefab,
                       mousePos,
                       Quaternion.Euler(0, BuildingSO.GetDirectionAngle(currentDirection),
                       0));
        currentGhostBuilding.GetComponentInChildren<MeshRenderer>().material = buildableMat;
    }

    private void BuildMode(Vector3 mousePos) {

        if (Input.GetKeyDown("1")) {
            CreateNewGhostBuilding(buildingList[0], mousePos);

            UtilsClass.CreateWorldTextPopup(currentBuilding.name, mousePos, 2);

        }
        else if (Input.GetKeyDown("2")) {

            CreateNewGhostBuilding(buildingList[1], mousePos);

            UtilsClass.CreateWorldTextPopup(currentBuilding.name, mousePos, 2);

        }
        else if (Input.GetKeyDown("3")) {
            CreateNewGhostBuilding(buildingList[2], mousePos);

            UtilsClass.CreateWorldTextPopup(currentBuilding.name, mousePos, 2);


        }

        //Make the ghost building follow the mouse
        //currentGhostBuilding.transform.position = mousePos;

        bool canBuild = true;

        //Get the Cell indices in a Vector3 that we want to build in
        Vector3 gridCellIndices = grid.GetXZCell(mousePos);
        Vector3 rotationOffset = currentBuilding.GetAnglePosOffset(currentDirection);
        Vector3 buildingSpawnPos =
                grid.GetWorldPos((int)gridCellIndices.x, (int)gridCellIndices.z) + rotationOffset * cellSize;

        UpdateGhostBuilding(buildingSpawnPos);

        


        GridObject gridObject = grid.GetGridObject(mousePos);

        List<Vector3> occupiedGridCells = currentBuilding.GetGridPositionList(gridCellIndices, currentDirection);

        //Check if we can build in the space that we click on
        foreach (Vector3 gridPos in occupiedGridCells) {
            int x = (int)gridPos.x;
            int y = (int)gridPos.y;

            if (x >= 0 && y >= 0 && x <= width && y <= height 
                && grid.GetGridObject((int)gridPos.x, (int)gridPos.z).CanBuild() && gridObject != default) {
            }
            else {
                canBuild = false;
            }
        }

        if (canBuild) {
            //Debug.Log("CAN BUILD");
            currentGhostBuilding.GetComponentInChildren<MeshRenderer>().material = buildableMat;
            if (Input.GetMouseButtonDown(0)) {
                CreateBuilding(
                        buildingSpawnPos, Quaternion.Euler(0, BuildingSO.GetDirectionAngle(currentDirection), 0), currentBuilding, gridCellIndices);


                //gridObject.setBuilding(buildingObj);
            }

        }
        else {
            currentGhostBuilding.GetComponentInChildren<MeshRenderer>().material = notBuildableMat;
            if (Input.GetMouseButtonDown(0))
                UtilsClass.CreateWorldTextPopup("Cannot build here!", mousePos);


        }

         if (Input.GetMouseButtonDown(1)) {
            currentDirection = BuildingSO.GetNextDirection(currentDirection);
            UtilsClass.CreateWorldTextPopup(currentDirection.ToString(), mousePos, 2);
        } 

    }

    private void StartDestroyMode() {
        if(currentMode == BuildingMode.Build)
            Destroy(currentGhostBuilding);
    }

    private void DestroyMode(Vector3 mousePos) {
        if (Input.GetMouseButton(0)) {
            GameObject building = grid.GetGridObject(mousePos).GetBuildingObject();
            if (building != null) {
                BuildingScript buildingScript = building.GetComponent<BuildingScript>();
                List<Vector3> gridCells = buildingScript.GetGridPositionList();

                foreach (Vector3 gridPos in gridCells) {
                    //Set the building occupying the grid objects to the one we just instantiated
                    grid.GetGridObject((int)gridPos.x, (int)gridPos.z).ClearTransform();
                }
                buildingScript.DestroyThis();

            }
        }
    }

    

    private void UpdateGhostBuilding(Vector3 newPos) {

        currentGhostBuilding.transform.position = newPos;
        currentGhostBuilding.transform.rotation =
            Quaternion.Euler(0, BuildingSO.GetDirectionAngle(currentDirection), 0);
    }

    private void CreateNewGhostBuilding(BuildingSO newBuilding, Vector3 mousePos) {
        currentBuilding = newBuilding;
        Destroy(currentGhostBuilding);
        currentGhostBuilding = newBuilding.buildingPrefab;

        currentGhostBuilding =
                   Instantiate(
                       currentBuilding.buildingPrefab,
                       mousePos,
                       Quaternion.Euler(0, BuildingSO.GetDirectionAngle(currentDirection),
                       0));
        currentGhostBuilding.GetComponentInChildren<MeshRenderer>().material = buildableMat;
    }

    public void CreateBuilding(Vector3 spawnPos, Quaternion rotation, BuildingSO buildingSO, Vector3 originIndices) {
        Debug.Log("CREATE");
        GameObject buildingObj = Instantiate(currentBuilding.buildingPrefab, spawnPos, rotation);

        List<Vector3> occupiedGridCells = currentBuilding.GetGridPositionList(originIndices, currentDirection);

        foreach (Vector3 gridPos in occupiedGridCells) {
            //Set the building occupying the grid objects to the one we just instantiated
            grid.GetGridObject((int)gridPos.x, (int)gridPos.z).setBuilding(buildingObj);
        }

        BuildingScript script = buildingObj.GetComponent<BuildingScript>();
        script.InitValues(buildingSO, new Vector3Int((int)spawnPos.x, 0 ,(int)spawnPos.z), currentDirection);

    }

    public Vector3 GetMouseWorldPos()
    {
        return grid.getMouseWorldPos();
    }

    public Vector3 GetXZCell(Vector3 pos)
    {
        return grid.GetXZCell(pos);
    }

    public GridObject GetGridObject(Vector3 pos)
    {
        return grid.GetGridObject(pos);
    }

    public Vector3 GetWorldPos(int x, int y)
    {
        return grid.GetWorldPos(x, y);
    }

    public float GetCellSize()
    {
        return cellSize;
    }

}

