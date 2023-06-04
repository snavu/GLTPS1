using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DialogueManager : MonoBehaviour
{
    public string[] lines;
    public TextMeshProUGUI textComponent;
    public float textSpeed;
    private int index;
    public Animator dialogueBoxAnim;

    public NPCInteraction NPCInteractionScript;
    void OnEnable()
    {
        textComponent.text = string.Empty;

        index = 0;
        StartCoroutine(TypeLine());
        ShowDialogueBox();
    }

    IEnumerator TypeLine()
    {
        foreach (char c in lines[index].ToCharArray())
        {
            textComponent.text += c;
            yield return new WaitForSeconds(textSpeed);
        }
    }

    public void NextLine()
    {
        if (index < lines.Length - 1 && textComponent.text == lines[index])
        {
            index++;
            textComponent.text = string.Empty;
            StartCoroutine(TypeLine());
        }
        else if (textComponent.text != lines[index])
        {
            StopAllCoroutines();
            textComponent.text = lines[index];
        }
        else
        {
            HideDialogueBox();
        }
    }

    public void ShowDialogueBox()
    {
        dialogueBoxAnim.SetTrigger("open");
    }
    public void HideDialogueBox()
    {
        dialogueBoxAnim.SetTrigger("close");
        NPCInteractionScript.triggerDialogue = false;
        NPCInteractionScript.characterPossessionScript.enabled = true;
        this.enabled = false;
    }
}
