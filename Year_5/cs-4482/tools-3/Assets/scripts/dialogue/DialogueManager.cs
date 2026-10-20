using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;
using XNode;

public class DialogueManager : MonoBehaviour
{
    public TMP_Text nameText;
    public TMP_Text sentenceText;
    public Button nextButton, optionAButton, optionBButton, optionCButton, optionDButton;
    public TMP_Text optionAText, optionBText, optionCText, optionDText;
    public Image portrait;
    public AudioClip panelOpen, panelClose;
    [Space]
    public Vector3 showPanelPos = new (0,-140,0);
    public Vector3 hidePanelPos = new (0, -400, 0);
    public float panelAnimationTime = 1;
    public float textSpeed = 0.01f;
    public GameObject languageSelector;

    Node curNode;
    Queue<string> sentences = new ();
    AudioSource source;
    AudioClip talkingClip;

    void Start()
    {
        source = GetComponent<AudioSource>();
    }

    public void StartDialogue(Node rootNode)
    {
        StopAllCoroutines();
        curNode = rootNode;
        //option node
        if (curNode.GetType() == typeof(OptionDialogueNode))
        {
            //load node for speaker
            OptionDialogueNode options = curNode as OptionDialogueNode;
            Dialogue dialogue = options.speaker;
            
            //set panel
            nameText.text = dialogue.name;
            portrait.sprite = dialogue.portrait;
            talkingClip = dialogue.talkingClip;
            sentenceText.text = "";

            //set buttons
            nextButton.gameObject.SetActive(false);
            optionAButton.gameObject.SetActive(false);
            optionBButton.gameObject.SetActive(false);
            optionCButton.gameObject.SetActive(false);
            optionDButton.gameObject.SetActive(false);
            int sentencesCount = options.responses.sentences.Length;
            if (sentencesCount >= 1)
            {
                optionAButton.gameObject.SetActive(true);
                optionAText.text = GetTranslation(options.responses.sentences[0]);
            }
            if (sentencesCount >= 2)
            {
                optionBButton.gameObject.SetActive(true);
                optionBText.text = GetTranslation(options.responses.sentences[1]);
            }
            if (sentencesCount >= 3)
            {
                optionCButton.gameObject.SetActive(true);
                optionCText.text = GetTranslation(options.responses.sentences[2]);
            }
            if (sentencesCount >= 4)
            {
                optionDButton.gameObject.SetActive(true);
                optionDText.text = GetTranslation(options.responses.sentences[3]);
            }

            sentences.Clear();
            for (int i = 0; i < dialogue.sentences.Length; i++)
            {
                // sentences.Enqueue(dialogue.sentences[i]);
                sentences.Enqueue(GetTranslation(dialogue.sentences[i]));
            }

            source.PlayOneShot(panelOpen);
            transform.DOLocalMove(showPanelPos, panelAnimationTime).OnComplete(() => DisplaySentence());
        }
        //simple node
        else if (curNode.GetType() == typeof(SimpleDialogueNode))
        {
            //load node for speaker
            SimpleDialogueNode simple = curNode as SimpleDialogueNode;
            Dialogue dialogue = simple.sentence;
            
            //set panel
            nameText.text = dialogue.name;
            portrait.sprite = dialogue.portrait;
            talkingClip = dialogue.talkingClip;
            sentenceText.text = "";

            //set buttons
            nextButton.gameObject.SetActive(true);
            optionAButton.gameObject.SetActive(false);
            optionBButton.gameObject.SetActive(false);
            optionCButton.gameObject.SetActive(false);
            optionDButton.gameObject.SetActive(false);

            sentences.Clear();
            for (int i = 0; i < dialogue.sentences.Length; i++)
            {
                sentences.Enqueue(GetTranslation(dialogue.sentences[i]));
            }

            source.PlayOneShot(panelOpen);
            transform.DOLocalMove(showPanelPos, panelAnimationTime).OnComplete(() => DisplaySentence());
        }
        //control node;
        else
        {
            //load node for speaker
            DialogueControlNode control = curNode as DialogueControlNode;
            
            if (control.dialogueControl == DialogueControlNode.option.endDialogue)
            {
                EndDialogue();
            }
            else if (control.dialogueControl == DialogueControlNode.option.continueDialogue)
            {
                //continue Dialogue
            }
            else
            {
                //restart Dialogue
            }
        }
    }

    public void DisplayNextOption(string option)
    {
        if (option == "A")
        {
            OptionDialogueNode optionNode = curNode as OptionDialogueNode;
            
            NodePort portA = optionNode.GetOutputPort("optionA").Connection;
            
            if (portA != null)
            {
                curNode = portA.node;
            }
        }
        else if (option == "B")
        {
            OptionDialogueNode optionNode = curNode as OptionDialogueNode;
            NodePort portB = optionNode.GetOutputPort("optionB").Connection;
            
            if (portB != null)
            {
                curNode = portB.node;
            }
        }
        else if (option == "C")
        {
            OptionDialogueNode optionNode = curNode as OptionDialogueNode;
            NodePort portC = optionNode.GetOutputPort("optionC").Connection;
            
            if (portC != null)
            {
                curNode = portC.node;
            }
        }
        else if (option == "D")
        {
            OptionDialogueNode optionNode = curNode as OptionDialogueNode;
            NodePort portD = optionNode.GetOutputPort("optionD").Connection;
            
            if (portD != null)
            {
                curNode = portD.node;
            }
        }
        
        StartDialogue(curNode);
    }

    public void DisplayNextSimple()
    {
        SimpleDialogueNode simpleNode = curNode as SimpleDialogueNode;
            
        NodePort port = simpleNode.GetOutputPort("nextNode").Connection;
            
        if (port != null)
        {
            curNode = port.node;
        }
        
        StartDialogue(curNode);
    }

    public void DisplaySentence()
    {
        StopAllCoroutines();
        StartCoroutine(RenderSentence(sentences.Dequeue()));
    }

    IEnumerator RenderSentence(string sentence)
    {
        sentenceText.text = "";
        char[] letters = sentence.ToCharArray();
        for (int i = 0; i < letters.Length; i++)
        {
            sentenceText.text += letters[i];
            if(i % 4 == 0)
                source.PlayOneShot(talkingClip);
            yield return new WaitForSeconds(textSpeed);
        }
    }

    public void EndDialogue()
    {
        StopAllCoroutines();
        source.PlayOneShot(panelClose);
        transform.DOLocalMove(hidePanelPos, panelAnimationTime);
        languageSelector.SetActive(true);
    }

    private string GetTranslation(string key)
    {
        return LocalizationManager.Instance.GetTranslation(key);
    }
}
