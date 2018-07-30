using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

/// <summary>
/// Process when Item on mouse drag or move to other inventory slot.
/// </summary>
/// <author> SangJun </author>
public class ItemData : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler {

    public Item item;   // Item
    public string Location;
    public int Amount = 1; // basic item count 1
    public int slot;

    private Vector2 _offset;

    void Start()
    {
    }

    /// <summary>
    /// Start Point to Mouse Drag
    /// </summary>
    public void OnBeginDrag(PointerEventData eventData)
    {
        if(item != null)
        {
            _offset = eventData.position - new Vector2(this.transform.position.x, this.transform.position.y);
            this.transform.SetParent(this.transform.parent.parent.parent.parent);//move SlotPanel. not hide other slots
            this.transform.position = eventData.position - _offset; // mouse position
            GetComponent<CanvasGroup>().blocksRaycasts = false;
        }
    }

    /// <summary>
    /// During Mouse Drag
    /// </summary>
    public void OnDrag(PointerEventData eventData)
    {
        if (item != null)
        {
            // -> mouse position = draged Slot position
            this.transform.position = eventData.position - _offset;
        }
    }

    /// <summary>
    /// End Point Mouse Drag
    /// </summary>
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

        GetComponent<CanvasGroup>().blocksRaycasts = true;
    }

    /// <summary>
    /// Set Item Location(Inventory, Equip, TreasureBox)
    /// </summary>
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
