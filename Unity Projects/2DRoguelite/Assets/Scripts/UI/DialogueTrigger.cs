using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueTrigger : MonoBehaviour
{
    public bool isStoryIntro;
    public DialogueController dialogueController;
    public Dialogue dialogue;
    public TutorialRoom roomManager;

    public bool changesObjectState;
    public GameObject[] objectsToChange;

    private void Start()
    {
        if (isStoryIntro)
            dialogueController.StartDialogue(dialogue);
    }

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
        {
            foreach(GameObject _object in objectsToChange)
            {
                _object.SetActive(!_object.activeInHierarchy);
            }
        }

        Destroy(gameObject);
    }
}
