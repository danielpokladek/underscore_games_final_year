using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DialogueController : MonoBehaviour
{
    [SerializeField] private TMP_Text dialName;
    [SerializeField] private TMP_Text dialText;

    private Queue<string> sentenceQueue;

    void Start()
    {
        sentenceQueue = new Queue<string>();
    }

    public void StartDialogue(Dialogue dialogue)
    {
        Debug.Log("Starting conversation with: " + dialogue.NPCName);

        dialName.text = dialogue.NPCName;
        sentenceQueue.Clear();

        foreach (string sentence in dialogue.NPCSentences)
        {
            sentenceQueue.Enqueue(sentence);
        }

        DisplayNextSentence();
    }

    public void DisplayNextSentence()
    {
        if (sentenceQueue.Count == 0)
        {
            EndDialogue();
            return;
        }

        string sentence = sentenceQueue.Dequeue();

        StopAllCoroutines();
        StartCoroutine(TypeSentence(sentence));
    }

    private IEnumerator TypeSentence(string sentence)
    {
        dialText.text = "";

        foreach (char letter in sentence.ToCharArray())
        {
            dialText.text += letter;
            yield return new WaitForSeconds(0.02f);
        }
    }

    private void EndDialogue()
    {
        Debug.Log("End of dialogue");
        UIManager.current.EndDialogue();
    }
}
