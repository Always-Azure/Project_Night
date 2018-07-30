using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Inventory System
/// </summary>
/// <author> SangJun </author>
public class NewInventory : MonoBehaviour
{
    public enum ItemLocation
    {
        Inventory, Equip, TreasureBox
    }

    public GameObject inventorySlot;
    public GameObject inventoryItem;

    //Inventory
    private GameObject _inventoryPanel;
    private GameObject _invenSlotPanel;
    private ItemDatabase _database;
    private List<Item> _inventoryItems = new List<Item>();
    private List<GameObject> _inventorySlots = new List<GameObject>();
    private int _inventorySlotAmount = 16;
    private bool _invenTrigger = false;

    //Equip
    private GameObject _equipPanel;
    private GameObject _equipSlotPanel;
    private List<Item> _equipItems = new List<Item>();
    private List<GameObject> _equipSlots = new List<GameObject>();
    private int _equipSlotAmount = 3;
    private bool _equipTrigger = false;

    //Treasure Box
    public TreasureBox treasureBox;

    private GameObject _treasureBoxPanel;
    private GameObject _treasureBoxSlotPanel;
    private List<Item> _tBoxItems = new List<Item>();
    private List<GameObject> _tBoxSlots = new List<GameObject>();
    private int _tBoxSlotAmount = 3;
    private bool _tBoxTrigger = false;

    private bool SlotOnCheck = false;

    private void Start()
    {
        // 마우스 커서 보이지 않도록
        Cursor.visible = false;

        //아이템 데이터베이스 내용을 저장
        _database = GetComponent<ItemDatabase>();

        //인벤토리 시스템을 만들기 위해 필요한 프리펩과 내용을 가져옴
        _inventoryPanel = GameObject.Find("Inventory");
        _invenSlotPanel = _inventoryPanel.transform.Find("Slot Panel").gameObject;
        _inventoryPanel.SetActive(_invenTrigger);

        _equipPanel = GameObject.Find("Equip");
        _equipSlotPanel = _equipPanel.transform.Find("Slot Panel").gameObject;
        _equipPanel.SetActive(_equipTrigger);

        _treasureBoxPanel = GameObject.Find("TreasureBox");
        _treasureBoxSlotPanel = _treasureBoxPanel.transform.Find("Slot Panel").gameObject;
        _treasureBoxPanel.SetActive(_tBoxTrigger);

        //각 인벤토리 시스템별 슬롯 생성
        for (int i = 0; i < _inventorySlotAmount; ++i)
        {
            _inventoryItems.Add(new Item());
            _inventorySlots.Add(Instantiate(inventorySlot));
            _inventorySlots[i].GetComponent<Slot>().ID = i;
            _inventorySlots[i].transform.SetParent(_invenSlotPanel.transform);
        }

        for (int i = 0; i < _equipSlotAmount; ++i)
        {
            _equipItems.Add(new Item());
            _equipSlots.Add(Instantiate(inventorySlot));
            _equipSlots[i].GetComponent<Slot>().ID = i;
            _equipSlots[i].transform.SetParent(_equipSlotPanel.transform);
        }

        for (int i = 0; i < _tBoxSlotAmount; ++i)
        {
            _tBoxItems.Add(new Item());
            _tBoxSlots.Add(Instantiate(inventorySlot));
            _tBoxSlots[i].GetComponent<Slot>().ID = i;
            _tBoxSlots[i].transform.SetParent(_treasureBoxSlotPanel.transform);
        }

        //각 인벤토리 시스템에 최초 아이템 생성
        AddItemToInventory(0);
        AddItemToInventory(1);
        AddItemToInventory(1);
        AddItemToEquip(0);
        AddItemToInventory(2);
    }

    private void Update()
    {
        //각 창의 On Off 확인
        if (Input.GetButtonDown("Inventory"))
        {
            _invenTrigger = !_invenTrigger;
            if (_invenTrigger)
                _inventoryPanel.SetActive(_invenTrigger);
            else
                _inventoryPanel.SetActive(_invenTrigger);
        }
        if (Input.GetButtonDown("Equipment"))
        {
            _equipTrigger = !_equipTrigger;
            if (_equipTrigger)
                _equipPanel.SetActive(_equipTrigger);
            else
                _equipPanel.SetActive(_equipTrigger);
        }

        //인벤토리 시스템이 동작하는 중인지 판별. 상황에 따라 마우스 처리
        if (_invenTrigger || _equipTrigger || _tBoxTrigger) {
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
            //Cursor.lockState = CursorLockMode.Confined;
            SlotOnCheck = true;
        }
        else {
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
            SlotOnCheck = false;
        }
    }

