using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class LevelManager : MonoBehaviour
{

    public int actionCount;
    public int actionLimit;
    public Vector3 startPos;
    GameManager gm;
    StagingGroundPipe sg;
    GridBuilder gb;

    public List<BuildingSO> buildingSOs;

    public Vector3 stagingGroundPos;
    public List<Vector3> ironPosList;
    public List<Vector3> mineralPosList;
    private List<List<Vector3>> nodePosList;

    public int ironNeeded;
    public int mineralNeeded;

    //UI
    [SerializeField] private TextMeshProUGUI actionLimitText;
    //[SerializeField] private int tempLevelIndex = 2;
    [SerializeField] private DialogueUI dialogueUI;
    [SerializeField] private DialogueObject testDialogueObject;

    public enum ResourceType
    {
        Iron, Mineral
    }

    // Start is called before the first frame update
    void Start()
    {
        // get game manager, gridbuilder
        gm = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
        gb = GameObject.FindGameObjectWithTag("Grid Builder").GetComponent<GridBuilder>();

        // get gridbuilder to create staging ground, and enable SG script
        sg = gb.CreateStagingGround(stagingGroundPos).GetComponent<StagingGroundPipe>();
        sg.enabled = true;

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

        for (int i = 0; i < buildingSOs.Count; i++)
        {
            for (int j = 0; j < nodePosList[i].Count; j++)
            {
                Debug.Log("placing node at" + nodePosList[i][j]);
                gb.BuildNode(nodePosList[i][j], buildingSOs[i]);
            }
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
            OnLevelCompleted();
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

}
