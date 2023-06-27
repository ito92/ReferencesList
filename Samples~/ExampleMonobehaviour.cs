using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DaBois.EditorUtilities;

public class ExampleMonobehaviour : MonoBehaviour
{
    [SerializeField]
    private ExampleDatabase _database = default;
    [SerializeField]
    [ExampleReference]
    private int _itemIdA = default;
    [SerializeField]
    [ExampleSearchableReference]
    private int _itemIdB = default;

    private void Start()
    {
        Debug.Log("Field A: " + (_itemIdA >= 0 ? _database.Items[_itemIdA].Name + ": " + _database.Items[_itemIdA].Value : "Empty"));
        Debug.Log("Field B: " + (_itemIdB >= 0 ? _database.Items[_itemIdB].Name + ": " + _database.Items[_itemIdB].Value : "Empty"));
    }
}
