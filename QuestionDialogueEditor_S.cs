using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(QuestionDialogue_S)), CanEditMultipleObjects]
public class QuestionDialogueEditor_S : Editor
{
    SerializedProperty m_ResetChatterboxScoring;
    SerializedProperty m_HasEvent;
    SerializedProperty m_Npc;
    SerializedProperty m_QuestionStart;
    SerializedProperty m_PlayOnCorrect;
    SerializedProperty m_DialogueNumOnCorrect;
    SerializedProperty m_PlayOnIncorrect;
    SerializedProperty m_DialogueNumOnIncorrect;
 

    private void OnEnable()
    {
        m_ResetChatterboxScoring = this.serializedObject.FindProperty("resetChatterboxScoring");
        m_HasEvent = this.serializedObject.FindProperty("hasEvent");
        m_Npc = this.serializedObject.FindProperty("npc");
        m_QuestionStart = this.serializedObject.FindProperty("sentence");
        m_PlayOnCorrect = this.serializedObject.FindProperty("playOnCorrect");
        m_DialogueNumOnCorrect = this.serializedObject.FindProperty("dialogueNumOnCorrect");
        m_PlayOnIncorrect = this.serializedObject.FindProperty("playOnIncorrect");
        m_DialogueNumOnIncorrect = this.serializedObject.FindProperty("dialogueNumOnIncorrect");
    }

    public override void OnInspectorGUI()
    {
        EditorGUILayout.LabelField("Question Dialogue Options", EditorStyles.boldLabel);

        EditorGUI.BeginChangeCheck();
        this.serializedObject.Update();

        m_ResetChatterboxScoring.boolValue = EditorGUILayout.Toggle("Reset Chatterbox Scoring", m_ResetChatterboxScoring.boolValue);
        m_HasEvent.boolValue = EditorGUILayout.Toggle("Has Event", m_HasEvent.boolValue);
        EditorGUILayout.PropertyField(m_Npc, new GUIContent("NPC Name"));
        EditorGUILayout.PropertyField(m_QuestionStart, new GUIContent("Starting Text"));
        EditorGUILayout.Space();
        EditorGUILayout.PropertyField(m_PlayOnCorrect, new GUIContent("Correct Branch"));
        EditorGUILayout.PropertyField(m_DialogueNumOnCorrect, new GUIContent("Dialogue Number"));
        EditorGUILayout.PropertyField(m_PlayOnIncorrect, new GUIContent("Incorrect Branch"));
        EditorGUILayout.PropertyField(m_DialogueNumOnIncorrect, new GUIContent("Dialogue Number"));

        this.serializedObject.ApplyModifiedProperties();
        EditorGUI.EndChangeCheck();
    }
}
