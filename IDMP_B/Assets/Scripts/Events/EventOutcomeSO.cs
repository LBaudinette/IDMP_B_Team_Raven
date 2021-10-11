using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/EventOutcomeSO", order = 1)]

public class EventOutcomeSO : ScriptableObject
{
    public enum OutcomeType
    {
        Dialogue, Material, Both
    }

    public OutcomeType outcomeType;

    public DialogueObject dialogue;
    public List<Material> materials;
    public List<int> matIndexes;
}
