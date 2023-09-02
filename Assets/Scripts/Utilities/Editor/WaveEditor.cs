using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using static WaveController;

[CustomEditor(typeof(Wave))]
public class WaveEditor : Editor
{
    private SerializedProperty enemyTypes;
    private SerializedProperty enemyCounts;

    private void OnEnable()
    {
        enemyTypes = serializedObject.FindProperty("enemyTypes");
        enemyCounts = serializedObject.FindProperty("enemyCounts");
    }
    // Start is called before the first frame update
    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        DrawDefaultInspector();
        //Sync List<Enemy> and List<int>
        EditorGUILayout.PropertyField(enemyTypes, new GUIContent("Enemy Types"), true);
        EditorGUILayout.PropertyField(enemyCounts, new GUIContent("Enemy Counts"), true);

        if (GUILayout.Button("Sync Lists"))
        {
            // Synchronize the Lists based on their sizes
            SynchronizeLists();
        }
        serializedObject.ApplyModifiedProperties();
    }

    private void SynchronizeLists()
    {
        int targetCount = Mathf.Min(enemyTypes.arraySize, enemyCounts.arraySize);

        while (enemyTypes.arraySize < targetCount)
        {
            enemyTypes.InsertArrayElementAtIndex(enemyTypes.arraySize);
        }

        while (enemyCounts.arraySize < targetCount)
        {
            enemyCounts.InsertArrayElementAtIndex(enemyCounts.arraySize);
        }

        while (enemyTypes.arraySize > targetCount)
        {
            enemyTypes.DeleteArrayElementAtIndex(enemyTypes.arraySize - 1);
        }

        while (enemyCounts.arraySize > targetCount)
        {
            enemyCounts.DeleteArrayElementAtIndex(enemyCounts.arraySize - 1);
        }
    }
}
