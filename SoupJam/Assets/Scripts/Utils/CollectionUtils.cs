using System.Collections.Generic;

public class CollectionUtils
{
    public static Key GetKeyFromValue<Key, Value>(Dictionary<Key, Value> dictionary, Value value)
    {
        foreach (var pair in dictionary) {
            if (pair.Value.Equals(value)) {
                return pair.Key;
            }
        }
        return default;
    }
}
