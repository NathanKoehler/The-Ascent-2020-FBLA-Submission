using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OldManNPC_S : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        transform.GetComponent<DialogueTrigger_S>().TriggerDialogue();
    }
}
