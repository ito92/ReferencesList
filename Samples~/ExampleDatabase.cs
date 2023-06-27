using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Debug/Example Database")]
public class ExampleDatabase : ScriptableObject
{
    [System.Serializable]
    public class Item
    {
        [SerializeField]
        private string _name = default;
        [SerializeField]
        private int _value = default;

        public string Name { get => _name; }
        public int Value { get => _value; }
    }

    [SerializeField]
    private Item[] _items = default;

    private static ExampleDatabase _instance;
    public static ExampleDatabase Instance { get => _instance; }
    public Item[] Items { get => _items; }

    private void OnEnable()
    {
        _instance = this;
    }
}
