using System.Reflection;
using UnityEngine;
using UnityEditor;

namespace EditorTools
{
    [CustomPropertyDrawer(typeof(Range))]
    public class RangeDrawerGUI : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            // Начинаем отрисовку свойства
            EditorGUI.BeginProperty(position, label, property);
        
            // Получаем свойства Min и Max
            SerializedProperty minProp = property.FindPropertyRelative("_min");
            SerializedProperty maxProp = property.FindPropertyRelative("_max");
        
            // Рисуем заголовок
            Rect labelRect = new Rect(position.x, position.y, position.width, EditorGUIUtility.singleLineHeight);
            EditorGUI.LabelField(labelRect, label);
        
            // Вычисляем позиции для полей Min и Max
            float lineHeight = EditorGUIUtility.singleLineHeight;
            float spacing = 2f;
        
            // Первая строка - Min
            Rect minRect = new Rect(position.x, position.y + lineHeight + spacing, position.width, lineHeight);
            // Вторая строка - Max
            Rect maxRect = new Rect(position.x, position.y + (lineHeight + spacing) * 2, position.width, lineHeight);
        
            // Рисуем Min с кнопками
            DrawFieldWithButtons(minRect, minProp, "Min");
        
            // Рисуем Max с кнопками
            DrawFieldWithButtons(maxRect, maxProp, "Max");
        
            // Завершаем отрисовку
            EditorGUI.EndProperty();
        
            // Вызываем Validate для проверки Min <= Max
            Range range = (Range)fieldInfo.GetValue(property.serializedObject.targetObject);
            MethodInfo validateMethod = typeof(Range).GetMethod("Validate", 
                BindingFlags.NonPublic | BindingFlags.Instance);
            validateMethod?.Invoke(range, null);
        }
    
        private void DrawFieldWithButtons(Rect position, SerializedProperty property, string label)
        {
            float buttonWidth = 25f;
            float spacing = 5f;
        
            // Прямоугольник для подписи
            Rect labelRect = new Rect(position.x, position.y, 35f, position.height);
        
            // Прямоугольник для поля значения
            Rect fieldRect = new Rect(position.x + 35f, position.y, position.width - 35f - buttonWidth * 2 - spacing * 2, position.height);
        
            // Прямоугольники для кнопок
            Rect minusRect = new Rect(position.x + position.width - buttonWidth * 2 - spacing, position.y, buttonWidth, position.height);
            Rect plusRect = new Rect(position.x + position.width - buttonWidth, position.y, buttonWidth, position.height);
        
            // Рисуем подпись
            EditorGUI.LabelField(labelRect, label);
        
            // Рисуем поле значения
            EditorGUI.PropertyField(fieldRect, property, GUIContent.none);
        
            // Кнопка "-"
            if (GUI.Button(minusRect, "-"))
            {
                property.intValue--;
                property.serializedObject.ApplyModifiedProperties();
                Range range = (Range)fieldInfo.GetValue(property.serializedObject.targetObject);
                MethodInfo validateMethod = typeof(Range).GetMethod("Validate", 
                    BindingFlags.NonPublic | BindingFlags.Instance);
                validateMethod?.Invoke(range, null);
            }
        
            // Кнопка "+"
            if (GUI.Button(plusRect, "+"))
            {
                property.intValue++;
                property.serializedObject.ApplyModifiedProperties();
            
                Range range = (Range)fieldInfo.GetValue(property.serializedObject.targetObject);
                MethodInfo validateMethod = typeof(Range).GetMethod("Validate", 
                    BindingFlags.NonPublic | BindingFlags.Instance);
                validateMethod?.Invoke(range, null);
            }
        }
    
        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            // Высота: заголовок + 2 строки (Min и Max) + отступы между ними
            float lineHeight = EditorGUIUtility.singleLineHeight;
            float spacing = 2f;
        
            return lineHeight + (lineHeight + spacing) * 2;
        }
    }
}