using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace DaBois.EditorUtilities
{
    public class ColorsListPickupWindow : EditorWindow
    {
        public static ColorsListPickupWindow Instance;
        private SerializedObject _obj;
        private SerializedProperty _field;
        private Color[] _options;
        private ColorsListDrawer.start _start;
        private readonly int _itemSize = 35;
        private GUIStyle _itemStyle;

        public static void Create(SerializedObject obj, SerializedProperty field, Color[] options, ColorsListDrawer.start startsFrom)
        {
            if (Instance)
            {
                Instance.Close();
            }

            Instance = (ColorsListPickupWindow)GetWindow(typeof(ColorsListPickupWindow), true);
            Instance.titleContent = new GUIContent("Pick Color");
            Instance._obj = obj;
            Instance._field = field;
            Instance._options = options;
            Instance._start = startsFrom;
            Instance._itemStyle = new GUIStyle("HelpBox");
            Instance._itemStyle.normal.background = Texture2D.whiteTexture;
            Instance.ShowPopup();
        }

        private void OnGUI()
        {
            if (_obj == null)
            {
                Close();
                return;
            }

            GUI.backgroundColor = Color.white;

            int rowSize = Mathf.FloorToInt(EditorGUIUtility.currentViewWidth / _itemSize) - 1;
            EditorGUILayout.BeginHorizontal();

            for (int i = 0; i < _options.Length; i++)
            {
                GUI.backgroundColor = _options[i];
                if (GUILayout.Button("", _itemStyle, GUILayout.Width(_itemSize), GUILayout.Height(_itemSize)))
                {
                    _field.intValue = i - (_start == ColorsListDrawer.start.Zero ? 0 : 1);
                    _obj.ApplyModifiedProperties();
                    Close();
                    return;
                }

                if (i == 0 && _options.Length == 1)
                {
                    EditorGUILayout.EndHorizontal();
                }
                else if (i > 0 && ((i + 1) % rowSize == 0 || i == _options.Length - 1))
                {
                    EditorGUILayout.EndHorizontal();
                    if (i != _options.Length - 1)
                    {
                        EditorGUILayout.BeginHorizontal();
                    }
                }
            }


        }
    }
}