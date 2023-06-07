using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct DialogueItem
{
    [SerializeField] public GameObject[] reference;
}

public class UnlockDialogueItem : MonoBehaviour
{
    public DialogueManager dialogueManagerScript;
    public string[] itemName;
    public DialogueItem[] dialogueItems;
    public int[] dialogueItemIndex;
    public bool[] unlocked;
    private int index;

    void Update()
    {
        //check if index is at point along dialogue to unlock item 
        if (index < dialogueItems.Length)
        {
            if (dialogueItemIndex[index] == dialogueManagerScript.index && !unlocked[index])
            {
                //dynamically enable the component for each character
                foreach (GameObject reference in dialogueItems[index].reference)
                {
                    if (reference != null)
                    {
                        MonoBehaviour monoBehaviour = reference.GetComponent(itemName[index]) as MonoBehaviour;
                        if (monoBehaviour != null)
                        {
                            monoBehaviour.enabled = true;
                        }
                    }
                }
                unlocked[index] = true;
                index++;
            }
        }
    }
}
