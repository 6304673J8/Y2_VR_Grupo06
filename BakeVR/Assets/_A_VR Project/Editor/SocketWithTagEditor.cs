using UnityEngine;
using UnityEditor;
using UnityEditor.XR.Interaction.Toolkit;

[CustomEditor(typeof(SocketTagManager))]
public class SocketWithTagEditor : XRSocketInteractorEditor
{
    //Get The Value From Our Desired Script
    private SerializedProperty targetTag = null;

    protected override void OnEnable()
    {
        base.OnEnable();
        targetTag = serializedObject.FindProperty("targetTag");
    }

    protected override void DrawProperties()
    {
        base.DrawProperties();
        EditorGUILayout.PropertyField(targetTag);
    }
}
