using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryUserSample : MonoBehaviour
{
    [SerializeField] private SimpleInventory<string> inventory;

    [Header("Settings")]
    [SerializeField] private string itemToAdd;
    [SerializeField] private string itemToRemove;

    public void GainItem()
    {
        inventory.GainItem(itemToAdd);
    }

    public void RemoveItem()
    {
        inventory.UseItem(itemToRemove);
    }
}
