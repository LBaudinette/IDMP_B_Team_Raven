using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;
using TMPro;

public class LevelManager : MonoBehaviour
{
    public bool levelCompletable;
    public bool levelFailed;
    public int actionCount;
    public int actionLimit;
    public Vector3 startPos;
    public Vector3 endPos;
    GameManager gm;
    StagingGroundPipe sg;
    GridBuilder gb;
    PlayerControls player;
    EventManager em;

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

    private delegate void Del();

    public enum ResourceType
    {
        Iron, Mineral
    }

    // Start is called before the first frame update
    void Start()
    {
        levelFailed = false;
        levelCompletable = false;
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerControls>();
        // get game manager, gridbuilder
        gm = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
        gb = GameObject.FindGameObjectWithTag("Grid Builder").GetComponent<GridBuilder>();
        em = GetComponent<EventManager>();
        em.SetDialogueUI(dialogueUI);

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
        // wait till player movement has finished, then update stuff
        StartCoroutine(waitForPlayerMovement());
    }

    void updateStuff()
    {
        sg.UpdateResources();
        actionCount++;
        UpdateActionLimitUI();
        // if level failed - player exceeded action limit
        if (actionCount >= actionLimit)
        {
            levelFailed = true;
            // restart level / reload scene
            // if there's an event that functions on the player failing the level, do that
            // else reload the scene
            if (!em.OnLevelFailed())
            {
                OnLevelFailed();
            }
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
            levelCompletable = true;
            Debug.Log("level completable, player must move to end position");
            Debug.Log("enabling portal");
            portalVFX.SetActive(true);
            StartCoroutine(waitToActivatePortalMat());
            if (player.pos.x == endPos.x && player.pos.z == endPos.z)
            {
                // if there's an event that should be triggered by the level being completed, trigger it, else end normally
                if (!em.OnLevelCompleted())
                {
                    OnLevelCompleted();
                }
            }
        }

        // check current event trigger
        em.checkCurrentTrigger();

    }

    public void OnLevelCompleted()
    {
        Debug.Log("level completed!");
        Del handler = gm.LoadNextScene;
        StartCoroutine(waitForDialogue(handler));
    }

    public void OnLevelFailed()
    {
        Debug.Log("level failed, reloading");
        Del handler = gm.ReloadScene;
        StartCoroutine(waitForDialogue(handler));
    }

    private void UpdateActionLimitUI()
    {
        if (actionCount >= actionLimit)
        {
            actionLimitText.text = "0";
            actionLimitText.faceColor = Color.red;
        } else
        {
            actionLimitText.text = (actionLimit - actionCount).ToString();
        }
    }

    IEnumerator waitForPlayerMovement()
    {
        Debug.Log("checking for player movement");
        while (player.playerMoving)
        {
            yield return null;
        }

        updateStuff();

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

    IEnumerator waitForDialogue(Del onDialogueClose)
    {
        while (dialogueUI.IsOpen)
        {
            yield return null;
        }

        onDialogueClose();
    }

}
