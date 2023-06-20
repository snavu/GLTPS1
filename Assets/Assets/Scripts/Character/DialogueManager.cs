using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DialogueManager : MonoBehaviour
{
    public TextMeshProUGUI textComponent;
    public float textSpeed;
    public int index;
    public Animator dialogueBoxAnim;
    public NPCInteraction NPCInteractionScript;
    public string[] lines;
    public AudioClip[] voicelines;
    public AudioSource audioSource;

    public int[] indexOfNextSequence;
    public int sequenceCount;
    void Awake()
    {

        textComponent = GameObject.FindWithTag("Text").GetComponent<TextMeshProUGUI>();
        dialogueBoxAnim = GameObject.FindWithTag("DialogueBox").GetComponent<Animator>();
    }

    public void OnEnable()
    {
        StartCoroutine(TypeLine());
    }
    public IEnumerator TypeLine()
    {
        audioSource.Stop();
        audioSource.PlayOneShot(voicelines[index]);

        textComponent.text = string.Empty;
        foreach (char c in lines[index].ToCharArray())
        {
            textComponent.text += c;
            yield return new WaitForSeconds(textSpeed);
        }
    }

    public void NextLine()
    {
        // type next line
        if (index + 1 < indexOfNextSequence[sequenceCount] && textComponent.text == lines[index])
        {
            index++;
            StartCoroutine(TypeLine());
        }
        // stop typing, set text to line
        else if (textComponent.text != lines[index])
        {
            StopAllCoroutines();
            textComponent.text = lines[index];
        }
        else
        {
            //current seqence is done, increment index for next sequence
            index++;
            //current sequence is done, increment sequence
            if (sequenceCount < indexOfNextSequence.Length - 1)
            {
                sequenceCount++;
            }

            //if last sequence is done, repeat last sequence 
            if (index == voicelines.Length)
            {
                index = indexOfNextSequence[indexOfNextSequence.Length - 2];
            }

            HideDialogueBox();
        }
    }

    public void ShowDialogueBox()
    {
        dialogueBoxAnim.SetTrigger("open");
    }

    public void HideDialogueBox()
    {
        StopAllCoroutines();

        dialogueBoxAnim.SetTrigger("close");
        NPCInteractionScript.triggerDialogue = false;
        NPCInteractionScript.characterPossessionScript.enabled = true;
        this.enabled = false;
    }
}
