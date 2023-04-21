using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;

public class CharacterDeath : MonoBehaviour
{

    [SerializeField] private CharacterItemInteraction yuuriItemInteractionScript;
    [SerializeField] private NavMeshAgent yuuriNavMeshAgent;
    [SerializeField] private Animator yuuriAnim;
    [SerializeField] private PlayerInputInitialize yuuriInputScript;
    [SerializeField] private CharacterPossession yuuriPosessionScript;
    [SerializeField] private CharacterBarrelInteraction yuuriBarrelInteractionScript;
    [SerializeField] private NavMeshAgentVehicleInteraction yuuriNavMeshAgentVehicleInteractionScript;



    [SerializeField] private CharacterItemInteraction chitoItemInteractionScript;
    [SerializeField] private NavMeshAgent chitoNavMeshAgent;
    [SerializeField] private Animator chitoAnim;
    [SerializeField] private PlayerInputInitialize chitoInputScript;
    [SerializeField] private CharacterPossession chitoPosessionScript;
    [SerializeField] private CharacterBarrelInteraction chitoBarrelInteractionScript;
    [SerializeField] private NavMeshAgentVehicleInteraction chitoNavMeshAgentVehicleInteractionScript;


    public bool isChitoDead;
    public bool isYuuriDead;

    private bool isYuuriPossessable = true;
    private bool isChitoPossessable = true;

    private bool gameOver;
    void LateUpdate()
    {
        isChitoDead = IsDead(chitoItemInteractionScript,
                             chitoAnim,
                             chitoInputScript,
                             chitoNavMeshAgent,
                             chitoBarrelInteractionScript,
                             chitoNavMeshAgentVehicleInteractionScript,
                             isChitoDead);

        isYuuriDead = IsDead(yuuriItemInteractionScript,
                             yuuriAnim,
                             yuuriInputScript,
                             yuuriNavMeshAgent,
                             yuuriBarrelInteractionScript,
                             yuuriNavMeshAgentVehicleInteractionScript,
                             isYuuriDead);

        //check yuuri death sequence
        if (isYuuriDead)
        {
            DeathSequence(yuuriPosessionScript,
                          chitoPosessionScript,
                          yuuriNavMeshAgent,
                          isChitoDead,
                          isYuuriPossessable);
        }
        //check chito death sequence
        if (isChitoDead)
        {
            DeathSequence(chitoPosessionScript,
                          yuuriPosessionScript,
                          chitoNavMeshAgent,
                          isYuuriDead,
                          isChitoPossessable);
        }

        if (isYuuriDead && isChitoDead && !gameOver)
        {
            StartCoroutine(DelayReloadScene());
            gameOver = true;
        }
    }
    bool IsDead(CharacterItemInteraction characterItemInteractionScript,
                Animator characterAnim,
                PlayerInputInitialize characterInputScript,
                NavMeshAgent characterNavMeshAgent,
                CharacterBarrelInteraction deadCharacterBarrelInteractionScript,
                NavMeshAgentVehicleInteraction deadNavMeshAgentVehicleInteraction,
                bool isDead)
    {
        if (characterItemInteractionScript.thirstLevel <= 0 &&
            characterItemInteractionScript.hungerLevel <= 0 &&
            !characterAnim.GetCurrentAnimatorStateInfo(1).IsTag("Ket"))
        {
            //check if navmesg agent is enable and that the navmesh agent has exited the vehicle
            if (characterNavMeshAgent.enabled)
            {
                characterNavMeshAgent.enabled = false;
                deadNavMeshAgentVehicleInteraction.enabled = false;
            }

            //check if player is holding barrel
            if (characterAnim.GetCurrentAnimatorStateInfo(2).IsTag("Carry"))
            {
                deadCharacterBarrelInteractionScript.DropBarrel();
            }
            characterInputScript.actions.Player.Disable();
            characterAnim.SetTrigger("die");
            isDead = true;
        }
        return isDead;
    }

    void DeathSequence(CharacterPossession deadCharacterPosessionScript,
                       CharacterPossession aliveCharacterPosessionScript,
                       NavMeshAgent characterNavMeshAgent,
                       bool otherIsDead,
                       bool isPossessable)
    {
        //disable possession mechanic
        if (isPossessable)
        {
            //check if player is controlling dead character
            if (deadCharacterPosessionScript.enabled)
            {
                aliveCharacterPosessionScript.enabled = false;
                if (!otherIsDead)
                {
                    StartCoroutine(DelayCharacterPosession(deadCharacterPosessionScript, characterNavMeshAgent));
                }
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
        yield return new WaitForSeconds(4);
        deadCharacterPosessionScript.PossessCharacter(true);
    }

    IEnumerator DelayReloadScene()
    {
        yield return new WaitForSeconds(6);
        SceneManager.LoadScene(0);
    }
}


