using System.Collections.Generic;

[System.Serializable]
public class SimpleInventory<T>
{
    [System.Serializable]
    public class ItemStack {
        public T item;
        public int count;
    }

    public List<ItemStack> inventory = new List<ItemStack>();

    //-----------item management-----------
    public bool UseItem(T item)
    {
        for (int i = 0; i < inventory.Count; i++) {
            if (item.Equals(inventory[i].item)) {
                inventory[i].count--;
                if (inventory[i].count <= 0) { RemoveItemType(i); }
                return true;
            }
        }
        return false;
    }

    private void RemoveItemType(int index)
    {
        inventory.RemoveAt(index);
    }

    public void GainItem(T item)
    {
        for (int i = 0; i < inventory.Count; i++) {
            if (item.Equals(inventory[i].item)) {
                inventory[i].count++;
                return;
            }
        }
        //add new item type to inventory
        inventory.Add(new ItemStack { item = item, count = 1 });
    }
}
