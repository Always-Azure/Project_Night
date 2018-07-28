using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NewInventory : MonoBehaviour
{
    UIManager UImanager;
    public enum ItemLocation
    {
        Inventory, Equip, TreasureBox
    }
    //TreasureBox TBox; //생성된 박스의 정보를 가져옴. //기존 스크립트 갱신필요

    //Inventory
    GameObject InventoryPanel;
    GameObject InvenSlotPanel;
    ItemDatabase database;
    private int InventorySlotAmount = 16;
    private List<Item> InventoryItems = new List<Item>();
    private List<GameObject> InventorySlots = new List<GameObject>();
    private bool InvenTrigger = false;

    //Equip
    GameObject EquipPanel;
    GameObject EquipSlotPanel;
    private int EquipSlotAmount = 3;
    private List<Item> EquipItems = new List<Item>();
    private List<GameObject> EquipSlots = new List<GameObject>();
    private bool EquipTrigger = false;

    //Treasure Box
    public TreasureBox TreasureBox;
    GameObject TreasureBoxPanel;
    GameObject TreasureBoxSlotPanel;
    private int TBoxSlotAmount = 3;
    private List<Item> TBoxItems = new List<Item>();
    private List<GameObject> TBoxSlots = new List<GameObject>();
    private bool TBoxTrigger = false;

    public GameObject InventorySlot;
    public GameObject InventoryItem;
    private bool SlotOnCheck = false;

    private void Start()
    {
        database = GetComponent<ItemDatabase>();
        UImanager = GameObject.Find("UIManager").GetComponent<UIManager>();

        InventoryPanel = GameObject.Find("Inventory");
        InvenSlotPanel = InventoryPanel.transform.Find("Slot Panel").gameObject;
        InventoryPanel.SetActive(InvenTrigger);

        EquipPanel = GameObject.Find("Equip");
        EquipSlotPanel = EquipPanel.transform.Find("Slot Panel").gameObject;
        EquipPanel.SetActive(EquipTrigger);

        TreasureBoxPanel = GameObject.Find("TreasureBox");
        TreasureBoxSlotPanel = TreasureBoxPanel.transform.Find("Slot Panel").gameObject;
        TreasureBoxPanel.SetActive(TBoxTrigger);


        for (int i = 0; i < InventorySlotAmount; ++i)
        {
            InventoryItems.Add(new Item());
            InventorySlots.Add(Instantiate(InventorySlot));
            InventorySlots[i].GetComponent<Slot>().ID = i;
            InventorySlots[i].transform.SetParent(InvenSlotPanel.transform);
        }

        for (int i = 0; i < EquipSlotAmount; ++i)
        {
            EquipItems.Add(new Item());
            EquipSlots.Add(Instantiate(InventorySlot));
            EquipSlots[i].GetComponent<Slot>().ID = i;
            EquipSlots[i].transform.SetParent(EquipSlotPanel.transform);
        }

        for (int i = 0; i < TBoxSlotAmount; ++i)
        {
            TBoxItems.Add(new Item());
            TBoxSlots.Add(Instantiate(InventorySlot));
            TBoxSlots[i].GetComponent<Slot>().ID = i;
            TBoxSlots[i].transform.SetParent(TreasureBoxSlotPanel.transform);
        }


        AddItem_Inventory(0);
        AddItem_Inventory(1);
        AddItem_Inventory(1);
        AddItem_Equip(0);
        AddItem_Inventory(2);

        //AddItem_Equip(1);
        //AddItem_TreasureBox(1);
        //AddItem_TreasureBox(1);
        //AddItem_TreasureBox(1);
    }
    private void Update()
    {
        //각 창의 On Off 확인
        if (Input.GetButtonDown("Inventory"))
        {
            InvenTrigger = !InvenTrigger;
            if (InvenTrigger)
                InventoryPanel.SetActive(InvenTrigger);
            else
                InventoryPanel.SetActive(InvenTrigger);
        }
        if (Input.GetButtonDown("Equipment"))
        {
            EquipTrigger = !EquipTrigger;
            if (EquipTrigger)
                EquipPanel.SetActive(EquipTrigger);
            else
                EquipPanel.SetActive(EquipTrigger);
        }
        //if (Input.GetButtonDown("TreasureBox"))
        //{
        //    TBoxTrigger = !TBoxTrigger;

        //    if (TBoxTrigger)
        //        TreasureBoxPanel.SetActive(TBoxTrigger);
        //    else
        //        TreasureBoxPanel.SetActive(TBoxTrigger);
        //}


        //각 트리거 중 하나라도 true면 슬롯은 켜져있는 상태이다.
        if (InvenTrigger || EquipTrigger || TBoxTrigger) { SlotOnCheck = true; }
        else { SlotOnCheck = false; }
    }

    public void AddItem(ItemLocation _itemLocation, int id)
    {
        if (_itemLocation == ItemLocation.Inventory)
        {
            AddItem_Inventory(id);
        }
        else if (_itemLocation == ItemLocation.Equip)
        {
            AddItem_Equip(id);
        }
        else
        {
            AddItem_TreasureBox(id);
        }
    }
    private void AddItem_Inventory(int id)
    {
        {
            Item itemtoAdd = database.FetchItemByID(id);
            if (itemtoAdd.stackable && CheckIfItemIsInventory(itemtoAdd))
            {
                //같은게 존재하고 이게 중첩이 가능하다면
                for (int i = 0; i < InventoryItems.Count; ++i)
                {
                    if (InventoryItems[i].itemID == id)
                    {
                        ItemData data = InventorySlots[i].transform.GetChild(0).GetComponent<ItemData>();
                        ++data.Amount;
                        data.transform.GetChild(0).GetComponent<Text>().text = data.Amount.ToString();
                    }
                }
            }
            else
            {
                for (int i = 0; i < InventoryItems.Count; ++i)
                {
                    if (InventoryItems[i].itemID == -1)
                    {
                        InventoryItems[i] = itemtoAdd;
                        GameObject itemObj = Instantiate(InventoryItem);
                        itemObj.GetComponent<ItemData>().item = itemtoAdd;
                        itemObj.GetComponent<ItemData>().slot = i;
                        itemObj.GetComponent<ItemData>().Location = "Inventory";
                        itemObj.transform.SetParent(InventorySlots[i].transform);
                        itemObj.transform.position = Vector2.zero; // parent

                        itemObj.GetComponent<Image>().sprite = itemtoAdd.Sprite;
                        itemObj.name = itemtoAdd.itemName;

                        break;
                    }
                }
            }
        }
    }
    bool CheckIfItemIsInventory(Item item)
    {
        for (int i = 0; i < InventoryItems.Count; ++i)
        {
            if (InventoryItems[i].itemID == item.itemID)
            {
                return true;
            }
        }
        return false;
    }
    private void AddItem_Equip(int id)
    {
        Item itemtoAdd = database.FetchItemByID(id);
        if (itemtoAdd.stackable && CheckIfItemIsEquip(itemtoAdd))
        {
            //같은게 존재하고 이게 중첩이 가능하다면
            for (int i = 0; i < EquipItems.Count; ++i)
            {
                if (EquipItems[i].itemID == id)
                {
                    ItemData data = EquipSlots[i].transform.GetChild(0).GetComponent<ItemData>();
                    ++data.Amount;
                    data.transform.GetChild(0).GetComponent<Text>().text = data.Amount.ToString();
                }
            }
        }
        else
        {
            for (int i = 0; i < EquipItems.Count; ++i)
            {
                if (EquipItems[i].itemID == -1)
                {
                    EquipItems[i] = itemtoAdd;
                    GameObject itemObj = Instantiate(InventoryItem);
                    itemObj.GetComponent<ItemData>().item = itemtoAdd;
                    itemObj.GetComponent<ItemData>().slot = i;
                    itemObj.GetComponent<ItemData>().Location = "Equip";
                    itemObj.transform.SetParent(EquipSlots[i].transform);
                    itemObj.transform.position = Vector2.zero; // parent

                    itemObj.GetComponent<Image>().sprite = itemtoAdd.Sprite;
                    itemObj.name = itemtoAdd.itemName;

                    break;
                }
            }
        }
    }
    bool CheckIfItemIsEquip(Item item)
    {
        for (int i = 0; i < EquipItems.Count; ++i)
        {
            if (EquipItems[i].itemID == item.itemID)
            {
                return true;
            }
        }
        return false;
    }
    private void AddItem_TreasureBox(int id)
    {
        Item itemtoAdd = database.FetchItemByID(id);
        if (itemtoAdd.stackable && CheckIfItemIsTreasureBox(itemtoAdd))
        {
            //같은게 존재하고 이게 중첩이 가능하다면
            for (int i = 0; i < TBoxItems.Count; ++i)
            {
                if (TBoxItems[i].itemID == id)
                {
                    ItemData data = TBoxSlots[i].transform.GetChild(0).GetComponent<ItemData>();
                    ++data.Amount;
                    data.transform.GetChild(0).GetComponent<Text>().text = data.Amount.ToString();
                }
            }
        }
        else
        {
            for (int i = 0; i < TBoxItems.Count; ++i)
            {
                if (TBoxItems[i].itemID == -1)
                {
                    TBoxItems[i] = itemtoAdd;
                    GameObject itemObj = Instantiate(InventoryItem);
                    itemObj.GetComponent<ItemData>().item = itemtoAdd;
                    itemObj.GetComponent<ItemData>().slot = i;
                    itemObj.GetComponent<ItemData>().Location = "TreasureBox";
                    itemObj.transform.SetParent(TBoxSlots[i].transform);
                    Debug.Log("aaaaa" + TBoxSlots[i].transform.position);
                    //itemObj.transform.position = Vector2.zero; // parent
                    itemObj.transform.position = new Vector2(itemObj.transform.parent.position.x, itemObj.transform.parent.position.y) + Vector2.zero;

                    itemObj.GetComponent<Image>().sprite = itemtoAdd.Sprite;
                    itemObj.name = itemtoAdd.itemName;

                    break;
                }
            }
        }
    }
    bool CheckIfItemIsTreasureBox(Item item)
    {
        for (int i = 0; i < TBoxItems.Count; ++i)
        {
            if (TBoxItems[i].itemID == item.itemID)
            {
                return true;
            }
        }
        return false;
    }
    private void AddNULLItem_TreasureBox(int id)
    {
        Item itemtoAdd = new Item();

        TBoxItems[id] = itemtoAdd;
        GameObject itemObj = Instantiate(InventoryItem);
        itemObj.GetComponent<ItemData>().item = itemtoAdd;
        itemObj.GetComponent<ItemData>().slot = id;
        itemObj.GetComponent<ItemData>().Location = "TreasureBox";
        itemObj.transform.SetParent(TBoxSlots[id].transform);
        //itemObj.transform.position = Vector2.zero; // parent

        itemObj.transform.position = new Vector2(itemObj.transform.parent.position.x, itemObj.transform.parent.position.y) + Vector2.zero;

        itemObj.GetComponent<Image>().sprite = itemtoAdd.Sprite;
        itemObj.name = itemtoAdd.itemName;
    }
    private void Add_TreasureBox(Item _item, int id)
    {
        TBoxItems[id] = _item;
        GameObject itemObj = Instantiate(InventoryItem);
        itemObj.GetComponent<ItemData>().item = _item;
        itemObj.GetComponent<ItemData>().slot = id;
        itemObj.GetComponent<ItemData>().Location = "TreasureBox";
        itemObj.transform.SetParent(TBoxSlots[id].transform);
        //itemObj.transform.position = Vector2.zero; // parent

        itemObj.transform.position = new Vector2(itemObj.transform.parent.position.x, itemObj.transform.parent.position.y) + Vector2.zero;

        itemObj.GetComponent<Image>().sprite = _item.Sprite;
        itemObj.name = _item.itemName;
    }


    public int GetInventoryCount() { return InventorySlotAmount; }
    public int GetEquipCount() { return EquipSlotAmount; }
    public int GetTreasureBoxCount() { return TBoxSlotAmount; }

    public int GetEquipSlotCount() { return EquipSlotAmount; }
    public bool GetSlotOnCheck() { return SlotOnCheck; }

    public float TotalBatterySize()
    {
        return (GetEquipSlotCount() * database.FetchItemByID(1).itemSize);
    }
    public float CurrentBatterySize()
    {
        float Size = 0;
        for (int i = 0; i < GetEquipCount(); ++i)
        {
            if (EquipItems[i].itemID == 1)
                Size += EquipItems[i].itemSize;
        }

        return Size;
        ;
    }
    public void UseBattery()
    {
        for (int i = 0; i < GetEquipCount(); ++i)
        {
            if (EquipItems[i].itemSize > 0)
            {
                EquipItems[i].itemSize -= 0.1f;
                break;
            }
        }
    }

    public void Link_TreasureBox(TreasureBox Box) // 연동 시작시 정보를 가져옴 -> arr로.
    {
        Item[] _TBoxList = Box.ItemList;

        TBoxTrigger = true;
        TreasureBoxPanel.SetActive(TBoxTrigger);

        if (_TBoxList.Length > TBoxItems.Count)
            Debug.Log("TreasureBox Count Different - Link_TreasureBox()");
        else
        {
            TreasureBox = Box;
            int i;
            for (i = 0; i < _TBoxList.Length; ++i)
            {
                if (_TBoxList[i] == null)
                    continue;
                    //AddNULLItem_TreasureBox(i);

                if (_TBoxList[i].itemID != -1)
                {
                     Add_TreasureBox(_TBoxList[i], i);
                }
            }
        }
    }
    public void CutOff_TreasureBox() // 연동 종료시 TBox 내용 초기화
    {
        TBoxTrigger = false;
        TreasureBoxPanel.SetActive(TBoxTrigger);

        TreasureBox = null;

        for (int i = 0; i < GetTreasureBoxCount(); ++i)
        {
            if (TBoxSlots[i].transform.childCount != 0)
            {
                TBoxItems[i] = new Item();
                Destroy(TBoxSlots[i].transform.GetChild(0).gameObject);
            }
        }
        
    }


    public List<Item> GetInventoryItems() { return InventoryItems; }
    public List<GameObject> GetInventorySlots() { return InventorySlots; }
    public List<Item> GetEquipItems() { return EquipItems; }
    public List<GameObject> GetEquipSlots() { return EquipSlots; }
    public List<Item> GetTreasureBoxItems() { return TBoxItems; }
    public List<GameObject> GetTreasureBoxSlots() { return TBoxSlots; }

    public Item GetInventoryItems(int id) { return InventoryItems[id]; }
    public GameObject GetInventorySlots(int id) { return InventorySlots[id]; }
    public Item GetEquipItems(int id) { return EquipItems[id]; }
    public GameObject GetEquipSlots(int id) { return EquipSlots[id]; }
    public Item GetTreasureBoxItems(int id) { return TBoxItems[id]; }
    public GameObject GetTreasureBoxSlots(int id) { return TBoxSlots[id]; }


    public void SetItem(string Location, int id, Item _item)
    {
        if (Location == "Inventory") SetInventoryItems(id, _item);
        else if (Location == "Equip") SetEquipItems(id, _item);
        else SetTreasureBoxItems(id, _item);
    }
    public void SetSlot(string Location, int id, GameObject _item)
    {
        if (Location == "Inventory") SetInventorySlots(id, _item);
        else if (Location == "Equip") SetEquipSlots(id, _item);
        else SetTreasureBoxSlots(id, _item);
    }
    private void SetInventoryItems(int id, Item _item) { InventoryItems[id] = _item; }
    private void SetInventorySlots(int id, GameObject _slot) { InventorySlots[id] = _slot; }
    private void SetEquipItems(int id, Item _item) { EquipItems[id] = _item; }
    private void SetEquipSlots(int id, GameObject _slot) { EquipSlots[id] = _slot; }
    private void SetTreasureBoxItems(int id, Item _item) { TBoxItems[id] = _item; }
    private void SetTreasureBoxSlots(int id, GameObject _slot) { TBoxSlots[id] = _slot; }
}