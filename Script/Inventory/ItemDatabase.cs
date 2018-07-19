using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//JOSN 사용시 https://www.youtube.com/watch?v=x24t4DCxGh8 참고

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
