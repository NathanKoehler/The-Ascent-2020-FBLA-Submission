using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(MovingPlatform_S)), CanEditMultipleObjects]
public class MovingPlatformGUIEditor_S : Editor
{
    SerializedProperty m_LerpTime;
    SerializedProperty m_Repeat;
    SerializedProperty m_SimplifyControls;
    SerializedProperty m_StartPos;
    SerializedProperty m_EndPos;
    SerializedProperty m_EndPosDiff;

    private void OnEnable()
    {
        m_LerpTime = this.serializedObject.FindProperty("lerpTime");
        m_Repeat = this.serializedObject.FindProperty("repeat");
        m_SimplifyControls = this.serializedObject.FindProperty("simplifyControls");
        m_StartPos = this.serializedObject.FindProperty("startPos");
        m_EndPos = this.serializedObject.FindProperty("endPos");
        m_EndPosDiff = this.serializedObject.FindProperty("endPosDiff");
    }

    public override void OnInspectorGUI()
    {
        EditorGUI.BeginChangeCheck();
        this.serializedObject.Update();

        m_LerpTime.floatValue = EditorGUILayout.FloatField("Speed", m_LerpTime.floatValue);
        m_Repeat.boolValue = EditorGUILayout.Toggle("Repeat", m_Repeat.boolValue);

        m_SimplifyControls.boolValue = EditorGUILayout.Toggle("Simplify End Coordinate", m_SimplifyControls.boolValue);

        using (new EditorGUI.DisabledScope(m_SimplifyControls.boolValue == false))
        {
            m_EndPosDiff.vector2Value = EditorGUILayout.Vector2Field("Relative End Position", m_EndPosDiff.vector2Value);
        }

        using (new EditorGUI.DisabledScope(m_SimplifyControls.boolValue))
        {
            m_StartPos.vector2Value = EditorGUILayout.Vector2Field("Start Position", m_StartPos.vector2Value);
            m_EndPos.vector2Value = EditorGUILayout.Vector2Field("End Position", m_EndPos.vector2Value);
        }

        this.serializedObject.ApplyModifiedProperties();
        EditorGUI.EndChangeCheck();
    }
}
