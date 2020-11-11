using UnityEngine;
using UnityEditor;
using System.Collections;
[CustomEditor(typeof(DiskData))]
[CanEditMultipleObjects]
public class DiskEditor : Editor
{
	SerializedProperty score;  
	SerializedProperty color;    
	SerializedProperty scale;    

	void OnEnable()
	{
		score = serializedObject.FindProperty("score");
		color = serializedObject.FindProperty("color");
		scale = serializedObject.FindProperty("scale");
	}

	public override void OnInspectorGUI()
	{
		serializedObject.Update();
		EditorGUILayout.IntSlider(score, 0, 5, new GUIContent("score"));
		EditorGUILayout.PropertyField(color);
		EditorGUILayout.PropertyField(scale);
		serializedObject.ApplyModifiedProperties();
	}
}