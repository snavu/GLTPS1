using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


public class CharacterDeath : MonoBehaviour
{

    [SerializeField] private CharacterItemInteraction yuuriItemInteractionScript;
    [SerializeField] private NavMeshAgent yuuriNavMeshAgent;
    [SerializeField] private Animator yuuriAnim;
    [SerializeField] private PlayerInputInitialize yuuriInputScript;
    [SerializeField] private CharacterPossession yuuriPosessionScript;


    [SerializeField] private CharacterItemInteraction chitoItemInteractionScript;
    [SerializeField] private NavMeshAgent chitoNavMeshAgent;
    [SerializeField] private Animator chitoAnim;
    [SerializeField] private PlayerInputInitialize chitoInputScript;
    [SerializeField] private CharacterPossession chitoPosessionScript;




    public bool isChitoDead;
    public bool isYuuriDead;

    private bool isYuuriPossessable = true;
    private bool isChitoPossessable = true;
    void Update()
    {
        isChitoDead = IsDead(chitoItemInteractionScript, chitoAnim, chitoInputScript, chitoNavMeshAgent, isChitoDead);
        isYuuriDead = IsDead(yuuriItemInteractionScript, yuuriAnim, yuuriInputScript, yuuriNavMeshAgent, isYuuriDead);

        //check yuuri death sequence
        if (isYuuriDead)
        {
            DeathSequence(yuuriPosessionScript, chitoPosessionScript, yuuriNavMeshAgent, isYuuriPossessable);
        }
        //check chito death sequence
        if (isChitoDead)
        {
            DeathSequence(chitoPosessionScript, yuuriPosessionScript, chitoNavMeshAgent, isChitoPossessable);
        }
    }
    bool IsDead(CharacterItemInteraction characterItemInteractionScript, Animator characterAnim, PlayerInputInitialize characterInputScript, NavMeshAgent characterNavMeshAgent, bool isDead)
    {
        if (characterItemInteractionScript.thirstLevel <= 0 && !isDead &&
            !characterAnim.GetCurrentAnimatorStateInfo(1).IsTag("Ket"))
        {
            if (characterNavMeshAgent.enabled)
            {
                characterNavMeshAgent.enabled = false;
            }
            characterInputScript.actions.Player.Disable();
            characterAnim.SetTrigger("die");
            isDead = true;
        }
        return isDead;
    }

    void DeathSequence(CharacterPossession deadCharacterPosessionScript, CharacterPossession aliveCharacterPosessionScript, NavMeshAgent characterNavMeshAgent, bool isPossessable)
    {
        //disable possession mechanic
        if (isPossessable)
        {
            //check if player is controlling dead character
            if (deadCharacterPosessionScript.enabled)
            {
                aliveCharacterPosessionScript.enabled = false;
                StartCoroutine(DelayCharacterPosession(deadCharacterPosessionScript, characterNavMeshAgent));
            }
            else //player is controlling alive character
            {
                aliveCharacterPosessionScript.enabled = false;
            }

            isPossessable = false;
        }
    }

    IEnumerator DelayCharacterPosession(CharacterPossession deadCharacterPosessionScript, NavMeshAgent characterNavMeshAgent)
    {
        yield return new WaitForSeconds(2);

        deadCharacterPosessionScript.PossessCharacter(true);
    }
}
