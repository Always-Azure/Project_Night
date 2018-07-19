using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;


public class ItemData : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler {

    public Item item;
    public int Amount = 1; // basic item count 1

    private NewInventory inv;

    private Vector2 offset;
    public int slot;
    public string Location;

    void Start()
    {
        inv = GameObject.Find("InventorySystem").GetComponent<NewInventory>();
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if(item != null)
        {
            offset = eventData.position - new Vector2(this.transform.position.x, this.transform.position.y);
            this.transform.SetParent(this.transform.parent.parent.parent.parent);//move SlotPanel. not hide other slots
            this.transform.position = eventData.position - offset; // mouse position
            GetComponent<CanvasGroup>().blocksRaycasts = false;
        }
        //throw new System.NotImplementedException();
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (item != null)
        {
            this.transform.position = eventData.position - offset; // mouse position = draged Slot position
        }
        //throw new System.NotImplementedException();
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        List<GameObject> AfterSlots;
        if (Location == "Inventory")
            AfterSlots = GameObject.Find("InventorySystem").GetComponent<NewInventory>().GetInventorySlots();
        else if (Location == "Equip")
            AfterSlots = GameObject.Find("InventorySystem").GetComponent<NewInventory>().GetEquipSlots();
        else
            AfterSlots = GameObject.Find("InventorySystem").GetComponent<NewInventory>().GetTreasureBoxSlots();
        

        this.transform.SetParent(AfterSlots[slot].transform);
        this.transform.position = AfterSlots[slot].transform.position;
        //this.transform.SetParent(inv.InventorySlots[slot].transform);
        //this.transform.position = inv.InventorySlots[slot].transform.position;
        GetComponent<CanvasGroup>().blocksRaycasts = true;
        //throw new System.NotImplementedException();
    }

    public void SetLocation()
    {
        List<GameObject> AfterSlots;

        if (Location == "Inventory")
            AfterSlots = GameObject.Find("InventorySystem").GetComponent<NewInventory>().GetInventorySlots();
        else if (Location == "Equip")
            AfterSlots = GameObject.Find("InventorySystem").GetComponent<NewInventory>().GetEquipSlots();
        else
            AfterSlots = GameObject.Find("InventorySystem").GetComponent<NewInventory>().GetTreasureBoxSlots();


        this.transform.SetParent(AfterSlots[slot].transform);
        this.transform.position = AfterSlots[slot].transform.position;
    }
}
