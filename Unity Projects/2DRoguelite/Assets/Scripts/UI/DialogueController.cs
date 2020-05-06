using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UIElements;

public class DialogueController : MonoBehaviour
{
    [SerializeField] private TMP_Text dialName;
    [SerializeField] private TMP_Text dialText;

    private Queue<string> sentenceQueue;
    private bool inConvo;

    private TutorialRoom rm;

    public delegate void OnFinishDialogue();
    public OnFinishDialogue onFinishDialogueCallback;

    private void Awake()
    {
        sentenceQueue = new Queue<string>();
        inConvo = false;
    }

    private void Update()
    {
        if (inConvo && Input.GetKeyDown(KeyCode.Space) ||
            inConvo && Input.GetMouseButtonDown(0))
        {
            DisplayNextSentence();
        }
    }

    public void StartDialogue(Dialogue dialogue)
    {
        if (inConvo)
            return;

        inConvo = true;

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
        onFinishDialogueCallback?.Invoke();

        inConvo = false;

        if (UIManager.current)
            UIManager.current.EndDialogue();
    }
}