    /// <summary>
    /// Add Item
    /// </summary>
    /// <param name="_itemLocation"> inventory type </param>
    /// <param name="id"> item id </param>
    public void AddItem(ItemLocation _itemLocation, int id)
    {
        if (_itemLocation == ItemLocation.Inventory)
        {
            AddItemToInventory(id);
        }
        else if (_itemLocation == ItemLocation.Equip)
        {
            AddItemToEquip(id);
        }
        else
        {
            AddItemToTreasureBox(id);
        }
    }
    
    /// <summary>
    /// Add Item in basic inventory
    /// </summary>
    /// <param name="id"> item id </param>
    private void AddItemToInventory(int id)
    {
        Item itemtoAdd = _database.FetchItemByID(id);

        if (itemtoAdd.stackable && IsItemInInventory(itemtoAdd))
        {
            //같은게 존재하고 이게 중첩이 가능하다면
            for (int i = 0; i < _inventoryItems.Count; ++i)
            {
                if (_inventoryItems[i].itemID == id)
                {
                    ItemData data = _inventorySlots[i].transform.GetChild(0).GetComponent<ItemData>();
                    ++data.Amount;
                    data.transform.GetChild(0).GetComponent<Text>().text = data.Amount.ToString();
                }
            }
        }
        else
        {
            for (int i = 0; i < _inventoryItems.Count; ++i)
            {
                if (_inventoryItems[i].itemID == -1)
                {
                    _inventoryItems[i] = itemtoAdd;
                    GameObject itemObj = Instantiate(inventoryItem);
                    itemObj.GetComponent<ItemData>().item = itemtoAdd;
                    itemObj.GetComponent<ItemData>().slot = i;
                    itemObj.GetComponent<ItemData>().Location = "Inventory";
                    itemObj.transform.SetParent(_inventorySlots[i].transform);
                    itemObj.transform.position = Vector2.zero; // parent

                    itemObj.GetComponent<Image>().sprite = itemtoAdd.Sprite;
                    itemObj.name = itemtoAdd.itemName;

                    break;
                }
            }
        }
    }

    /// <summary>
    /// Check Itme is in Inventory.
    /// </summary>
    /// <param name="item"> Item </param>
    /// <returns> true : exist, false : not exist </returns>
    private bool IsItemInInventory(Item item)
    {
        for (int i = 0; i < _inventoryItems.Count; ++i)
        {
            if (_inventoryItems[i].itemID == item.itemID)
            {
                return true;
            }
        }
        return false;
    }

    /// <summary>
    /// Add Item in Equip inventory
    /// </summary>
    /// <param name="id"></param>
    private void AddItemToEquip(int id)
    {
        Item itemtoAdd = _database.FetchItemByID(id);

        if (itemtoAdd.stackable && IsItemInEquip(itemtoAdd))
        {
            //같은게 존재하고 이게 중첩이 가능하다면
            for (int i = 0; i < _equipItems.Count; ++i)
            {
                if (_equipItems[i].itemID == id)
                {
                    ItemData data = _equipSlots[i].transform.GetChild(0).GetComponent<ItemData>();
                    ++data.Amount;
                    data.transform.GetChild(0).GetComponent<Text>().text = data.Amount.ToString();
                }
            }
        }
        else
        {
            for (int i = 0; i < _equipItems.Count; ++i)
            {
                if (_equipItems[i].itemID == -1)
                {
                    _equipItems[i] = itemtoAdd;
                    GameObject itemObj = Instantiate(inventoryItem);
                    itemObj.GetComponent<ItemData>().item = itemtoAdd;
                    itemObj.GetComponent<ItemData>().slot = i;
                    itemObj.GetComponent<ItemData>().Location = "Equip";
                    itemObj.transform.SetParent(_equipSlots[i].transform);
                    itemObj.transform.position = Vector2.zero; // parent
                    itemObj.GetComponent<Image>().sprite = itemtoAdd.Sprite;
                    itemObj.name = itemtoAdd.itemName;

                    break;
                }
            }
        }
    }

