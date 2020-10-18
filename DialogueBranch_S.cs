using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[CreateAssetMenu(fileName = "New Dialogue Branch", menuName = "Dialogue/Dialogue Branch")]
[System.Serializable]
public class DialogueBranch_S : ScriptableObject
{
    public AdvancedDialogue_S[] dialogues;
}
