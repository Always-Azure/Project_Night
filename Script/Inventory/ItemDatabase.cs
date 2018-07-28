using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemDatabase : MonoBehaviour
{
    public static ItemDatabase instance;
    public List<Item> items = new List<Item>();

    private void Awake()
    {
        instance = this;
    }

    void Start()
    {

    }

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
