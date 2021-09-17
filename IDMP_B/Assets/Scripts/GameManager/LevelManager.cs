using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public int actionCount;
    public int actionLimit;
    public Vector3 startPos;
    GameManager gm;
    StagingGroundPipe sg;

    public int neededR1;
    public int neededR2;

    public enum ResourceType {
        Iron, Copper
    }

    // Start is called before the first frame update
    void Start()
    {
        gm = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
        sg = GameObject.FindGameObjectWithTag("Staging Ground").GetComponent<StagingGroundPipe>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void OnNewAction()
    {
        actionCount++;
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
            } else if (r == ResourceType.Copper)
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
}
