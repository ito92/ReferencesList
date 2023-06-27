using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace DaBois.EditorUtilities
{
    [CustomPropertyDrawer(typeof(ColorsListAttribute))]
    public abstract class ColorsListDrawer : PropertyDrawer
    {
        public enum start { Zero, One }

        private bool _init;
        protected List<Color> _list = new List<Color>();
        //protected GUIContent[] _listNamesArray = new GUIContent[0];

        protected abstract start StartsFrom();

        protected virtual void Init(SerializedProperty property)
        {
            if (_init)
            {
                return;
            }
            _init = true;

            if (StartsFrom() == start.One)
            {
                _list.Add(Color.clear);
            }
            Color[] items = null;
            GenerateList(out items);
            
            for(int i = 0; i < items.Length; i++)
            {
                _list.Add(items[i]);
            }
        }

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            Init(property);

            DrawList(position, property, label);

            property.serializedObject.ApplyModifiedProperties();
        }

        protected abstract void GenerateList(out Color[] names);

        protected virtual void DrawList(Rect position, SerializedProperty property, GUIContent label)
        {
            if (property.serializedObject.isEditingMultipleObjects)
            {
                EditorGUI.LabelField(position, label.text, "Multiediting not allowed");
                return;
            }

            EditorGUI.BeginDisabledGroup(true);
            EditorGUI.ColorField(position, label, _list[property.intValue]);
            EditorGUI.EndDisabledGroup();
            if(GUI.Button(position, "", GUIStyle.none))
            {
                ColorsListPickupWindow.Create(property.serializedObject, property, _list.ToArray(), StartsFrom());
            }
            //property.intValue = EditorGUI.(position, label, property.intValue + (StartsFrom() == start.Zero ? 0 : 1), _listNamesArray) - (StartsFrom() == start.Zero ? 0 : 1);
        }
    }
}