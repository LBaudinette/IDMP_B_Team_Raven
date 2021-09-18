using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CodeMonkey.Utils;

public class GridBuilder : MonoBehaviour {

    private LevelManager lm;

    public GridXZ<GridObject> grid;
    [SerializeField] private List<BuildingSO> buildingList;     //Holds buildings that the player can create
    [SerializeField] private List<BuildingSO> resourceList;     //Holds resource nodes
    [SerializeField] private List<Vector3> resourcePosList;     //Holds resource positions

    private BuildingSO currentBuilding;

    //The current direction that the building we want to build is facing
    private BuildingSO.Direction currentDirection = BuildingSO.Direction.Down;

    [SerializeField] private int width;
    [SerializeField] private int height;
    [SerializeField] private float cellSize;

    [SerializeField] private Material buildableMat;
    [SerializeField] private Material notBuildableMat;

    private Material currentBuildingMat;
    private GameObject currentGhostBuilding;

    private BuildingMode currentMode = BuildingMode.Idle;

    private Coroutine coroutine;

    public BuildingSO stagingGroundSO;
    private enum BuildingMode {
        Build, Destroy, Idle
    }

    // Start is called before the first frame update
    void Awake() {
        lm = GameObject.FindGameObjectWithTag("LevelManager").GetComponent<LevelManager>();
        grid = new GridXZ<GridObject>(width, height, cellSize, transform.position, (gridXZ, x, y) => new GridObject(gridXZ, x, y));
        currentBuilding = buildingList[0];

        grid.OnGridValueChanged += Grid_OnGridValueChanged;

    }

    public void BuildNode(Vector3 indices, BuildingSO nodeType) {
        CreateSecondary(indices, nodeType);
    }

    public GameObject CreateStagingGround(Vector3 gridIndices) {
        Debug.Log(gridIndices);
        if (grid == null)
        {
            Debug.Log("grid == null");
        }
        Vector3 spawnPos = grid.GetWorldPos((int)gridIndices.x, (int)gridIndices.z) * cellSize;
        Debug.Log(spawnPos);

        GameObject buildingObj = Instantiate(stagingGroundSO.buildingPrefab,
            spawnPos,
            Quaternion.Euler(new Vector3(0.0f, 0.0f, 0.0f)), gameObject.transform);
        StagingGroundPipe script = buildingObj.GetComponent<StagingGroundPipe>();

        buildingObj.GetComponent<StagingGroundPipe>().enabled = true;
        grid.GetGridObject((int)gridIndices.x, (int)gridIndices.z).primaryBuilding = buildingObj;

        script.InitValues(stagingGroundSO, new Vector3((int)spawnPos.x, 0, (int)spawnPos.z), currentDirection, grid);

        return buildingObj;
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
            Destroy(currentGhostBuilding);
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
        currentGhostBuilding.GetComponentInChildren<MeshRenderer>().material = notBuildableMat;
    }

