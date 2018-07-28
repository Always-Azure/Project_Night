using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Item
{
    //private Animator _anim;

    public string itemName; //이름
    public ItemType itemType; // 타입
    public int itemID; // 아이디 - 구별자
    public string itemDesc; // 설명
    public Texture2D itemIcon; // 인벤토리 아이콘
    public Sprite Sprite;
    public float itemSize = 0; // 아이템 용량. 배터리면 전력, 체력포션이면 체력회복량
    public bool stackable = false; // 중첩 가능한지에 대해. Item data 스크립트에서 사용

    public enum ItemType //finding item type
    {
        Battery, Consumable, Potion, Hint
    }

    public Item(string name, int id, string desc, ItemType type, float size)
    {
        //Rect rect = new Rect()
        this.itemName = name;
        this.itemType = type;
        this.itemID = id;
        this.itemDesc = desc;
        this.itemIcon = Resources.Load<Texture2D>("Resources/Image/PN_Image_Inventory/" + name);
        this.Sprite = Resources.Load<Sprite>("Inventory/" + itemName);
        this.itemSize = size;
        this.stackable = false;
        //_anim = GetComponent<Animator>();
    }

    public Item()
    {
        itemID = -1;
    }

    // ShallowCopy vs DeepCopy
    public Item Copy()
    {
        return (Item)this.MemberwiseClone();
    }

    //public void OnBoxOpen()
    //{
    //    _anim.SetBool("BoxOpen", true);
    //}

}
