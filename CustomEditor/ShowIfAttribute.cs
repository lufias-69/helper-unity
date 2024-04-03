using UnityEditor;
using UnityEngine;

[System.AttributeUsage(System.AttributeTargets.Field)]
public class ShowIfAttribute : PropertyAttribute
{
    public enum ConditionType
    {
        Boolean,
        Integer,
        Float,
        Enum
    }

    public string fieldName;
    public ConditionType conditionType;
    public object[] conditionValues;

    public ShowIfAttribute(string boolVariableName)
    {
        fieldName = boolVariableName;
        conditionType = ConditionType.Boolean;
        conditionValues = new object[] { true };
    }

    public ShowIfAttribute(string fieldName, ConditionType conditionType, params object[] conditionValues)
    {
        this.fieldName = fieldName;
        this.conditionType = conditionType;
        this.conditionValues = conditionValues;
    }
}

#if UNITY_EDITOR
[CustomPropertyDrawer(typeof(ShowIfAttribute))]
public class ShowIfDrawer : PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        ShowIfAttribute showIf = attribute as ShowIfAttribute;
        SerializedProperty conditionProperty = property.serializedObject.FindProperty(showIf.fieldName);

        if (conditionProperty == null)
        {
            EditorGUI.PropertyField(position, property, label, true);
            return;
        }

        bool showProperty = CheckCondition(conditionProperty, showIf.conditionType, showIf.conditionValues);

        if (showProperty)
        {
            EditorGUI.PropertyField(position, property, label, true);
        }
    }

    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        ShowIfAttribute showIf = attribute as ShowIfAttribute;
        SerializedProperty conditionProperty = property.serializedObject.FindProperty(showIf.fieldName);

        if (conditionProperty == null || CheckCondition(conditionProperty, showIf.conditionType, showIf.conditionValues))
        {
            return EditorGUI.GetPropertyHeight(property, label);
        }
        else
        {
            return 0f;
        }
    }

    bool CheckCondition(SerializedProperty conditionProperty, ShowIfAttribute.ConditionType conditionType, object[] conditionValues)
    {
        switch (conditionType)
        {
            case ShowIfAttribute.ConditionType.Boolean:
                foreach (object value in conditionValues)
                {
                    if (conditionProperty.boolValue == (bool)value)
                        return true;
                }
                return false;
            case ShowIfAttribute.ConditionType.Integer:
                foreach (object value in conditionValues)
                {
                    if (conditionProperty.intValue == (int)value)
                        return true;
                }
                return false;
            case ShowIfAttribute.ConditionType.Float:
                foreach (object value in conditionValues)
                {
                    if (conditionProperty.floatValue == (float)value)
                        return true;
                }
                return false;
            case ShowIfAttribute.ConditionType.Enum:
                foreach (object value in conditionValues)
                {
                    if (conditionProperty.enumValueIndex == (int)value)
                        return true;
                }
                return false;
            default:
                return false;
        }
    }
}
#endif
