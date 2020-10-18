using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Question", menuName = "Question System/Question")]
[System.Serializable]
public class Question_S : ScriptableObject
{
    [Header("Question"), TextArea(1, 3)]
    public string sentence;
    [Space]
    [Header("Answers")]
    [TextArea(1, 3)]
    public string correctOption;
    [TextArea(1, 3)]
    public string wrongOption1;
    [TextArea(1, 3)]
    public string wrongOption2;
}
