using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{

    public List<ItemScriptableObject> inventory = new List<ItemScriptableObject>();
    public List<int> itemCounts = new List<int>();

    public void AddToInventory(ItemScriptableObject item, int count)
    {
        // check if item already exists in inventory

    }
}
