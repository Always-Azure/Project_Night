using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Slot : MonoBehaviour, IDropHandler, IPointerClickHandler
{

    public int ID;
    private NewInventory inv;

    void Start()
    {
        inv = GameObject.Find("InventorySystem").GetComponent<NewInventory>();
    }


    public void OnDrop(PointerEventData eventData)
    {

        List<Item> BeforListItems;
        List<GameObject> BeforListSlots;

        List<Item> AfterListItems;
        List<GameObject> AfterListSlots;

        ItemData droppedItem = eventData.pointerDrag.GetComponent<ItemData>();

        //string BeforLocation;
        string AfterLocation;
        AfterLocation = this.transform.parent.parent.name;


        //슬롯의 현재 위치를 확정
        if (AfterLocation == "Inventory")
        {
            AfterListItems = inv.GetInventoryItems();
            AfterListSlots = inv.GetInventorySlots();
        }
        else if (AfterLocation == "Equip")
        {
            AfterListItems = inv.GetEquipItems();
            AfterListSlots = inv.GetEquipSlots();
        }
        else
        {
            AfterListItems = inv.GetTreasureBoxItems();
            AfterListSlots = inv.GetTreasureBoxSlots();
        }

        //아이템 이동. 현재 끌고있는 아이템 기반.
        if (droppedItem.Location == "Inventory")
        {
            BeforListItems = inv.GetInventoryItems();
            BeforListSlots = inv.GetInventorySlots();
            if (AfterLocation == "Equip") // inven -> equip
            {    //장비창엔 배터리 아이템만 들어가야 한다.
                if (droppedItem.item.itemType == Item.ItemType.Battery)
                {
                    if (AfterListItems[ID].itemID == -1)
                        ItemExchange_withEmpty(droppedItem, BeforListItems, AfterListItems, AfterLocation);

                    else
                        ItemExchange_withItem(droppedItem, BeforListSlots, AfterLocation);
                }
            }
            else // inven -> inven or treasure
            {
                if (AfterListItems[ID].itemID == -1)
                {
                    ItemExchange_withEmpty(droppedItem, BeforListItems, AfterListItems, AfterLocation);
                    if (AfterLocation == "TreasureBox")
                    {
                        inv.TreasureBox.InsertItem(droppedItem.item, droppedItem.slot);
                    }
                }
                else
                {
                    if (AfterLocation == "TreasureBox")
                    {
                        inv.TreasureBox.RemoveItem(droppedItem.slot);
                        //remove(droppedItem.slot);
                        ItemExchange_withItem(droppedItem, BeforListSlots, AfterLocation);
                        //insert(droppedItem.slot);
                        inv.TreasureBox.InsertItem(droppedItem.item, droppedItem.slot);
                    }
                    else
                        ItemExchange_withItem(droppedItem, BeforListSlots, AfterLocation);
                }
            }
        }
        else if (droppedItem.Location == "Equip")
        {
            BeforListItems = inv.GetEquipItems();
            BeforListSlots = inv.GetEquipSlots();
            if (AfterLocation == "Equip") // equip -> equip
            {
                if (droppedItem.item.itemType == Item.ItemType.Battery)
                {

                    if (AfterListItems[ID].itemID == -1) //이동하려는 곳이 빈칸
                        ItemExchange_withEmpty(droppedItem, BeforListItems, AfterListItems, AfterLocation);



                    else
                        ItemExchange_withItem(droppedItem, BeforListSlots, AfterLocation);
                }
            }
            else //equip -> ivnen or treasure
            {
                if (AfterListItems[ID].itemID == -1) //이동하려는 곳이 빈칸
                {
                    ItemExchange_withEmpty(droppedItem, BeforListItems, AfterListItems, AfterLocation);
                    if (AfterLocation == "TreasureBox")
                    {
                        inv.TreasureBox.InsertItem(droppedItem.item, droppedItem.slot);
                        //insert(droppedItem.slot);
                    }
                }
                else //이동하려는 곳이 빈칸이 아님
                {
                    if (AfterLocation == "TreasureBox")
                    {
                        inv.TreasureBox.RemoveItem(droppedItem.slot);
                        //remove(droppedItem.slot);
                        ItemExchange_withItem(droppedItem, BeforListSlots, AfterLocation);
                        //insert(droppedItem.slot);
                        inv.TreasureBox.InsertItem(droppedItem.item, droppedItem.slot);
                    }
                    else
                        ItemExchange_withItem(droppedItem, BeforListSlots, AfterLocation);


                }
            }
        }
        else
        {
            //T박스 -> T박스
            BeforListItems = inv.GetTreasureBoxItems();
            BeforListSlots = inv.GetTreasureBoxSlots();
            if (AfterLocation == "Equip") // treasure -> equip
            {
                if (droppedItem.item.itemType == Item.ItemType.Battery)
                {
                    if (AfterListItems[ID].itemID == -1)
                    {
                        inv.TreasureBox.RemoveItem(droppedItem.slot);
                        //remove(droppedItem.slot); // 끌어서 아이템을 뺐으니 원래 있던 아이템을 제거
                        ItemExchange_withEmpty(droppedItem, BeforListItems, AfterListItems, AfterLocation); // 제거 후 해당 아이템 이동
                    }
                    else
                    {
                        int _beforSlot = droppedItem.slot;

                        inv.TreasureBox.RemoveItem(_beforSlot);
                        
                        //remove(_beforSlot);
                        ItemExchange_withItem(droppedItem, BeforListSlots, AfterLocation);

                        inv.TreasureBox.InsertItem(BeforListItems[_beforSlot], _beforSlot);
                        //insert(_beforSlot);
                    }
                }
            }
            else if (AfterLocation == "TreasureBox") // treasure -> treasure
            {
                if (AfterListItems[ID].itemID == -1)
                {
                    int _beforSlot = droppedItem.slot;

                    //inv.TreasureBox.RemoveItem(droppedItem.slot);
                    //remove(droppedItem.slot);
                    ItemExchange_withEmpty(droppedItem, BeforListItems, AfterListItems, AfterLocation);
                    //insert(droppedItem.slot);
                    //inv.TreasureBox.InsertItem(droppedItem.item, droppedItem.slot);
                    inv.TreasureBox.ChangeItem(_beforSlot, droppedItem.slot);
                }
                else
                {    
                    int _beforSlot = droppedItem.slot;

                    ItemExchange_withItem(droppedItem, BeforListSlots, AfterLocation);
                    // exchange(_beforSlot,droppedItem.slot);
                    inv.TreasureBox.ChangeItem(_beforSlot, droppedItem.slot);
                }
            }
            else // treasure -> inven
            {
                if (AfterListItems[ID].itemID == -1)
                {
                    inv.TreasureBox.RemoveItem(droppedItem.slot);
                    //remove(droppedItem.slot);
                    ItemExchange_withEmpty(droppedItem, BeforListItems, AfterListItems, AfterLocation);
                }
                else
                {
                    int _beforSlot = droppedItem.slot;
                    inv.TreasureBox.RemoveItem(_beforSlot);
                    //remove(droppedItem.slot);
                    ItemExchange_withItem(droppedItem, BeforListSlots, AfterLocation);
                    //insert(_beforSlot);
                    inv.TreasureBox.InsertItem(BeforListItems[_beforSlot], _beforSlot);
                }

            }

        }
    }

    //아이템 사용
    public void OnPointerClick(PointerEventData eventData)
    {

        List<Item> BeforListItems;
        List<GameObject> BeforListSlots;

        List<Item> AfterListItems;
        List<GameObject> AfterListSlots;

        Debug.Log(eventData.pointerCurrentRaycast.gameObject.name); // 아이템 데이터가 나옴 게임오브젝트까지 하면.

        ItemData test = eventData.pointerCurrentRaycast.gameObject.GetComponent<ItemData>();

        //eventData.pointerId == -2 -> 우클릭. -1 -> 왼클릭, 클릭 우클릭 제한없고 상관없다 하면 조건을 없애면 됨
        if (this.transform.childCount > 0 && test.Location != "TreasureBox" && eventData.pointerId == -2)
        {
            //Debug.Log("child not null!");

            string BeforLocation = test.Location;
            if (BeforLocation == "Inventory")
            {
                BeforListItems = inv.GetInventoryItems();
                BeforListSlots = inv.GetInventorySlots();
            }
            else //if (BeforLocation == "Equip")
            {
                BeforListItems = inv.GetEquipItems();
                BeforListSlots = inv.GetEquipSlots();
            }

            if (BeforLocation == "Inventory" && test.item.itemType == Item.ItemType.Battery)
            {
                string AfterLocation = "Equip";
                AfterListItems = inv.GetEquipItems();
                AfterListSlots = inv.GetEquipSlots();

                int MinBatteryID = -1;
                float MinBatterySize = 51;

                for (int i = 0; i < inv.GetEquipCount(); ++i)
                {
                    if (AfterListItems[i].itemID == -1) //장비창의 빈칸이 있다면
                    {
                        ItemExchange_withEmpty(test, BeforListItems, AfterListItems, AfterLocation, i);
                        test.SetLocation();
                        return;
                    }
                    else
                    {
                        if (AfterListItems[i].itemSize < MinBatterySize) //AfterListItems[i].itemType == Item.ItemType.Battery &&
                        {
                            MinBatterySize = AfterListItems[i].itemSize;
                            MinBatteryID = i;
                        }
                    }
                }

                ItemExchange_withItem(test, BeforListSlots, AfterLocation, MinBatteryID);
                test.SetLocation();
                return;
            }

            else if (BeforLocation == "Inventory" && test.item.itemType == Item.ItemType.Consumable)
            {
                PlayerManager Player = GameObject.Find("Player").GetComponent<PlayerManager>();
                if (Player._PosionOn)
                {
                    Player.TakePosion(test.item.itemSize);
                    BeforListItems[test.slot] = new Item();
                    Destroy(this.transform.GetChild(0).gameObject);
                }
            }

            else if (BeforLocation == "Equip" && test.item.itemType == Item.ItemType.Battery)
            {
                string AfterLocation = "Inventory";
                AfterListItems = inv.GetInventoryItems();
                AfterListSlots = inv.GetInventorySlots();

                int FirstBatteryID = -1;

                for (int i = 0; i < inv.GetInventoryCount(); ++i)
                {
                    if (AfterListItems[i].itemID == -1)
                    {
                        ItemExchange_withEmpty(test, BeforListItems, AfterListItems, AfterLocation, i);
                        test.SetLocation();
                        return;
                    }
                    else if (FirstBatteryID == -1)
                    {
                        if (AfterListItems[i].itemType == Item.ItemType.Battery)
                        {
                            FirstBatteryID = i; //인벤토리 내 최초 배터리의 위치
                        }
                    }
                }

                ItemExchange_withItem(test, BeforListSlots, AfterLocation, FirstBatteryID);
                test.SetLocation();
                return;
            }
        }

    }

    private void ItemExchange_withEmpty(ItemData droppedItem, List<Item> Befor, List<Item> After, string AfterLocation)
    {
        Befor[droppedItem.slot] = new Item();
        After[ID] = droppedItem.item;
        droppedItem.slot = ID;
        droppedItem.Location = AfterLocation;
    }
    private void ItemExchange_withEmpty(ItemData droppedItem, List<Item> Befor, List<Item> After, string AfterLocation, int id)
    {
        Befor[droppedItem.slot] = new Item();
        After[id] = droppedItem.item;
        droppedItem.slot = id;
        droppedItem.Location = AfterLocation;
    }

    private void ItemExchange_withItem(ItemData droppedItem, List<GameObject> Befor, string AfterLocation)
    {
        Transform item = this.transform.GetChild(0);
        item.GetComponent<ItemData>().slot = droppedItem.slot;
        item.GetComponent<ItemData>().Location = droppedItem.Location;
        item.transform.SetParent(Befor[droppedItem.slot].transform);
        item.transform.position = Befor[droppedItem.slot].transform.position;

        droppedItem.slot = ID;
        droppedItem.Location = AfterLocation;
        droppedItem.transform.SetParent(this.transform);
        droppedItem.transform.position = this.transform.position;
    }
    private void ItemExchange_withItem(ItemData droppedItem, List<GameObject> Befor, string AfterLocation, int id)
    {
        Transform item = this.transform.GetChild(0);
        item.GetComponent<ItemData>().slot = droppedItem.slot;
        item.GetComponent<ItemData>().Location = droppedItem.Location;
        item.transform.SetParent(Befor[droppedItem.slot].transform);
        item.transform.position = Befor[droppedItem.slot].transform.position;


        droppedItem.slot = id;
        droppedItem.Location = AfterLocation;
        droppedItem.transform.SetParent(this.transform);
        droppedItem.transform.position = this.transform.position;
    }
}