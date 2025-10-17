using UnityEditor;
using UnityEngine;

[CustomPropertyDrawer(typeof(Row))]
public class RowDrawer : PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        var helpers = property.FindPropertyRelative("rowHelpers");
        var moving = property.FindPropertyRelative("MovingRow");
        var pos1 = property.FindPropertyRelative("Pos1");
        var pos2 = property.FindPropertyRelative("Pos2");

        EditorGUI.LabelField(new Rect(position.x, position.y, position.width, EditorGUIUtility.singleLineHeight), label);

        float y = position.y + EditorGUIUtility.singleLineHeight + 4;
        float elementWidth = 100;

        if (helpers.isArray)
        {
            for (int i = 0; i < helpers.arraySize; i++)
            {
                var element = helpers.GetArrayElementAtIndex(i);
                var stack = element.FindPropertyRelative("stack");
                var color = element.FindPropertyRelative("color");

                var rect = new Rect(position.x + (elementWidth + 10) * i, y, elementWidth, EditorGUIUtility.singleLineHeight);
                EditorGUI.PropertyField(rect, stack, GUIContent.none);
                rect.y += EditorGUIUtility.singleLineHeight + 2;
                EditorGUI.PropertyField(rect, color, GUIContent.none);
            }
        }

        y += EditorGUIUtility.singleLineHeight * 2 + 6;
        EditorGUI.PropertyField(new Rect(position.x, y, position.width, EditorGUIUtility.singleLineHeight), moving);

        if (moving.boolValue)
        {
            y += EditorGUIUtility.singleLineHeight + 4;
            EditorGUI.PropertyField(new Rect(position.x, y, position.width, EditorGUIUtility.singleLineHeight), pos1);
            y += EditorGUIUtility.singleLineHeight + 4;
            EditorGUI.PropertyField(new Rect(position.x, y, position.width, EditorGUIUtility.singleLineHeight), pos2);
        }
    }

    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        var moving = property.FindPropertyRelative("MovingRow");
        float height = EditorGUIUtility.singleLineHeight * 4 + 10;
        if (moving.boolValue) height += EditorGUIUtility.singleLineHeight * 2 + 8;
        return height;
    }
}