    /// <summary>
    /// Check Itme is in Equip inventory
    /// </summary>
    /// <param name="item"> Item </param>
    /// <returns> true : exist, false : not exist </returns>
    private bool IsItemInEquip(Item item)
    {
        for (int i = 0; i < _equipItems.Count; ++i)
        {
            if (_equipItems[i].itemID == item.itemID)
            {
                return true;
            }
        }
        return false;
    }

    /// <summary>
    /// Add Item in TreasureBox
    /// </summary>
    /// <param name="id"> Item id </param>
    private void AddItemToTreasureBox(int id)
    {
        Item itemtoAdd = _database.FetchItemByID(id);

        if (itemtoAdd.stackable && IsItemInTreasureBox(itemtoAdd))
        {
            //같은게 존재하고 이게 중첩이 가능하다면
            for (int i = 0; i < _tBoxItems.Count; ++i)
            {
                if (_tBoxItems[i].itemID == id)
                {
                    ItemData data = _tBoxSlots[i].transform.GetChild(0).GetComponent<ItemData>();
                    ++data.Amount;
                    data.transform.GetChild(0).GetComponent<Text>().text = data.Amount.ToString();
                }
            }
        }
        else
        {
            for (int i = 0; i < _tBoxItems.Count; ++i)
            {
                if (_tBoxItems[i].itemID == -1)
                {
                    _tBoxItems[i] = itemtoAdd;
                    GameObject itemObj = Instantiate(inventoryItem);
                    itemObj.GetComponent<ItemData>().item = itemtoAdd;
                    itemObj.GetComponent<ItemData>().slot = i;
                    itemObj.GetComponent<ItemData>().Location = "TreasureBox";
                    itemObj.transform.SetParent(_tBoxSlots[i].transform);
                    itemObj.transform.position = new Vector2(itemObj.transform.parent.position.x, itemObj.transform.parent.position.y) + Vector2.zero;
                    itemObj.GetComponent<Image>().sprite = itemtoAdd.Sprite;
                    itemObj.name = itemtoAdd.itemName;

                    break;
                }
            }
        }
    }

    /// <summary>
    /// Check Itme is in TreasureBox
    /// </summary>
    /// <param name="item"> Item </param>
    /// <returns> true : exist, false : not exist </returns>
    private bool IsItemInTreasureBox(Item item)
    {
        for (int i = 0; i < _tBoxItems.Count; ++i)
        {
            if (_tBoxItems[i].itemID == item.itemID)
            {
                return true;
            }
        }
        return false;
    }

    /// <summary>
    /// Handle when there are empty slot in TreasureBox(Array).
    /// ex) Array[item, x, x, item, x] => x = Null Item.
    /// </summary>
    /// <param name="id"></param>
    private void AddNULLItemTreasureBox(int id)
    {
        //사실상 빈 칸일때는 continue를 하기때문에 필요없는 함수이긴 함.
        Item itemtoAdd = new Item();

        _tBoxItems[id] = itemtoAdd;
        GameObject itemObj = Instantiate(inventoryItem);
        itemObj.GetComponent<ItemData>().item = itemtoAdd;
        itemObj.GetComponent<ItemData>().slot = id;
        itemObj.GetComponent<ItemData>().Location = "TreasureBox";
        itemObj.transform.SetParent(_tBoxSlots[id].transform);
        itemObj.transform.position = new Vector2(itemObj.transform.parent.position.x, itemObj.transform.parent.position.y) + Vector2.zero;
        itemObj.GetComponent<Image>().sprite = itemtoAdd.Sprite;
        itemObj.name = itemtoAdd.itemName;
    }
    /// <summary>
    /// Handle when syncronize TreasureBox with Inventory
    /// </summary>
    /// <param name="_item"></param>
    /// <param name="id"></param>
    private void AddTreasureBox(Item _item, int id)
    {
        _tBoxItems[id] = _item;
        GameObject itemObj = Instantiate(inventoryItem);
        itemObj.GetComponent<ItemData>().item = _item;
        itemObj.GetComponent<ItemData>().slot = id;
        itemObj.GetComponent<ItemData>().Location = "TreasureBox";
        itemObj.transform.SetParent(_tBoxSlots[id].transform);
        itemObj.transform.position = new Vector2(itemObj.transform.parent.position.x, itemObj.transform.parent.position.y) + Vector2.zero;
        itemObj.GetComponent<Image>().sprite = _item.Sprite;
        itemObj.name = _item.itemName;
    }

