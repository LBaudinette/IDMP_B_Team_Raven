using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventManager : MonoBehaviour
{
    private int eventNum;
    private bool eventsExist;
    public List<EventTriggerSO> triggers;
    public List<EventOutcomeSO> outcomes;
    public EventOutcomeSO onLevelFailed;
    public EventOutcomeSO onLevelCompleted;

    public List<GameObject> objsToChangeMats;
    

    private PlayerControls player;
    private GridBuilder grid;
    private LevelManager levelManager;
    private DialogueUI dialogueUI;

    /*private EventTriggerSO*/

    // Start is called before the first frame update
    void Start()
    {
        eventNum = 0;
        eventsExist = true;
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerControls>();
        grid = GameObject.FindGameObjectWithTag("Grid Builder").GetComponent<GridBuilder>();
        levelManager = GameObject.FindGameObjectWithTag("LevelManager").GetComponent<LevelManager>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public bool checkCurrentTrigger()
    {
        // if no events, or all events have been completed, return false
        if (!eventsExist)
        {
            return false;
        }

        bool triggered = false;
        EventTriggerSO currTrigger = triggers[eventNum];
        switch (currTrigger.triggerType)
        {
            case EventTriggerSO.TriggerType.PlayerAtPos:
                if (player.pos == currTrigger.triggerPos)
                {
                    triggered = true;
                }
                break;
            case EventTriggerSO.TriggerType.BuildAtPos:
                GridObject gridObj = grid.GetGridObject(currTrigger.triggerPos);
                switch (currTrigger.buildingType)
                {
                    case EventTriggerSO.BuildingType.Harvester:
                        if (gridObj != default && gridObj.primaryBuilding != null)
                        {
                            if (gridObj.primaryBuilding.CompareTag("Harvester"))
                            {
                                triggered = true;
                            }
                        }
                        break;
                    case EventTriggerSO.BuildingType.Pipe:
                        if (gridObj != default && gridObj.primaryBuilding != null)
                        {
                            if (gridObj.primaryBuilding.CompareTag("Conveyor"))
                            {
                                triggered = true;
                            }
                        }
                        break;
                }

                break;
            case EventTriggerSO.TriggerType.Completable:
                if (levelManager.levelCompletable)
                {
                    triggered = true;
                }
                break;
        }

        if (triggered)
        {
            triggerOutcome(outcomes[eventNum]);
            NextEvent();
        }

        return triggered;
    }
    
    private void triggerOutcome(EventOutcomeSO outcome)
    {
        switch (outcome.outcomeType)
        {
            case EventOutcomeSO.OutcomeType.Dialogue:
                dialogueUI.ShowDialogue(outcome.dialogue);
                break;
            case EventOutcomeSO.OutcomeType.Material:
                for (int i = 0; i < outcome.matIndexes.Count; i++)
                {
                    if (outcome.matIndexes[i] != -1)
                    {
                        objsToChangeMats[i].GetComponent<MeshRenderer>().material = outcome.materials[outcome.matIndexes[i]];
                    }
                    
                }
                break;
            case EventOutcomeSO.OutcomeType.Both:
                for (int i = 0; i < outcome.matIndexes.Count; i++)
                {
                    if (outcome.matIndexes[i] != -1)
                    {
                        objsToChangeMats[i].GetComponent<MeshRenderer>().material = outcome.materials[outcome.matIndexes[i]];
                    }

                }
                dialogueUI.ShowDialogue(outcome.dialogue);
                break;
        }
    }

    public bool OnLevelCompleted()
    {
        if (onLevelCompleted == null)
        {
            return false;
        } else
        {
            triggerOutcome(onLevelCompleted);
            levelManager.OnLevelCompleted();
            return true;
        }
    }

    public bool OnLevelFailed()
    {
        if (onLevelFailed == null)
        {
            return false;
        }
        else
        {
            triggerOutcome(onLevelFailed);
            levelManager.OnLevelFailed();
            return true;
        }
    }

    public void NextEvent()
    {
        eventNum++;
        if (eventNum >= triggers.Count)
        {
            eventsExist = false;
        }
    }

    public void SetDialogueUI(DialogueUI dUI)
    {
        // dUI lol license suspended lmao lay off the drink
        dialogueUI = dUI;
    }
}
