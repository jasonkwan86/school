using UnityEngine;

public class DialogueTrigger : MonoBehaviour
{
    public DialogueTree tree;

    public void TriggerDialog()
    {
        FindAnyObjectByType<DialogueManager>().StartDialogue(tree.nodes[0]);
    }
}
