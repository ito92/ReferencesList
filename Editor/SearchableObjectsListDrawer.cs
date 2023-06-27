using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEditor.IMGUI.Controls;
using UnityEngine;

namespace DaBois.EditorUtilities
{
    [CustomPropertyDrawer(typeof(SearchableObjectsListAttribute))]
    public class SearchableObjectsListDrawer : ObjectsListDrawer
    {
        private UnityEditor.IMGUI.Controls.AdvancedDropdownState _dropdownState;
        private readonly float _pingButtonWidth = 16;
        private GUIContent _displayLabel = new GUIContent();

        protected override void GenerateList(out GUIContent[] names)
        {
            names = null;
        }

        protected override start StartsFrom()
        {
            return start.Zero;
        }

        protected virtual void PingCurrent(int id)
        {

        }

        protected override void DrawList(Rect position, SerializedProperty property, GUIContent label)
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

            position = EditorGUI.IndentedRect(position);

            SearchableObjectsListAttribute att = (SearchableObjectsListAttribute)attribute;

            GUI.Label(position, label);

            position.position += Vector2.right * EditorGUIUtility.labelWidth;
            position.width -= position.position.x;

            if (att.pingable)
            {
                position.width -= _pingButtonWidth;
            }

            _displayLabel.text = _listNamesArray[property.intValue + (StartsFrom() == start.Zero ? 0 : 1)].text.Substring(_listNamesArray[property.intValue + (StartsFrom() == start.Zero ? 0 : 1)].text.LastIndexOf("/") + 1);
            _displayLabel.tooltip = _listNamesArray[property.intValue + (StartsFrom() == start.Zero ? 0 : 1)].tooltip;
            _displayLabel.image = _listNamesArray[property.intValue + (StartsFrom() == start.Zero ? 0 : 1)].image;

            if (GUI.Button(position, _displayLabel, EditorStyles.popup))
            {
                if (_dropdownState == null)
                {
                    _dropdownState = new UnityEditor.IMGUI.Controls.AdvancedDropdownState();
                    //_dropdownState = JsonUtility.FromJson<UnityEditor.IMGUI.Controls.AdvancedDropdownState>("{\"states\":[{\"itemId\":" + (property.intValue - (StartsFrom() == start.Zero ? 0 : 1)) + ",\"selectedIndex\":2,\"scroll\":{\"x\":0,\"y\":0}}]}");                    
                    //_dropdownState = JsonUtility.FromJson<UnityEditor.IMGUI.Controls.AdvancedDropdownState>("{\"states\":[{\"itemId\":599598532,\"selectedIndex\":1,\"scroll\":{\"x\":0,\"y\":0}},{\"itemId\":419569007,\"selectedIndex\":4,\"scroll\":{\"x\":0,\"y\":0}},{\"itemId\":1,\"selectedIndex\":-1,\"scroll\":{\"x\":0,\"y\":0}},{\"itemId\":14,\"selectedIndex\":-1,\"scroll\":{\"x\":0,\"y\":0}},{\"itemId\":16,\"selectedIndex\":-1,\"scroll\":{\"x\":0,\"y\":0}},{\"itemId\":17,\"selectedIndex\":-1,\"scroll\":{\"x\":0,\"y\":0}}]}");                    
                }
                
                var dropdown = new AdvanceDropDownWindow(_dropdownState, label.text, _listNamesArray, (t) => SetSelection(property, t - (StartsFrom() == start.Zero ? 0 : 1)));
                dropdown.Show(position);
                //_dropdownState = JsonUtility.FromJson<UnityEditor.IMGUI.Controls.AdvancedDropdownState>("{\"states\":[{\"itemId\":" + (property.intValue - (StartsFrom() == start.Zero ? 0 : 1)) + ",\"selectedIndex\":-1,\"scroll\":{\"x\":0,\"y\":0}}]}");
                //Debug.Log(JsonUtility.ToJson(_dropdownState, true));
            }

            position.x += position.width;
            position.width = _pingButtonWidth;
            
            if(GUI.Button(position, new GUIContent("...", "Open")))
            {
                PingCurrent(property.intValue + (StartsFrom() == start.Zero ? 0 : 1));
            }
        }        

        private void SetSelection(SerializedProperty property, int index)
        {
            EditorGUI.BeginChangeCheck();
            property.intValue = index;
            EditorGUI.EndChangeCheck();
            property.serializedObject.ApplyModifiedProperties();
        }

        protected void ShowObject(Object obj)
        {
            PreviewWindow.Init(obj);
        }
    }

    public class AdvanceDropDownWindow : AdvancedDropdown
    {
        private GUIContent[] _items;
        private System.Action<int> _callback;
        private string _title;
        private AdvancedDropdownState _state;

        public AdvanceDropDownWindow(AdvancedDropdownState state, string title, GUIContent[] items, System.Action<int> callback) : base(state)
        {
            minimumSize = new Vector2(minimumSize.x, 300);
            _items = items;
            _callback = callback;
            _title = title;
            _state = state;
        }

        protected override AdvancedDropdownItem BuildRoot()
        {
            AdvancedDropdownItem root = new AdvancedDropdownItem(_title);
            root.name = _title;
            root.id = 10000;
            AdvancedDropdownItem lastRoot = root;
            AdvancedDropdownItem test = null;
            for (int i = 0; i < _items.Length; i++)
            {
                lastRoot = root;
                string[] cats = _items[i].text.Split("/");

                string itemName = Regex.Replace(cats[cats.Length - 1], "(?<=[a-z])([A-Z])", " $1");
                AdvancedDropdownItem item = new AdvancedDropdownItem(itemName);
                item.id = i;
                test = item;

                for(int j = 0; j < cats.Length - 1; j++)
                {
                    bool exist = false;
                    for (int r = 0; r < lastRoot.children.Count(); r++)
                    {                        
                        if(lastRoot.children.ElementAt(r).name == cats[j])
                        {
                            exist = true;
                            lastRoot = lastRoot.children.ElementAt(r);
                            break;
                        }                        
                    }

                    if (!exist)
                    {
                        AdvancedDropdownItem newRoot = new AdvancedDropdownItem(cats[j]);
                        newRoot.name = cats[j];                        
                        lastRoot.AddChild(newRoot);
                        newRoot.id = (lastRoot.children.Count() + 1) * 10000;
                        lastRoot = newRoot;
                    }
                }                

                lastRoot.AddChild(item);
            }

            //_state = JsonUtility.FromJson<UnityEditor.IMGUI.Controls.AdvancedDropdownState>("{\"states\":[{\"itemId\":" + "7" + ",\"selectedIndex\":2,\"scroll\":{\"x\":0,\"y\":0}}]}");
            return root;
        }

        protected override void ItemSelected(AdvancedDropdownItem item)
        {
            _callback?.Invoke(item.id);
        }
    }

    public class PreviewWindow : EditorWindow
    {
        private Object _target;
        private Editor _editor;

        public static void Init(Object target)
        {
            PreviewWindow window = PreviewWindow.CreateInstance<PreviewWindow>();
            window._target = target;
            window._editor = Editor.CreateEditor(target);
            window.titleContent = new GUIContent(target.name);
            window.Show();
        }

        private void OnGUI()
        {
            if (GUILayout.Button("Locate"))
            {
                EditorGUIUtility.PingObject(_target);
            }
            _editor.OnInspectorGUI();
        }
    }

}