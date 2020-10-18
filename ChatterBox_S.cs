using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class ChatterBox_S : MonoBehaviour
{
    public Text nameText;
    public Text dialogueText;
    public Text optionAText;
    public Text optionBText;
    public Text optionCText;

    public GameObject button;

    public Animator animatorMain;
    public Animator animatorOptA;
    public Animator animatorOptB;
    public Animator animatorOptC;

    public Button buttonA;
    public Button buttonB;
    public Button buttonC;

    public static int localQuestionsCorrect = 0;
    public static int localQuestionsIncorrect = 0;
    public static bool isTalking;

    [SerializeField]
    private List<DialogueEvent> dialogueEvents = new List<DialogueEvent>();

    [Space]
    [Header("Into Animation Length")]
    [SerializeField]
    private float dialogueOptionDelay;

    [Space]
    [Header("Disabled Button Color Block")]
    [SerializeField]
    private ColorBlock disabledButtonColor;

    private int interation;
    private int breakAwayInteration;

    private static IEnumerator coroutine;
    private GameObject NPC;
    private Question_S question;
    private DialogueBranch_S dialogueBranch;
    private DialogueBranch_S breakAwayDialogueBranch;
    private Queue<string> sentences;
    private bool optionADisabled;
    private bool optionBDisabled;
    private bool optionCDisabled;
    private bool breakAwayNext;
    private bool isTyping;


    // Start is called before the first frame update
    void Start()
    {
        MasterController_S.chatterBox_S = this;
        sentences = new Queue<string>();
    }


    public void StartDialogue(SimpleDialogue_S dialogue, GameObject npcGameObject)
    {
        SoundManager_S.PlaySound("woosh");
        isTalking = true;
        breakAwayNext = false;

        NPC = npcGameObject;

        button.SetActive(true); // Re-enables the NEXT button if it was disabled

        animatorMain.SetBool("isOpen", true);

        nameText.text = dialogue.npc;

        sentences.Clear();

        foreach (string sentence in dialogue.sentences)
        {
            sentences.Enqueue(sentence);
        }

        DisplayNextSentence(); // Goes to the next sentence
    }


    public void StartDialogue(DialogueBranch_S branch, GameObject npcGameObject)
    {
        SoundManager_S.PlaySound("woosh");
        isTalking = true;
        breakAwayNext = false;

        NPC = npcGameObject;

        animatorMain.SetBool("isOpen", true);

        nameText.text = branch.dialogues[0].npc;

        sentences.Clear();

        interation = 0;

        dialogueBranch = branch;

        DisplayNextDialogueSentence();
    }

    public void OnButtonPress()
    {
        DisplayNextSentence();
    }

    public void DisplayNextSentence() // Used for simple interactions, like signs
    {
        if (sentences == null || sentences.Count == 0)
        {
            if (dialogueBranch != null)
            {
                DisplayNextDialogueSentence();
                return;
            }
            else
            {
                EndDialogue();
                return;
            }
        }

        string sentence = sentences.Dequeue(); // Removes the first component of the storage of simple sentences
        StartSentenceGeneration(sentence); // method packaging has enabled for even simple sentences to be linearized conveniently
    }

    public void DisplayNextDialogueSentence() // Used for interactions that warrent a response
    {
        //Debug.Log(dialogueBranch);

        if (breakAwayNext)
        {
            breakAwayNext = false;
            dialogueBranch = breakAwayDialogueBranch;
            interation = breakAwayInteration;
        }

        AdvancedDialogue_S[] advancedDialogues = dialogueBranch.dialogues;
        
        if (advancedDialogues.Length <= interation)
        {
            if (advancedDialogues.Length == 0)
            {
                Debug.Log("Reached end of dialogue branch.");
                EndDialogue(); // Ends the Dialogue
                return; // Prevents the rest of the code from running
            }
            else // Designed for further expansion into what occurs after an array of dialogue is promptly exausted
            {
                EndDialogue(); // Ends the Dialogue
                return; // Prevents the rest of the code from running
            }
        }

        if (advancedDialogues[interation].breakAway != null) // Sees if there as a breakaway point and then escapes to that point after the array has exausted
        {

            AdvancedDialogue_S temp = advancedDialogues[interation];
            breakAwayInteration = temp.breakAwayNumber;
            breakAwayDialogueBranch = temp.breakAway;
            breakAwayNext = true;
        }

        if (advancedDialogues[interation].hasEvent)
        {
            string eventName = advancedDialogues[interation].name;

            foreach (DialogueEvent dialogueEvent in dialogueEvents)
            {
                if (dialogueEvent.Name.Equals(eventName))
                {
                    dialogueEvent.Event.Invoke();
                }
            }
        }

        if (advancedDialogues[interation].GetIsQuestion()) // If the player options tie into the scoring system
        {
            if (question == null)
            {
                optionADisabled = false;
                optionBDisabled = false;
                optionCDisabled = false;
                question = ((QuestionDialogue_S)advancedDialogues[interation]).Activate();
            }

            if (((QuestionDialogue_S)advancedDialogues[interation]).resetChatterboxScoring) // When to tell the ChatterBox, oh we AREN'T talking to the same NPC and reset local scoring
            {
                localQuestionsCorrect = 0;
                if (!optionADisabled && !optionBDisabled && !optionCDisabled)
                    localQuestionsIncorrect = 0;
            }

            button.SetActive(false); // Disables the NEXT button

            if (optionADisabled || (!advancedDialogues[interation].optionACondition.Equals("") && !DialogueConditions_S.HasMetCondition(advancedDialogues[interation].optionACondition)))
            {
                buttonA.interactable = false;
            }
            else
            {
                buttonA.interactable = true;
            }

            if (optionBDisabled || (!advancedDialogues[interation].optionBCondition.Equals("") && !DialogueConditions_S.HasMetCondition(advancedDialogues[interation].optionBCondition)))
            {
                buttonB.interactable = false;
            }
            else
            {
                buttonB.interactable = true;
            }

            if (optionCDisabled || (!advancedDialogues[interation].optionCCondition.Equals("") && !DialogueConditions_S.HasMetCondition(advancedDialogues[interation].optionCCondition)))
            {
                buttonC.interactable = false;
            }
            else
            {
                buttonC.interactable = true;
            }

            StartCoroutine(ExposeDialogue(true));

            optionAText.text = advancedDialogues[interation].optionA;
            optionBText.text = advancedDialogues[interation].optionB;
            optionCText.text = advancedDialogues[interation].optionC;

            StartSentenceGeneration(advancedDialogues[interation].GetSentence());
        }
        else if (advancedDialogues[interation].response) // Sets the responses to reflect the player choices that are not tied to scoring
        {

            button.SetActive(false); // Disables the NEXT button

            if (!advancedDialogues[interation].optionACondition.Equals("") && !DialogueConditions_S.HasMetCondition(advancedDialogues[interation].optionACondition))
            {
                buttonA.interactable = false;
            }
            else
            {
                buttonA.interactable = true;
            }

            if (!advancedDialogues[interation].optionBCondition.Equals("") && !DialogueConditions_S.HasMetCondition(advancedDialogues[interation].optionBCondition))
            {
                buttonB.interactable = false;
            }
            else
            {
                buttonB.interactable = true;
            }

            if (!advancedDialogues[interation].optionCCondition.Equals("") && !DialogueConditions_S.HasMetCondition(advancedDialogues[interation].optionCCondition))
            {
                buttonC.interactable = false;
            }
            else
            {
                buttonC.interactable = true;
            }

            StartCoroutine(ExposeDialogue(true));

            optionAText.text = advancedDialogues[interation].optionA;
            optionBText.text = advancedDialogues[interation].optionB;
            optionCText.text = advancedDialogues[interation].optionC;

            StartSentenceGeneration(advancedDialogues[interation].GetSentence());
        }
        else
        {
            StartSentenceGeneration(advancedDialogues[interation].GetSentence());

            button.SetActive(true); // Re-enables the NEXT button

            interation++;
        }
    }


    private void StartSentenceGeneration(string sentence) // Prints out the text LAST--------to ensure it all works.
    {
        if (sentence.IndexOf('#') != -1) // Tests to see if the player is looking to add any chatterBox in-line commands
        {
            string[] dialogueCommands = sentence.Split('#');
            for (int i = dialogueCommands.Length - 1 ; i >= 0 ; i--)
            {
                if (dialogueCommands[i].Equals("LOCAL_ANS"))
                {
                    dialogueCommands[i] = (localQuestionsCorrect.ToString()); // returns the number of questions correct localized to a single NPC
                }
                else if (dialogueCommands[i].Equals("LOCAL_INC"))
                {
                    dialogueCommands[i] = (localQuestionsIncorrect.ToString()); // returns the number of questions incorrect localized to a single NPC
                }
                else if (dialogueCommands[i].Equals("LOCAL_TOTAL"))
                {
                    dialogueCommands[i] = ((localQuestionsCorrect + localQuestionsIncorrect).ToString()); // returns the total number of questions from the NPC
                }
                else if (dialogueCommands[i].Equals("TOTAL_ANS"))
                {
                    dialogueCommands[i] = 
                        ((MasterController_S.scoreKeeper_Correct[SceneManager.GetActiveScene().buildIndex]).ToString()); // returns the number of questions correct
                }
                else if (dialogueCommands[i].Equals("TOTAL_INC"))
                {
                    dialogueCommands[i] = 
                        ((MasterController_S.scoreKeeper_Correct[SceneManager.GetActiveScene().buildIndex]).ToString()); // returns the number of questions incorrect
                }
                else if (dialogueCommands[i].Equals("TOTAL"))
                {
                    dialogueCommands[i] = ((MasterController_S.scoreKeeper_Correct[SceneManager.GetActiveScene().buildIndex]
                        + MasterController_S.scoreKeeper_Incorrect[SceneManager.GetActiveScene().buildIndex]).ToString()); // total questions
                }
            }
            sentence = "";
            foreach (string command in dialogueCommands)
            {
                sentence += command;
            }
        }
        if (coroutine != null)
            StopCoroutine(coroutine); // If there is any on-going dialogue, end it.
        coroutine = TypeSentence(sentence); // Starts typing the dialogue in the dialogue box
        StartCoroutine(coroutine);
    }


    public void RecieveOpinion(int num) // 3 options for player will all call this function with their respective option number
    {
        if (dialogueBranch != null && dialogueBranch.dialogues[interation].GetIsQuestion())
        {
            RecieveOpinionForQuestion(num); // If the question is for scoring, the buttons will be linked to a different command that focuses modifying the score (questions correct)
            return; // prevents further code
        }

        StartCoroutine(ExposeDialogue(false));

        int temp = interation;

        if (dialogueBranch == null || dialogueBranch.dialogues.Length <= temp)
        {
            return;
        }

        if (num == 0)
        {
            // ADD NEUTRAL QUESTIONS ANSWER RERHUITR54HYIU8RTHYU8I45RTHYSU458I8TY45RU788TU546Y7856T4
            if (dialogueBranch.dialogues[temp].dialogueBranchA != null)
            {
                interation = dialogueBranch.dialogues[temp].dialogueNumberA;
                dialogueBranch = dialogueBranch.dialogues[temp].dialogueBranchA;
            }
            else
            {
                interation++;
            }

            DisplayNextDialogueSentence();
        }
        if (num == 1)
        {
            if (dialogueBranch.dialogues[temp].dialogueBranchB != null)
            {
                interation = dialogueBranch.dialogues[temp].dialogueNumberB;
                dialogueBranch = dialogueBranch.dialogues[temp].dialogueBranchB;
            }
            else
            {
                interation++;
            }

            DisplayNextDialogueSentence();
        }
        if (num == 2)
        {
            if (dialogueBranch.dialogues[temp].dialogueBranchC != null)
            {
                interation = dialogueBranch.dialogues[temp].dialogueNumberC;
                dialogueBranch = dialogueBranch.dialogues[temp].dialogueBranchC;
            }
            else
            {
                interation++;
            }

            DisplayNextDialogueSentence();
        }
    }

    public void RecieveOpinionForQuestion(int num) // 3 options for player requiring score will all call this function
    {
        if (dialogueBranch == null || dialogueBranch.dialogues.Length <= interation)
        {
            return;
        }

        // Needs to know which option is correct
        // On correct go to the next sentence.
        // On incorrect resend the old question. With the wrong answer redded-out.

        QuestionDialogue_S temp = ((QuestionDialogue_S)dialogueBranch.dialogues[interation]); // Fragile system. Handle with care.

        if (num+1 == (temp.correctOption))
        {
            SoundManager_S.PlaySound("correct");
            if (!optionADisabled && !optionBDisabled && !optionCDisabled)
            {
                MasterController_S.scoreKeeper_Correct[SceneManager.GetActiveScene().buildIndex]++;
                
                localQuestionsCorrect++;
            }

            question = null;
            dialogueBranch = temp.playOnCorrect;
            interation = temp.dialogueNumOnCorrect;

            DisplayNextDialogueSentence();
        }
        else
        {
            SoundManager_S.PlaySound("incorrect");
            if (!optionADisabled && !optionBDisabled && !optionCDisabled)
            {
                MasterController_S.scoreKeeper_Incorrect[SceneManager.GetActiveScene().buildIndex]++;
                
                localQuestionsIncorrect++;
            }

            if (num == 0)
                optionADisabled = true;
            else if (num == 1)
                optionBDisabled = true;
            else if (num == 2)
                optionCDisabled = true;
            dialogueBranch = temp.playOnIncorrect;
            interation = temp.dialogueNumOnIncorrect;

            DisplayNextDialogueSentence();
        }

        StartCoroutine(ExposeDialogue(false));
    }

    public void EndDialogue() // When the dialogue ends, reset all memorable components
    {
        question = null;

        // Clears both methods to reset dialogue on completion, just a safety precaution
        sentences.Clear();
        dialogueBranch = null;
        interation = 0;

        animatorMain.SetBool("isOpen", false);  // Closes all the UI elements associated with dialogue

        StartCoroutine(ExposeDialogue(false));




        isTalking = false;
    }

    public void Reward(string type)
    {
        DialogueEventHandler_S eventHandler_S = NPC.GetComponent<DialogueEventHandler_S>();

        eventHandler_S.SetPlayerCheckpoint();

        eventHandler_S.RewardPlayer(type, NPC.transform, 1);
    }

    IEnumerator TypeSentence(string sentence) // Prints out the sentences like real sexy like
    {
        isTyping = true;
        dialogueText.text = "";
        foreach (char letter in sentence.ToCharArray())
        {
            if (Input.GetMouseButtonDown(0))
            {
                dialogueText.text = sentence;
                break;
            }

            dialogueText.text += letter;
            yield return null;
        }
        isTyping = false;
    }

    public void activateLumin()
    {
        MasterController_S.self.hasLumin = true;
    }

    public void SwitchNPCDialogue(int newDialogueBranchNumber)
    {
        NPC.GetComponent<DialogueTrigger_S>().ChangeDialogue(newDialogueBranchNumber);
    }

    IEnumerator ExposeDialogue(bool isExposed) // Adds a cool animation to the player options showing up
    {
        animatorOptA.SetBool("isOpen", isExposed);
        yield return new WaitForSeconds(dialogueOptionDelay);
        animatorOptB.SetBool("isOpen", isExposed);
        yield return new WaitForSeconds(dialogueOptionDelay);
        animatorOptC.SetBool("isOpen", isExposed);
    }
}
