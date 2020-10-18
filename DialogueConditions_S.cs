using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DialogueConditions_S : MonoBehaviour
{
    public static bool HasMetCondition(string name)
    {
        if (name.Equals("PlayerHasCoat"))
        {
            return PlayerHasCoat();
        }
        else if (name.Equals("LocalAnsAll"))
        {
            return ScoringAnswers(0, 3, true);
        }
        else if (name.Equals("LocalAnsHlf"))
        {
            return ScoringAnswers(0, 2, true);
        }
        else if (name.Equals("LevelAnsAll"))
        {
            return ScoringAnswers(1, 15, true);
        }
        else if (name.Equals("LevelAnsHlf"))
        {
            return ScoringAnswers(1, 7, true);
        }
        else if (name.Equals("LevelAnsNon"))
        {
            return ScoringAnswersSpecial(1, 7, true);
        }
        else
            return false;
    }

    public static bool ScoringAnswersSpecial(int scope, int numCorrect, bool correct)
    {
        if (scope == 0)
        {
            if (correct)
            {
                if (ChatterBox_S.localQuestionsCorrect < numCorrect)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                if (ChatterBox_S.localQuestionsIncorrect < numCorrect)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }
        else if (scope == 1)
        {
            if (MasterController_S.GetLocalScore(correct) < numCorrect)
            {
                return true;
            }
        }
        return false;
    }

        public static bool ScoringAnswers(int scope, int numCorrect, bool correct)
    {
        if (scope == 0)
        {
            if (correct)
            {
                if (ChatterBox_S.localQuestionsCorrect >= numCorrect)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                if (ChatterBox_S.localQuestionsIncorrect >= numCorrect)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }
        else if (scope == 1)
        {
            if (MasterController_S.GetLocalScore(correct) >= numCorrect)
            {
                return true;
            }
        }
        return false;
    }

    public static bool PlayerHasCoat()
    {
        if (UIHealth_S.roleCall[5])
            return true;
        else
            return false;
    }
}
