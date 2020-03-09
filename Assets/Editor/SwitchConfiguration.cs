using System.Collections;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(Switch))]
public class SwitchConfiguration : Editor {

    SerializedProperty numWalls;
    SerializedProperty moveDown;
    SerializedProperty puzzleWalls;

    void OnEnable() {
        numWalls = serializedObject.FindProperty("numWalls");
        moveDown = serializedObject.FindProperty("moveDown");
        puzzleWalls = serializedObject.FindProperty("puzzleWalls");
    }

    public override void OnInspectorGUI() {
        serializedObject.Update();

        EditorGUILayout.LabelField("Puzzle Walls");
        for (int i = 0; i < numWalls.intValue; i++) {
            EditorGUILayout.BeginHorizontal();
            if (i >= puzzleWalls.arraySize) {
                puzzleWalls.InsertArrayElementAtIndex(i);
            }
            if (i >= moveDown.arraySize) {
                moveDown.InsertArrayElementAtIndex(i);
            }
            EditorGUILayout.PropertyField(puzzleWalls.GetArrayElementAtIndex(i), new GUIContent(""));
            float labelWidth = EditorGUIUtility.labelWidth;
            EditorGUIUtility.labelWidth = 50;
            EditorGUILayout.PropertyField(moveDown.GetArrayElementAtIndex(i), new GUIContent("Down"));
            EditorGUIUtility.labelWidth = labelWidth;
            if (GUILayout.Button("Remove", EditorStyles.miniButtonRight)) {
                int currentSize = puzzleWalls.arraySize;
                puzzleWalls.DeleteArrayElementAtIndex(i);
                // This is the dumbest thing ever. Unity just nulls out the index if it has an object the first time.
                if (currentSize == puzzleWalls.arraySize) {
                    puzzleWalls.DeleteArrayElementAtIndex(i);
                }
                moveDown.DeleteArrayElementAtIndex(i);
                numWalls.intValue--;
            }
            EditorGUILayout.EndHorizontal();
        }

        if (GUILayout.Button("Add Wall")) {
            numWalls.intValue++;
        }

        serializedObject.ApplyModifiedProperties();
    }
}