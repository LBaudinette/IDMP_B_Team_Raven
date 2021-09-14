using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public int actionCount;
    public int actionLimit;
    public Vector2Int startPos;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnNewAction()
    {
        if (actionCount > actionLimit)
        {
            // player exceeded action limit
            // restart level / reload scene
        } else
        {
            // else, check for staging ground for level completion via belt output enum
            //stagingGround.output == whatever
        }
    }

    void CheckCompletion()
    {
        //if complete, call OnLevelCompleted
    }

    void OnLevelCompleted()
    {

    }
}
