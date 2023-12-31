using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(WaveController))]
public class WaveControllerEditor : Editor
{
    private SerializedProperty enemyTypes;
    private SerializedProperty enemyCounts;

    private void OnEnable()
    {
        enemyTypes = serializedObject.FindProperty("enemyTypes");
        enemyCounts = serializedObject.FindProperty("enemyCounts");
    }

    public override void OnInspectorGUI()
    {
        WaveController targetScript = (WaveController)target;

        serializedObject.Update();

        DrawDefaultInspector();

        FilterObjectArray("criticalEntity", serializedObject);
        FilterObjectArray("bossEntity", serializedObject);

        

        serializedObject.ApplyModifiedProperties();
    }

    

    [CustomPropertyDrawer(typeof(Enemy))]
    [CustomPropertyDrawer(typeof(Character))]
    [CustomPropertyDrawer(typeof(Battleship))]
    public class ObjectDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            EditorGUI.BeginChangeCheck();
            EditorGUI.PropertyField(position, property, label);
            if (EditorGUI.EndChangeCheck())
            {
                // Ensure that the assigned object is of the allowed type
                if (property.objectReferenceValue != null && !IsValidObjectType(property.objectReferenceValue))
                {
                    property.objectReferenceValue = null;
                }
            }
        }
    }

    private void FilterObjectArray(string fieldName, SerializedObject serializedObject)
    {
        SerializedProperty array = serializedObject.FindProperty(fieldName);

        for (int i = 0; i < array.arraySize; i++)
        {
            SerializedProperty element = array.GetArrayElementAtIndex(i);

            if (element.objectReferenceValue != null && !IsValidObjectType(element.objectReferenceValue))
            {
                element.objectReferenceValue = null;
            }
        }
    }
    private static bool IsValidObjectType(Object obj)
    {
        // Add your allowed object types here
        return obj is Enemy || obj is Character || obj is Battleship; // Example: GameObject and Texture2D are allowed
    }
}






