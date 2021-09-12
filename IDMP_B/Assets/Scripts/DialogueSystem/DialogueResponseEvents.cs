using System;
using UnityEngine;

public class DialogueResponseEvents : MonoBehaviour
{
    [SerializeField] private DialogueObject dialogueObject;
    [SerializeField] private ResponseEvent[] events;

    public DialogueObject GetDialogueObject()
    {
        return dialogueObject;
    }

    public ResponseEvent[] GetEvents()
    {
        return events;
    }

    public void OnValidate()
    {
        if(dialogueObject == null)
        {
            return;
        }
        if (dialogueObject.GetResponses() == null)
        {
            return;
        }
        if(events != null && events.Length == dialogueObject.GetResponses().Length)
        {
            return;
        }

        if(events == null)
        {
            events = new ResponseEvent[dialogueObject.GetResponses().Length];
        } else
        {
            Array.Resize(ref events, dialogueObject.GetResponses().Length);
        }

        for (int i = 0; i < dialogueObject.GetResponses().Length; i++)
        {
            Response response = dialogueObject.GetResponses()[i];

            if(events[i] != null)
            {
                events[i].name = response.GetResponseText();
                continue;
            }

            events[i] = new ResponseEvent()
            {
                name = response.GetResponseText()
            };

        }
    }
}
