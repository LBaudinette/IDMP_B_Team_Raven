using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;
using TMPro;

public class LevelManager : MonoBehaviour
{
    public int actionCount;
    public int actionLimit;
    public Vector3 startPos;
    public Vector3 endPos;
    GameManager gm;
    StagingGroundPipe sg;
    GridBuilder gb;
    PlayerControls player;

    public List<BuildingSO> buildingSOs;

    public Vector3 stagingGroundPos;
    public List<Vector3> ironPosList;
    public List<Vector3> mineralPosList;
    private List<List<Vector3>> nodePosList;

    public List<BuildingSO> rockSOs;
    public List<Vector3> rock1PosList;
    public List<Vector3> rock2PosList;
    public List<Vector3> rock3PosList;

    public int ironNeeded;
    public int mineralNeeded;

    //UI
    [SerializeField] private TextMeshProUGUI actionLimitText;
    //[SerializeField] private int tempLevelIndex = 2;
    [SerializeField] private DialogueUI dialogueUI;
    [SerializeField] private DialogueObject testDialogueObject;

    private GameObject portalMat;
    private GameObject portalVFX;

    public enum ResourceType
    {
        Iron, Mineral
    }

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerControls>();
        // get game manager, gridbuilder
        gm = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
        gb = GameObject.FindGameObjectWithTag("Grid Builder").GetComponent<GridBuilder>();

        // get gridbuilder to create staging ground, and enable SG script
        GameObject stagingGround = gb.CreateStagingGround(stagingGroundPos);
        sg = stagingGround.GetComponent<StagingGroundPipe>();
        sg.enabled = true;

        portalMat = stagingGround.GetComponent<VFXAccessScript>().portalMat;
        portalVFX = stagingGround.GetComponent<VFXAccessScript>().portalVFX;


        // merge iron and mineral pos lists for node placement
        nodePosList = new List<List<Vector3>>();
        nodePosList.Add(ironPosList);
        nodePosList.Add(mineralPosList);
        // place nodes
        PlaceNodes();

        // update UI & show dialogue
        UpdateActionLimitUI();
        dialogueUI.ShowDialogue(testDialogueObject);
    }

    // Update is called once per frame
    void Update()
    {

    }

    void PlaceNodes()
    {
        // iterate through resource node positions to place resources
        for (int i = 0; i < buildingSOs.Count; i++)
        {
            for (int j = 0; j < nodePosList[i].Count; j++)
            {
                Debug.Log("placing node at" + nodePosList[i][j]);
                gb.BuildNode(nodePosList[i][j], buildingSOs[i]);
            }
        }

        // iterate through rock positions to place rock obstacles
        foreach (Vector3 v in rock1PosList)
        {
            gb.BuildPrimary(v, rockSOs[0]);
        }
        foreach (Vector3 v in rock2PosList)
        {
            gb.BuildPrimary(v, rockSOs[1]);
        }
        foreach (Vector3 v in rock3PosList)
        {
            gb.BuildPrimary(v, rockSOs[2]);
        }
    }

    public void OnNewAction()
    {
        sg.UpdateResources();
        actionCount++;
        UpdateActionLimitUI();
        // if level failed - player exceeded action limit
        if (actionCount > actionLimit)
        {
            // restart level / reload scene
            gm.ReloadScene();
        }
        else
        {
            // else, check completion
            CheckCompletion();

        }
    }

    void CheckCompletion()
    {
        Debug.Log("checking completion");
        // check for staging ground for level completion via belt output enum
        // stagingGround.output == whatever

        List<ResourceType> currResources = sg.GetCurrentResources();
        int currIron = 0;
        int currMineral = 0;

        for (int i = 0; i < currResources.Count; i++)
        {
            Debug.Log(i + " equals: " + currResources[i]);

            if (currResources[i] == ResourceType.Iron)
            {
                currIron++;

            } else if (currResources[i] == ResourceType.Mineral)
            {
                currMineral++;
            }
        }

        if (currIron >= ironNeeded && currMineral >= mineralNeeded)
        {
            Debug.Log("level completed, player must move to end position");
            Debug.Log("enabling portal");
            portalVFX.SetActive(true);
            StartCoroutine(waitToActivatePortalMat());
            if (player.pos.x == endPos.x && player.pos.z == endPos.z)
            {
                StartCoroutine(waitForPlayerMovement());
            }
        }

    }

    void OnLevelCompleted()
    {
        Debug.Log("level completed!");
        gm.LoadNextScene();
    }

    private void UpdateActionLimitUI()
    {
        actionLimitText.text = (actionLimit - actionCount).ToString();
    }

    IEnumerator waitForPlayerMovement()
    {
        Debug.Log("checking for player movement");
        while (player.playerMoving)
        {
            yield return null;
        }
        OnLevelCompleted();

    }
    
    IEnumerator waitToActivatePortalMat()
    {
        float elapsed = 0.0f;
        float total = 1.0f;
        while (elapsed < total)
        {
            elapsed += Time.deltaTime;
            yield return null;
        }
        portalMat.SetActive(true);
    }

}
