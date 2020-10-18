 using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class DialogueTrigger_S : MonoBehaviour
{
    public Cinemachine.CinemachineVirtualCameraBase CinematicCM;

    public SimpleDialogue_S dialogue;
    public DialogueBranch_S dialogueBranch;
    public DialogueBranch_S[] alternativeBranch;

    public int switchDialogue;

    [SerializeField]
    private bool cutsceneTrigger;
    [SerializeField]
    private float radiusExpansion = 2;

    private CircleCollider2D circleCollider2D;

    private float radius = 0;

    private void Start()
    {
        circleCollider2D = GetComponent<CircleCollider2D>();
        radius = circleCollider2D.radius;
    }

    public void TriggerDialogue()
    {
        circleCollider2D.radius = radius * radiusExpansion;
        if (cutsceneTrigger)
        {
            ChangeCamera(true);
        }
        if (dialogue == null)
        {
            if (switchDialogue == 0)
                MasterController_S.chatterBox_S.StartDialogue(dialogueBranch, gameObject);
            else
                MasterController_S.chatterBox_S.StartDialogue(alternativeBranch[switchDialogue-1], gameObject);
        }
        else
        {
            MasterController_S.chatterBox_S.StartDialogue(dialogue, gameObject);
        }
    }

    public void ChangeDialogue(int newDialogueNum)
    {
        switchDialogue = newDialogueNum;
    }

    public void ChangeCamera(bool enableCam)
    {
        CinematicCM.enabled = enableCam;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
            TriggerDialogue();
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            circleCollider2D.radius = radius;
            MasterController_S.chatterBox_S.EndDialogue();
            if (cutsceneTrigger)
            {
                ChangeCamera(false);
            }
        }
    }
}
