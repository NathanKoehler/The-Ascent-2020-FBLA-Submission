using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Advanced Dialogue Element", menuName = "Dialogue/Advanced Dialogue")]
[System.Serializable]
public class AdvancedDialogue_S : ScriptableObject
{
    //protected bool isQuestion = false;

    public bool hasEvent;
    
    public string npc;
    [SerializeField, TextArea(3, 10)]
    protected string sentence;

    public bool response;
    [TextArea(1, 3)]
    public string optionA;
    public DialogueBranch_S dialogueBranchA;
    public int dialogueNumberA;

    [TextArea(1, 3)]
    public string optionB;
    public DialogueBranch_S dialogueBranchB;
    public int dialogueNumberB;

    [TextArea(1, 3)]
    public string optionC;
    public DialogueBranch_S dialogueBranchC;
    public int dialogueNumberC;

    public string GetSentence()
    {
        return sentence;
    }
    public virtual bool GetIsQuestion()
    {
        return false;
    }

    [Space]
    public string optionACondition;
    public string optionBCondition;
    public string optionCCondition;
    [Space]
    public DialogueBranch_S breakAway;
    public int breakAwayNumber;
}
