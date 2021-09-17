using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StagingGroundPipe : BuildingScript
{
    public List<LevelManager.ResourceType> currentResources;
    // Start is called before the first frame update
    void Start()
    {
        currentResources = new List<LevelManager.ResourceType>();
    }

    // Update is called once per frame
    void Update()
    {
        currentResources.Clear();
        currentResources = CheckAdjacent(new List<int>());
    }

    public List<LevelManager.ResourceType> GetCurrentResources()
    {
        return currentResources;
    }
}
