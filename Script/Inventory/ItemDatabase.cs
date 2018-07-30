using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Item database. You can get all item data from here.
/// </summary>
/// <author> SangJun </author>
public class ItemDatabase : MonoBehaviour
{
    public static ItemDatabase instance;
    public List<Item> items = new List<Item>(); // Items declare in Unity object inspector.

    private void Awake()
    {
        instance = this;
    }
    
    /// <summary>
    /// Get Item Data by id.
    /// </summary>
    /// <param name="id"> Item id(Unique value) </param>
    /// <returns></returns>
    public Item FetchItemByID(int id)
    {
        for (int i = 0; i < items.Count; ++i)
        {
            if (items[i].itemID == id + 1)
            {
                return items[i].Copy();
            }
        }
        return null;
    }
}
