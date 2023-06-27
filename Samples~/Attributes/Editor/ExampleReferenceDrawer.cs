using UnityEngine;
using UnityEditor;
using DaBois.EditorUtilities;

[CustomPropertyDrawer(typeof(ExampleReferenceAttribute))]
public class ExampleReferenceDrawer : ObjectsListDrawer
{
    protected override void GenerateList(out GUIContent[] names)
    {
        names = new GUIContent[ExampleDatabase.Instance.Items.Length];
        for(int i = 0; i < names.Length; i++)
        {
            names[i] = new GUIContent(ExampleDatabase.Instance.Items[i].Name);
        }
    }

    protected override start StartsFrom()
    {
        return start.One;
    }
}
