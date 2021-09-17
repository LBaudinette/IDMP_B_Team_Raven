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

    public int neededR1;
    public int neededR2;

    //UI
    [SerializeField] private TextMeshProUGUI actionLimitText;
    //[SerializeField] private int tempLevelIndex = 2;
    [SerializeField] private DialogueUI dialogueUI;
    [SerializeField] private DialogueObject testDialogueObject;

    public enum ResourceType {
        Iron, Mineral
    }

    // Start is called before the first frame update
    void Start()
    {
        gm = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
		sg = GameObject.FindGameObjectWithTag("Staging Ground").GetComponent<StagingGroundPipe>();
        UpdateActionLimitUI();
        dialogueUI.ShowDialogue(testDialogueObject);
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void OnNewAction()
    {
        actionCount++;
        UpdateActionLimitUI();
        // if level failed - player exceeded action limit
        if (actionCount > actionLimit)
        {
            // restart level / reload scene
            gm.ReloadScene();
        } else
        {
            // else, check completion
            CheckCompletion();
            
        }
    }

    void CheckCompletion()
    {
        // check for staging ground for level completion via belt output enum
        // stagingGround.output == whatever
        List<ResourceType> resourceList = sg.GetCurrentResources();

        int r1 = 0;
        int r2 = 0;

        foreach (ResourceType r in resourceList)
        {
            if (r == ResourceType.Iron)
            {
                r1++;
            } else if (r == ResourceType.Mineral)
            {
                r2++;
            }
        }

        if (r1 >= neededR1 && r2 >= neededR2)
        {
            OnLevelCompleted();
        }

    }

    void OnLevelCompleted()
    {
        gm.LoadNextScene();
    }

    private void UpdateActionLimitUI()
    {
        actionLimitText.text = (actionLimit - actionCount).ToString();
    }

}
