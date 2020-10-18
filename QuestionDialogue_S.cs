using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Question Dialogue Element", menuName = "Dialogue/Question Dialogue")]
[System.Serializable]
public class QuestionDialogue_S : AdvancedDialogue_S
{
    [Space]
    public string questionStart;
    private Question_S question;
    [HideInInspector]
    public int correctOption = 0;
    public bool resetChatterboxScoring;
    public DialogueBranch_S playOnCorrect;
    public int dialogueNumOnCorrect;
    public DialogueBranch_S playOnIncorrect;
    public int dialogueNumOnIncorrect;

    public override bool GetIsQuestion()
    {
        return true;
    }

    public Question_S Activate()
    {
        response = true;

        question = MasterController_S.self.RandomQuestion();
        sentence = question.sentence;

        int rand = Random.Range(1, 4);
        correctOption = rand;
        if (rand == 1)
        {
            optionA = question.correctOption;
            dialogueBranchA = playOnCorrect;
            optionB = question.wrongOption1;
            dialogueBranchB = playOnIncorrect;
            optionC = question.wrongOption2;   
            dialogueBranchC = playOnIncorrect;
        }    
        if (rand == 2)
        {
            optionB = question.correctOption;
            dialogueBranchB = playOnCorrect;
            optionA = question.wrongOption1;
            dialogueBranchA = playOnIncorrect;
            optionC = question.wrongOption2;   
            dialogueBranchC = playOnIncorrect;
        }    
        if (rand == 3)
        {
            optionC = question.correctOption;
            dialogueBranchC = playOnCorrect;
            optionA = question.wrongOption1;
            dialogueBranchA = playOnIncorrect;
            optionB = question.wrongOption2;   
            dialogueBranchB = playOnIncorrect;
        }
        return question;
    }
}
