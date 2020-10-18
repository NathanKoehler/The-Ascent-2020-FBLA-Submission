using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Question List", menuName = "Question System/Question List")]
[System.Serializable]
public class QuestionList_S : ScriptableObject
{
    [Header("Question List")]
    public List<Question_S> questions;
}
