using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public int actionCount;
    public int actionLimit;
    public Vector3 startPos;
    GameManager gm;

    public enum ResourceType {
        Iron, Copper
    }

    // Start is called before the first frame update
    void Start()
    {
        gm = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
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

        // if complete, call
        // OnLevelCompleted();
    }

    void OnLevelCompleted()
    {
        gm.LoadNextScene();
    }
}
