using UnityEngine;

[System.Serializable]
public class Response
{
    [SerializeField] private string responseText;
    [SerializeField] private DialogueObject dialogueObject;

    public string GetResponseText()
    {
        return responseText;
    }

    public DialogueObject GetDialogueObject()
    {
        return dialogueObject;
    }
}
