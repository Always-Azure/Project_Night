using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

/// <summary>
/// Inventory slot in all inventory.
/// Handle all procedure about item.(using, moving etc)
/// </summary>
/// <author> SangJun </author>
public class Slot : MonoBehaviour, IDropHandler, IPointerClickHandler
{
    public int ID;  // Slot ID

    private NewInventory inv;   // 인벤토리 참조

    private void Start()
    {
        inv = GameObject.Find("InventorySystem").GetComponent<NewInventory>();
    }

    /// <summary>
    /// When Draged Item is Droped on this slot. -> Finish the drag on this slot
    /// </summary>
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

        //끌고 있던 아이템이 인벤토리에 있던 아이템이라면
        if (droppedItem.Location == "Inventory")
        {
            //이전 슬롯 및 아이템 정보를 가져옴
            BeforListItems = inv.GetInventoryItems();
            BeforListSlots = inv.GetInventorySlots();

            //현재 위치가 장비창일 때,
            if (AfterLocation == "Equip") // inven -> equip
            {    
                //끌고있던 아이템이 배터리일 경우에만
                if (droppedItem.item.itemType == Item.ItemType.Battery)
                {
                    if (AfterListItems[ID].itemID == -1)
                        ItemExchangeWithEmptySlot(droppedItem, BeforListItems, AfterListItems, AfterLocation);

                    else
                        ItemExchangeWithItemSlot(droppedItem, BeforListSlots, AfterLocation);
                }
            }
            //현재 위치가 인벤토리 일 때,
            else if (AfterLocation == "Inventory")
            {
                if (AfterListItems[ID].itemID == -1)
                    ItemExchangeWithEmptySlot(droppedItem, BeforListItems, AfterListItems, AfterLocation);

                else
                    ItemExchangeWithItemSlot(droppedItem, BeforListSlots, AfterLocation);
            }
            //현재 위치가 보물상자 일 때,
            else // inven -> inven or treasure
            {
                //현재 위치가 빈칸이라면
                if (AfterListItems[ID].itemID == -1)
                {
                    ItemExchangeWithEmptySlot(droppedItem, BeforListItems, AfterListItems, AfterLocation);
                    inv.treasureBox.InsertItem(droppedItem.item, droppedItem.slot);
                }
                //현재위치에 아이템이 있다면
                else
                {
                    inv.treasureBox.RemoveItem(droppedItem.slot);
                    ItemExchangeWithItemSlot(droppedItem, BeforListSlots, AfterLocation);
                    inv.treasureBox.InsertItem(droppedItem.item, droppedItem.slot);
                }
            }
        }
        //끌고 있던 아이템이 장비창에 있던 아이템이라면
        else if (droppedItem.Location == "Equip")
        {
            //이전 슬롯 및 아이템 정보를 가져옴
            BeforListItems = inv.GetEquipItems();
            BeforListSlots = inv.GetEquipSlots();

            //현재 위치가 장비창이라면
            if (AfterLocation == "Equip") // equip -> equip
            {
                //끌고있던 아이템이 배터리일 경우에만
                if (droppedItem.item.itemType == Item.ItemType.Battery)
                {
                    if (AfterListItems[ID].itemID == -1) //이동하려는 곳이 빈칸
                        ItemExchangeWithEmptySlot(droppedItem, BeforListItems, AfterListItems, AfterLocation);
                    else
                        ItemExchangeWithItemSlot(droppedItem, BeforListSlots, AfterLocation);
                }
            }
            //현재 위치가 인벤토리 창이라면 (장비 -> 인벤토리라면 어떤 아이템이든지 가능)
            else if(AfterLocation == "Inventory")
            {
                if (AfterListItems[ID].itemID == -1) //이동하려는 곳이 빈칸
                    ItemExchangeWithEmptySlot(droppedItem, BeforListItems, AfterListItems, AfterLocation);
                else
                    ItemExchangeWithItemSlot(droppedItem, BeforListSlots, AfterLocation);
            }

            //현재 위치가 보물 상자 창이라면
            else //equip -> ivnen or treasure
            {
                if (AfterListItems[ID].itemID == -1) //이동하려는 곳이 빈칸
                {
                    ItemExchangeWithEmptySlot(droppedItem, BeforListItems, AfterListItems, AfterLocation);
                    inv.treasureBox.InsertItem(droppedItem.item, droppedItem.slot); // 정보 연동
                }
                else //이동하려는 곳이 빈칸이 아님
                {
                    inv.treasureBox.RemoveItem(droppedItem.slot); // 정보 연동
                    ItemExchangeWithItemSlot(droppedItem, BeforListSlots, AfterLocation);
                    inv.treasureBox.InsertItem(droppedItem.item, droppedItem.slot); // 정보연동
                }
            }
        }
        //끌고 있던 아이템이 보물상자에 있던 아이템이라면
        else
        {
            BeforListItems = inv.GetTreasureBoxItems();
            BeforListSlots = inv.GetTreasureBoxSlots();

            //보물상자 -> 장비창
            if (AfterLocation == "Equip") // treasure -> equip
            {
                if (droppedItem.item.itemType == Item.ItemType.Battery)
                {
                    if (AfterListItems[ID].itemID == -1)
                    {
                        inv.treasureBox.RemoveItem(droppedItem.slot);
                        ItemExchangeWithEmptySlot(droppedItem, BeforListItems, AfterListItems, AfterLocation); // 제거 후 해당 아이템 이동
                    }
                    else
                    {
                        int _beforSlot = droppedItem.slot;

                        inv.treasureBox.RemoveItem(_beforSlot);
                        ItemExchangeWithItemSlot(droppedItem, BeforListSlots, AfterLocation);
                        inv.treasureBox.InsertItem(BeforListItems[_beforSlot], _beforSlot);
                    }
                }
            }
            //보물상자 -> 보물상자
            else if (AfterLocation == "TreasureBox") // treasure -> treasure
            {
                if (AfterListItems[ID].itemID == -1)
                {
                    int _beforSlot = droppedItem.slot;

                    ItemExchangeWithEmptySlot(droppedItem, BeforListItems, AfterListItems, AfterLocation);
                    inv.treasureBox.ChangeItem(_beforSlot, droppedItem.slot);
                }
                else
                {    
                    int _beforSlot = droppedItem.slot;

                    ItemExchangeWithItemSlot(droppedItem, BeforListSlots, AfterLocation);
                    inv.treasureBox.ChangeItem(_beforSlot, droppedItem.slot);
                }
            }
            //보물상자 -> 인벤토리창
            else // treasure -> inven
            {
                if (AfterListItems[ID].itemID == -1)
                {
                    inv.treasureBox.RemoveItem(droppedItem.slot);
                    ItemExchangeWithEmptySlot(droppedItem, BeforListItems, AfterListItems, AfterLocation);
                }
                else
                {
                    int _beforSlot = droppedItem.slot;
                    inv.treasureBox.RemoveItem(_beforSlot);
                    ItemExchangeWithItemSlot(droppedItem, BeforListSlots, AfterLocation);
                    inv.treasureBox.InsertItem(BeforListItems[_beforSlot], _beforSlot);
                }

            }

        }
    }

    /// <summary>
    /// Click this slot
    /// </summary>
    public void OnPointerClick(PointerEventData eventData)
    {
        List<Item> BeforListItems;
        List<GameObject> BeforListSlots;

        List<Item> AfterListItems;
        List<GameObject> AfterListSlots;

        Debug.Log(eventData.pointerCurrentRaycast.gameObject.name);

        ItemData UseItem = eventData.pointerCurrentRaycast.gameObject.GetComponent<ItemData>();

        //eventData.pointerId == -2 -> 우클릭. -1 -> 왼클릭, 클릭 우클릭 제한없고 상관없다 하면 조건을 없애면 됨
        if (this.transform.childCount > 0 && UseItem.Location != "TreasureBox" && eventData.pointerId == -2)
        {

            string BeforLocation = UseItem.Location;
            //아이템의 위치 판별
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

            //인벤토리에서 배터리 아이템 사용 -> 장비창에 배터리 등록 혹은 가장 용량이 낮은 배터리와 교체
            if (BeforLocation == "Inventory" && UseItem.item.itemType == Item.ItemType.Battery)
            {
                string AfterLocation = "Equip";
                AfterListItems = inv.GetEquipItems();
                AfterListSlots = inv.GetEquipSlots();

                int MinBatteryID = -1;
                float MinBatterySize = 51;

                for (int i = 0; i < inv.GetEquipCount(); ++i)
                {
                    if (AfterListItems[i].itemID == -1) 
                    {
                        ItemExchangeWithEmptySlot(UseItem, BeforListItems, AfterListItems, AfterLocation, i); // 현재 위치가 빈칸일 떄, 아이템을 빈칸과 교체
                        UseItem.SetLocation();
                        return;
                    }
                    else
                    {
                        if (AfterListItems[i].itemSize < MinBatterySize) 
                        {
                            MinBatterySize = AfterListItems[i].itemSize;
                            MinBatteryID = i; // 가장 용량이 낮은 아이템 위치 정보 저장
                        }
                    }
                }

                ItemExchangeWithItemSlot(UseItem, BeforListSlots, AfterLocation, MinBatteryID);//아이템 정보를 이용, 아이템 교체
                UseItem.SetLocation();
                return;
            }

            //인벤토리에서 먹을수 있는 타입의 아이템 사용 -> 플레이어 체력 회복
            else if (BeforLocation == "Inventory" && UseItem.item.itemType == Item.ItemType.Consumable)
            {
                PlayerManager Player = GameObject.Find("Player").GetComponent<PlayerManager>();
                if (Player._PosionOn)
                {
                    Player.TakePosion(UseItem.item.itemSize);
                    BeforListItems[UseItem.slot] = new Item();
                    Destroy(this.transform.GetChild(0).gameObject);
                }
            }

            //인벤토리에서 힌트 아이템 사용 -> 집 리소스의 실린더 활성화
            else if (BeforLocation == "Inventory" && UseItem.item.itemType == Item.ItemType.Hint)
            {
                House HintUse = GameObject.Find("House(Clone)").GetComponent<House>();
                HintUse.UseHint();
                BeforListItems[UseItem.slot] = new Item();
                Destroy(this.transform.GetChild(0).gameObject);

            }

            //장비창에서 배터리 사용, -> 인벤토리 빈 칸 혹은 최초 배터리와 교환
            else if (BeforLocation == "Equip" && UseItem.item.itemType == Item.ItemType.Battery)
            {
                string AfterLocation = "Inventory";
                AfterListItems = inv.GetInventoryItems();
                AfterListSlots = inv.GetInventorySlots();

                int FirstBatteryID = -1;

                for (int i = 0; i < inv.GetInventoryCount(); ++i)
                {
                    if (AfterListItems[i].itemID == -1)
                    {
                        ItemExchangeWithEmptySlot(UseItem, BeforListItems, AfterListItems, AfterLocation, i);
                        UseItem.SetLocation();
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

                ItemExchangeWithItemSlot(UseItem, BeforListSlots, AfterLocation, FirstBatteryID);
                UseItem.SetLocation();
                return;
            }
        }

    }

    /// <summary>
    /// Handle change slot data with empty slot.
    /// </summary>
    /// <param name="droppedItem"> Moved item </param>
    /// <param name="Befor"> Item list before move </param>
    /// <param name="After"> Item list after move </param>
    /// <param name="AfterLocation"> destination </param>
    private void ItemExchangeWithEmptySlot(ItemData droppedItem, List<Item> Befor, List<Item> After, string AfterLocation)
    {
        Befor[droppedItem.slot] = new Item();
        After[ID] = droppedItem.item;
        droppedItem.slot = ID;
        droppedItem.Location = AfterLocation;
    }
    /// <summary>
    /// Handle change slot data with empty slot.
    /// This method will use when use item.
    /// </summary>
    /// <param name="droppedItem"> Moved item </param>
    /// <param name="Befor"> Item list before move </param>
    /// <param name="After"> Item list after move </param>
    /// <param name="AfterLocation"> destination </param>
    /// <param name="id"> Item ID </param>
    private void ItemExchangeWithEmptySlot(ItemData droppedItem, List<Item> Befor, List<Item> After, string AfterLocation, int id)
    {
        Befor[droppedItem.slot] = new Item();
        After[id] = droppedItem.item;
        droppedItem.slot = id;
        droppedItem.Location = AfterLocation;
    }

    /// <summary>
    /// Handle change slot data with other item slot
    /// </summary>
    /// <param name="droppedItem"> Moved item </param>
    /// <param name="Befor"> Item list before move </param>
    /// <param name="AfterLocation"> destination </param>
    private void ItemExchangeWithItemSlot(ItemData droppedItem, List<GameObject> Befor, string AfterLocation)
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
    /// <summary>
    /// Handle change slot data with other item slot
    /// This method will use when use item.
    /// </summary>
    /// <param name="droppedItem"> Moved item </param>
    /// <param name="Befor"> Item list before move </param>
    /// <param name="AfterLocation"> destination </param>
    /// /// <param name="id"> Item ID </param>
    private void ItemExchangeWithItemSlot(ItemData droppedItem, List<GameObject> Befor, string AfterLocation, int id)
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