    /// <summary>
    /// Get Total sloat count from each Inventory
    /// </summary>
    public int GetInventoryCount() { return _inventorySlotAmount; }
    public int GetEquipCount() { return _equipSlotAmount; }
    public int GetTreasureBoxCount() { return _tBoxSlotAmount; }

    /// <summary>
    /// Check the inventory system is using now
    /// </summary>
    public bool GetSlotOnCheck() { return SlotOnCheck; }

    /// <summary>
    /// Get Total Battery size.
    /// </summary>
    public float GetTotalBatterySize()
    {
        return (GetEquipCount() * _database.FetchItemByID(1).itemSize);
    }

    /// <summary>
    /// Get Current Battery size
    /// </summary>
    public float GetCurrentBatterySize()
    {
        float Size = 0;
        for (int i = 0; i < GetEquipCount(); ++i)
        {
            if (_equipItems[i].itemID == 1)
                Size += _equipItems[i].itemSize;
        }

        return Size;
        ;
    }
    /// <summary>
    /// Process Use battery.
    /// </summary>
    public void UseBattery()
    {
        for (int i = 0; i < GetEquipCount(); ++i)
        {
            if (_equipItems[i].itemSize > 0)
            {
                _equipItems[i].itemSize -= 0.1f;
                break;
            }
        }
    }

    /// <summary>
    /// Syncronize with TreasureBox
    /// Get TreasureBox data
    /// </summary>
    /// <param name="Box"></param>
    public void LinkTreasureBox(TreasureBox Box)
    {
        Item[] _tBoxList = Box.ItemList;

        _tBoxTrigger = true;
        _treasureBoxPanel.SetActive(_tBoxTrigger);
        _invenTrigger = true;
        _inventoryPanel.SetActive(_invenTrigger);

        if (_tBoxList.Length > _tBoxItems.Count)
            Debug.Log("TreasureBox Count Different - Link_treasureBox()");
        else
        {
            treasureBox = Box;
            int i;
            for (i = 0; i < _tBoxList.Length; ++i)
            {
                if (_tBoxList[i] == null)
                    continue;

                if (_tBoxList[i].itemID != -1)
                {
                     AddTreasureBox(_tBoxList[i], i);
                }
            }
        }
    }

    /// <summary>
    /// Release syncronize with TreasureBox
    /// And initialize TreasureBox data to null
    /// </summary>
    public void CutOffTreasureBox() 
    {
        _tBoxTrigger = false;
        _treasureBoxPanel.SetActive(_tBoxTrigger);
        _invenTrigger = false;
        _inventoryPanel.SetActive(_invenTrigger);

        treasureBox = null;

        for (int i = 0; i < GetTreasureBoxCount(); ++i)
        {
            if (_tBoxSlots[i].transform.childCount != 0)
            {
                _tBoxItems[i] = new Item();
                Destroy(_tBoxSlots[i].transform.GetChild(0).gameObject);
            }
        }
        
    }

    // Get, Set Methods
    public List<Item> GetInventoryItems() { return _inventoryItems; }
    public List<GameObject> GetInventorySlots() { return _inventorySlots; }
    public List<Item> GetEquipItems() { return _equipItems; }
    public List<GameObject> GetEquipSlots() { return _equipSlots; }
    public List<Item> GetTreasureBoxItems() { return _tBoxItems; }
    public List<GameObject> GetTreasureBoxSlots() { return _tBoxSlots; }

    public Item GetInventoryItems(int id) { return _inventoryItems[id]; }
    public GameObject GetInventorySlots(int id) { return _inventorySlots[id]; }
    public Item GetEquipItems(int id) { return _equipItems[id]; }
    public GameObject GetEquipSlots(int id) { return _equipSlots[id]; }
    public Item GetTreasureBoxItems(int id) { return _tBoxItems[id]; }
    public GameObject GetTreasureBoxSlots(int id) { return _tBoxSlots[id]; }

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
    private void SetInventoryItems(int id, Item _item) { _inventoryItems[id] = _item; }
    private void SetInventorySlots(int id, GameObject _slot) { _inventorySlots[id] = _slot; }
    private void SetEquipItems(int id, Item _item) { _equipItems[id] = _item; }
    private void SetEquipSlots(int id, GameObject _slot) { _equipSlots[id] = _slot; }
    private void SetTreasureBoxItems(int id, Item _item) { _tBoxItems[id] = _item; }
    private void SetTreasureBoxSlots(int id, GameObject _slot) { _tBoxSlots[id] = _slot; }
}