    private void BuildMode(Vector3 mousePos) {

        #region Building Options
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
        else if (Input.GetKeyDown("4")) { //Harvester
            CreateNewGhostBuilding(buildingList[3], mousePos);

            UtilsClass.CreateWorldTextPopup(currentBuilding.name, mousePos, 2);


        }
        else if (Input.GetKeyDown("5")) { //Conveyor 
            CreateNewGhostBuilding(buildingList[4], mousePos);

            UtilsClass.CreateWorldTextPopup(currentBuilding.name, mousePos, 2);


        }
        else if (Input.GetKeyDown("6")) { //staging ground 
            CreateNewGhostBuilding(buildingList[5], mousePos);

            UtilsClass.CreateWorldTextPopup(currentBuilding.name, mousePos, 2);


        }

        //if (Input.GetMouseButtonDown(1)) {
        //    currentDirection = BuildingSO.GetNextDirection(currentDirection);
        //    UtilsClass.CreateWorldTextPopup(currentDirection.ToString(), mousePos, 2);
        //}
        #endregion

        bool canBuild = true;

        //Get the Cell indices in a Vector3 that we want to build in
        Vector3 gridCellIndices = grid.GetXZCell(mousePos);

        Vector3 rotationOffset = currentBuilding.GetAnglePosOffset(currentDirection);
        Vector3 buildingSpawnPos =
                grid.GetWorldPos((int)gridCellIndices.x, (int)gridCellIndices.z) + rotationOffset * cellSize;

        UpdateGhostBuilding(buildingSpawnPos);

        if (Input.GetKeyDown("h")) {
            Debug.Log($"CELL INDICES: {gridCellIndices}");
        }
        


        GridObject gridObject = grid.GetGridObject(mousePos);

        List<Vector3> occupiedGridCells = currentBuilding.GetGridPositionList(gridCellIndices, currentDirection);

        //Check if we can build in the space that we click on
        foreach (Vector3 gridPos in occupiedGridCells) {
            int x = (int)gridPos.x;
            int y = (int)gridPos.y;

            if (x >= 0 && y >= 0 && x <= width - 1 && y <= height - 1
               && grid.GetGridObject((int)gridPos.x, (int)gridPos.z).CanBuild() && gridObject != default) {
                //Can Build on this node
            }
            else {
                //UtilsClass.CreateWorldTextPopup("Cannot build here!", mousePos);
                canBuild = false;
            }
        }

        #region Special Building Checks
        //If it is a building that can only be built on certain nodes, check if it is on a node
        //Ideally buildings like harvesters should be 1x1
        if (currentBuilding.buildingPrefab.CompareTag("Harvester")) {
            //Initially set to false as it can only be built on certain nodes
            canBuild = false;

            //Check the node that it is being built on
            if (gridObject.secondaryBuilding!= null) {

                //Check if there is a resource node and there is no other buildings on top of ot
                if ((gridObject.secondaryBuilding.CompareTag("Iron Node") || 
                    gridObject.secondaryBuilding.CompareTag("Mineral Node")) && 
                    gridObject.primaryBuilding == null) {
                    canBuild = true;
                }
            }
            
        }
        #endregion


        #region Building Code
        if (canBuild) {
            //Set the building and its components material to show that it is buildable
            currentGhostBuilding.GetComponentInChildren<MeshRenderer>().material = buildableMat;
            MeshRenderer[] childRenderers = currentGhostBuilding.GetComponentsInChildren<MeshRenderer>();
            foreach(MeshRenderer currentRenderer in childRenderers) {
                currentRenderer.material = buildableMat;
            }


            if (Input.GetMouseButtonDown(0)) {
                CreateBuilding(
                        buildingSpawnPos, Quaternion.Euler(0, BuildingSO.GetDirectionAngle(currentDirection), 0), currentBuilding, gridCellIndices);
            }

        }
        else {
            if (Input.GetMouseButtonDown(0))
                UtilsClass.CreateWorldTextPopup("Cannot build here!", mousePos);

            //Set the building and its components material to show that it is buildable
            currentGhostBuilding.GetComponentInChildren<MeshRenderer>().material = notBuildableMat;
            MeshRenderer[] childRenderers = currentGhostBuilding.GetComponentsInChildren<MeshRenderer>();
            foreach (MeshRenderer currentRenderer in childRenderers) {
                currentRenderer.material = notBuildableMat;
            }

        }
        #endregion



    }

    private void StartDestroyMode() {
        if(currentMode == BuildingMode.Build)
            Destroy(currentGhostBuilding);
    }

    private void DestroyMode(Vector3 mousePos) {
        if (Input.GetMouseButton(0)) {
            GameObject building = grid.GetGridObject(mousePos).primaryBuilding;
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
        currentGhostBuilding.GetComponentInChildren<MeshRenderer>().material = notBuildableMat;
    }

    public void CreateBuilding(Vector3 spawnPos, Quaternion rotation, BuildingSO buildingSO, Vector3 originIndices) {
        Debug.Log("CREATE");
        GameObject buildingObj = Instantiate(currentBuilding.buildingPrefab, spawnPos, rotation, gameObject.transform );

        List<Vector3> occupiedGridCells = currentBuilding.GetGridPositionList(originIndices, currentDirection);

        foreach (Vector3 gridPos in occupiedGridCells) {

            //Set the building occupying the grid objects to the one we just instantiated
            grid.GetGridObject((int)gridPos.x, (int)gridPos.z).primaryBuilding = buildingObj;
           //Debug.Log($" Grid Position: X: {grid.GetGridObject((int)gridPos.x, (int)gridPos.z).x} + Z: {grid.GetGridObject((int)gridPos.x, (int)gridPos.z).z}");
        }

        BuildingScript script;
        if (currentBuilding.buildingPrefab.CompareTag("Conveyor")) {
            script = buildingObj.GetComponent<ConveyorScript>();
        }
        else {
            script = buildingObj.GetComponent<BuildingScript>();
        }
        script.enabled = true;
        script.InitValues(buildingSO, new Vector3((int)spawnPos.x, 0 ,(int)spawnPos.z), currentDirection, grid);

        // building has been created, tell level manager that a new action has been performed
        lm.OnNewAction();

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


    //Takes a Vector3 containing grid indices and the type of building to make
    public GameObject CreateSecondary(Vector3 gridIndices, BuildingSO buildingSO) {
        Vector3 spawnPos = grid.GetWorldPos((int)gridIndices.x, (int)gridIndices.z) * cellSize;

        GameObject buildingObj = Instantiate(
            buildingSO.buildingPrefab, 
            spawnPos, 
            Quaternion.Euler(new Vector3(0.0f,0.0f,0.0f)));

        grid.GetGridObject((int)gridIndices.x, (int)gridIndices.z).secondaryBuilding = buildingObj;

        return buildingObj;
    }
}

