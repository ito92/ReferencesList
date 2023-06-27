using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace DaBois.EditorUtilities
{
    [CustomPropertyDrawer(typeof(ObjectsListAttribute))]
    public abstract class ObjectsListDrawer : PropertyDrawer
    {
        public enum start { Zero, One }

        private bool _init;
        protected List<GUIContent> _listNames = new List<GUIContent>();
        protected GUIContent[] _listNamesArray = new GUIContent[0];

        protected abstract start StartsFrom();

        protected virtual void Init(SerializedProperty property)
        {
            if (_init)
            {
                return;
            }
            _init = true;

            _listNames.Add(new GUIContent("---None---"));
            GUIContent[] items = null;
            GenerateList(out items);
            
            for(int i = 0; i < items.Length; i++)
            {
                _listNames.Add(new GUIContent(items[i]));
            }
        }

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            Init(property);

            DrawList(position, property, label);

            property.serializedObject.ApplyModifiedProperties();
        }

        protected abstract void GenerateList(out GUIContent[] names);

        protected virtual void DrawList(Rect position, SerializedProperty property, GUIContent label)
        {
            if (property.serializedObject.isEditingMultipleObjects)
            {
                EditorGUI.LabelField(position, label.text, "Multiediting not allowed");
                return;
            }

            if (_listNamesArray.Length != _listNames.Count)
            {
                _listNamesArray = _listNames.ToArray();
            }

            property.intValue = EditorGUI.Popup(position, label, property.intValue + (StartsFrom() == start.Zero ? 0 : 1), _listNamesArray) - (StartsFrom() == start.Zero ? 0 : 1);
        }
    }
}