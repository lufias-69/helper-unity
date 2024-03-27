#if UNITY_EDITOR
using UnityEditor;
#endif
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
    public object conditionValue;

    public ShowIfAttribute(string boolVariableName)
    {
        fieldName = boolVariableName;
        conditionType = ConditionType.Boolean;
        conditionValue = true;
    }

    public ShowIfAttribute(string fieldName, ConditionType conditionType, object conditionValue)
    {
        this.fieldName = fieldName;
        this.conditionType = conditionType;
        this.conditionValue = conditionValue;
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

        bool showProperty = CheckCondition(conditionProperty, showIf.conditionType, showIf.conditionValue);

        if (showProperty)
        {
            EditorGUI.PropertyField(position, property, label, true);
        }
    }

    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        ShowIfAttribute showIf = attribute as ShowIfAttribute;
        SerializedProperty conditionProperty = property.serializedObject.FindProperty(showIf.fieldName);

        if (conditionProperty == null || CheckCondition(conditionProperty, showIf.conditionType, showIf.conditionValue))
        {
            return EditorGUI.GetPropertyHeight(property, label);
        }
        else
        {
            return 0f;
        }
    }

    bool CheckCondition(SerializedProperty conditionProperty, ShowIfAttribute.ConditionType conditionType, object conditionValue)
    {
        switch (conditionType)
        {
            case ShowIfAttribute.ConditionType.Boolean:
                return conditionProperty.boolValue == (bool)conditionValue;
            case ShowIfAttribute.ConditionType.Integer:
                return conditionProperty.intValue == (int)conditionValue;
            case ShowIfAttribute.ConditionType.Float:
                return conditionProperty.floatValue == (float)conditionValue;
            case ShowIfAttribute.ConditionType.Enum:
                return conditionProperty.enumValueIndex == (int)conditionValue;            
            default:
                return false;
        }
    }
}
#endif
