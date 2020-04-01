using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueTrigger : MonoBehaviour
{
    public Dialogue dialogue;
    public TutorialRoom roomManager;

    public bool changesObjectState;
    public GameObject objectToChange;

    public void StartDialogue()
    {
        UIManager.current.StartDialogue(dialogue, roomManager);
        UIManager.current.dialogueController.onFinishDialogueCallback += EndDiag;
    }

    // Gets toggled true once dialogue ends,
    //  otherwise EndDiag is called three times in the row.
    bool toggled = false;

    public void EndDiag()
    {
        if (toggled)
            return;

        toggled = true;

        if (changesObjectState)
            objectToChange.SetActive(!objectToChange.activeInHierarchy);

        Destroy(gameObject);
    }
}
