using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/EventTriggerSO", order = 1)]

public class EventTriggerSO : ScriptableObject
{
    public enum TriggerType
    {
        PlayerAtPos, BuildAtPos, Completable, Completed, Failure
    }

    public TriggerType triggerType;
    public Vector3 triggerPos;

    public enum BuildingType
    {
        None, Harvester, Pipe
    }

    public BuildingType buildingType;

}
