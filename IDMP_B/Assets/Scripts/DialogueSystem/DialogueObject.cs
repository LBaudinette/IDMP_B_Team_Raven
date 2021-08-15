using UnityEngine;

[CreateAssetMenu(menuName = "Dialogue/DialogueObject")]
public class DialogueObject : ScriptableObject
{
    [SerializeField] [TextArea] private string[] dialogue;
    [SerializeField] private Response[] responses;

    // read but do not overwrite
    public string[] GetDialogue()
    {
        return dialogue;
    }

    public Response[] GetResponses()
    {
        return responses;
    }

    public bool HasResponses()
    {
        return GetResponses() != null && GetResponses().Length > 0;
    }
}
