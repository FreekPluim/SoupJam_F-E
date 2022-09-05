using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class UnityDictionary<Key, Value> : ISerializationCallbackReceiver
{
    [System.Serializable]
    public struct Pair {
        public Key key;
        public Value value;
    }

    [SerializeField] private List<Pair> dictionary;

    public Dictionary<Key, Value> dict = new Dictionary<Key, Value>();

    //----------------serialization--------------------
    public void OnBeforeSerialize() { }

    //lists to dictionary
    public void OnAfterDeserialize()
    {
        dict = new Dictionary<Key, Value>();

        for (int i = 0; i < dictionary.Count; i++) {
            dict.Add(dictionary[i].key, dictionary[i].value);
        }
    }
}
