using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text.RegularExpressions;
using UnityEngine.UI;

public class Inventory : MonoBehaviour {

    // bool형 -> 현재 상태 체그

    //인벤토리 및 장비창 개수.
    private int invenslotX = 4;
    private int invenslotY = 2;

    private int equipslot = 3;

    private ItemDatabase database;
    public List<Item> Inven = new List<Item>();
    public List<Item> Equip = new List<Item>();

    public GUISkin skin; // 인벤 및 장비창 배경스킨
    public Image Image_InvenBG;
    public Image Image_EquipBG;
    public Texture2D BGImage_Inven;
    private Sprite InvenSprite;
    private Sprite EquipSprite;

    public bool ShowInven = false; // 인벤토리 온오프
    public bool ShowEquip = false; // 장비창 온오프

    private Rect InvenRect;
    private Rect EquipRect;

    private bool showTooltipInven; // 인벤토리 내 아이템 설명 온오프
    private bool showTooltipEquip; // 장비창 내 아이템 설명 온오프

    private string tooltip; // 아이템 설명 전체
    private string tooltipDesc; // 아이템 설명

    private bool draggingItem; // 아이템 드래그 온오프
    private Item draggedItem; // 드래그하는 아이템
    private int prevIndex; // 이전에 있던 위치
    private int nowIndex = 0;
    private bool locateChecker;

    // Use this for initialization
    void Start () {
        for(int i = 0; i<(invenslotX*invenslotY); ++i)
            Inven.Add(new Item());
        for(int i =0; i<equipslot; ++i)
            Equip.Add(new Item());


        InvenRect = new Rect(
            ((Screen.width / 5) * 3), //시작 x 좌표
            100, // 시작 y 좌표
            (invenslotX * 50), //너비 (왼 -> 오)
            (invenslotY * 50) //높이 (위 -> 아래)
            );

        //Rect slotRect = new Rect(((Screen.width / 5) * 3) + (x * 50), 100 + (y * 50), 50, 50);


        EquipRect = new Rect(((Screen.width / 10)),
            100,
            (equipslot * 50),
            50);
        //Rect slotRect = new Rect(((Screen.width / 10)) + (i * 60), 100, 50, 50);

        //BGImage_Inven = Image_InvenBG;
        InvenSprite = Resources.Load<Sprite>("Image_InvenBG");
        EquipSprite = Resources.Load<Sprite>("Image_EquipBG");

        //IInvenBG = GameObject.Find("Resource/Image/PN_Image_Inventory/InvenBG(1)").GetComponent<Image>();
        //IInvenBG.sprite = Resources.Load("Resource/Image/PN_Image_Inventory/"+"InvenBG(1)") as Image;


        database = GameObject.FindGameObjectWithTag("Item Database").GetComponent<ItemDatabase>();
        //AddItem(2);
        AddItem(1);
        AddItem(1);
        AddItem(1);
    }

    // Update is called once per frame
    void Update () {
        //Input.GetButtonDown("Inventory") or Input.GetKeyDown(KeyCode.I)

        //if (Input.GetKeyDown(KeyCode.I)) // i키 입력 -> 인벤토리 활성화
        if (Input.GetButtonDown("Inventory"))
        {
            ShowInven = !ShowInven;
        }
        ///Equipment
        //if (Input.GetKeyDown(KeyCode.E)) // e키 입력 -> 장비창 활성화
        if (Input.GetButtonDown("Equipment"))
        {
            ShowEquip = !ShowEquip;
        }
    }

    void OnGUI()
    {

        tooltip = "";
        GUI.skin = skin;

        if (ShowInven == true)
        {
            DrawInventory();
            if (showTooltipInven)
            {
                GUI.Box(new Rect(Event.current.mousePosition.x, Event.current.mousePosition.y, 200, 200), tooltip);//, tooltip,skin.GetStyle("tooltip"));
            }
        }

        if (ShowEquip == true)
        {
            //장비창 열기
            DrawEquip();
            if (showTooltipEquip)
            {
                //아이템 설명 출력
                GUI.Box(new Rect(Event.current.mousePosition.x, Event.current.mousePosition.y + 15.0f, 200, 200), tooltip);//, tooltip,skin.GetStyle("tooltip"));
            }
        }

        //끌고있는 상태인가
        if (draggingItem)
        {
            GUI.DrawTexture(new Rect(Event.current.mousePosition.x, Event.current.mousePosition.y, 50, 50), draggedItem.itemIcon);
        }
    }


    void DragManager()
    {
        //locateChecker : true -> Inven, false -> Equip

        Event currentEvent = Event.current;

        draggingItem = !draggingItem;


        //if (currentEvent.type == EventType.mouseUp && draggingItem)
        //{

        if (locateChecker == true) // Inven -> ??
        {
            if (InvenRect.Contains(currentEvent.mousePosition))
            {
                Debug.Log("mouse - inven -> inven");
                Inven[prevIndex] = Inven[nowIndex];
                Inven[nowIndex] = draggedItem;
                draggedItem = null;
                draggingItem = false;
            }
            else if (EquipRect.Contains(currentEvent.mousePosition))
            {
                Debug.Log("mouse - inven -> equip");
                if (draggedItem.itemType == Item.ItemType.Battery)
                {
                    Inven[prevIndex] = Equip[nowIndex];
                    Equip[nowIndex] = draggedItem;
                    draggedItem = null;
                    draggingItem = false;
                }
                else
                {
                    Inven[prevIndex] = draggedItem;
                    draggedItem = null;
                    draggingItem = false;
                }
            }
            else
            {
                Debug.Log("mouse - inven -> other");
                Inven[prevIndex] = new Item();
                draggedItem = null;
                draggingItem = false;
            }
        }

        else if (locateChecker == true) // Equip -> ??
        {
            if (InvenRect.Contains(currentEvent.mousePosition))
            {
                Debug.Log("mouse - equip -> inven");
                if (draggedItem.itemType == Item.ItemType.Battery)
                {
                    Equip[prevIndex] = Inven[nowIndex];
                    Inven[nowIndex] = draggedItem;
                    draggedItem = null;
                    draggingItem = false;
                }
                else
                {
                    Equip[prevIndex] = draggedItem;
                    draggedItem = null;
                    draggingItem = false;
                }
            }
            else if (EquipRect.Contains(currentEvent.mousePosition))
            {
                Debug.Log("mouse - equip -> equip");
                Equip[prevIndex] = Equip[nowIndex];
                Equip[nowIndex] = draggedItem;
                draggedItem = null;
                draggingItem = false;
            }
            else
            {
                Debug.Log("mouse - equip -> other");
                Inven[prevIndex] = new Item();
                draggedItem = null;
                draggingItem = false;
            }
        }
        //}
    }

    void DrawInventory()
    {
        Event currentEvent = Event.current;
        int index = 0;
        for (int y = 0; y < invenslotY; ++y)
        {
            for (int x = 0; x < invenslotX; ++x)
            {
                //인벤토리 백그라운드 이미지 출력
                //Rect slotRect = new Rect(((Screen.width / 5) * 3) + (x * 50), 100 + (y * 50), 50, 50);
                //IInvenBG.sprite = Resources.Load("Resource/Image/PN_Image_Inventory/InvenBG(1)") as Image;
                //Image_InvenBG.overrideSprite = InvenSprite;
                GUI.Box(InvenRect,BGImage_Inven);



                // 인벤토리 위치
                Rect slotRect = new Rect(((Screen.width / 5) * 3) + (x * 50), 100 + (y * 50), 50, 50);
                // 해당 위치에 인벤토리 배경 이미지 그리기
                GUI.Box(slotRect, "", skin.GetStyle("Slot"));

                if (Inven[index].itemName != null) // 해당 위치에 아이템이 있다면
                {
                    GUI.DrawTexture(slotRect, Inven[index].itemIcon); // 해당 위치에 아이템 그리기
                    if (slotRect.Contains(currentEvent.mousePosition)) // 해당 위치에 마우스 커서가 있다면 -> 아이템 칸 한 칸 영역 내에 마우스 커서가 있으면
                    {
                        CreateTooltip(Inven[index]); // 툴팁 그리기
                        showTooltipInven = true; // 인벤토리 퉅팁 온

                        //마우스 드래그 이벤트 시작
                        if (currentEvent.button == 0 && currentEvent.type == EventType.MouseDrag && !draggingItem)
                        {
                            draggingItem = true; // 드래그 온
                            prevIndex = index; // 인벤토리 아이템의 위치
                            draggedItem = Inven[index]; // 아이템 내용 저장
                            Inven[index] = new Item(); // 빈 공간으로 전환
                            locateChecker = true;
                        }

                        //마우스 드래그 이벤트 종료 -1. 같은 인벤토리 안
                        if (currentEvent.type == EventType.MouseUp && draggingItem)
                        {
                            if (locateChecker == false)
                            {
                                Equip[prevIndex] = Inven[index];
                                Inven[index] = draggedItem;
                            }
                            else
                            {
                                Inven[prevIndex] = Inven[index];
                                Inven[index] = draggedItem;
                            }
                            draggedItem = null;
                            draggingItem = false;
                        }

                        //아이템 왼클릭. -> 아이템 사용
                        //아이템 클릭 -> 변동 이동 그런거 없음
                        if (currentEvent.isMouse && currentEvent.type == EventType.MouseDown && currentEvent.button == 1 && !draggingItem)//1 = right, 0 = left
                        {

                            if (Inven[index].itemType == Item.ItemType.Consumable) //해당 아이템 == 회복아이템
                            {
                                //회복 연동
                                Inven[index] = new Item(); // 비워줌
                                Debug.Log("use Posion");
                            }

                            if (Inven[index].itemType == Item.ItemType.Battery) // 해당 아이템 == 배터리
                            {
                                Inven[index] = AddEquip(Inven[index]); //아이템을 장비창에 등록

                            }

                        }

                    }

                }

                else // 해당위치에 아이템이 없다면.
                {
                    //아무것도 없는 곳에 아이템을 놓는다.
                    if (slotRect.Contains(currentEvent.mousePosition))
                    {
                        if (currentEvent.type == EventType.MouseUp && draggingItem)
                        {
                            Inven[index] = draggedItem;
                            draggedItem = null;
                            draggingItem = false;
                        }
                    }
                }

                if (tooltip == "")
                {
                    showTooltipInven = false;
                }


                //ItemChecker(Inven[index], false, index, prevIndex);
                //ItemChecker(Inven[index], false);

                ++index;

                

            }
        }
    }

    //인벤토리 출력
    /*
    void DrawInventory()
    {
        Event currentEvent = Event.current;
        int index = 0;
        for (int y = 0; y < invenslotY; ++y)
        {
            for (int x = 0; x < invenslotX; ++x)
            {
                // 인벤토리 위치
                Rect slotRect = new Rect(((Screen.width / 5) * 3) + (x * 60), 100 + (y * 60), 50, 50);
                // 해당 위치에 인벤토리 배경 이미지 그리기
                GUI.Box(slotRect, "", skin.GetStyle("Slot"));

                if (Inven[index].itemName != null) // 해당 위치에 아이템이 있다면
                {
                    GUI.DrawTexture(slotRect, Inven[index].itemIcon); // 해당 위치에 아이템 그리기
                    if (slotRect.Contains(currentEvent.mousePosition)) // 해당 위치에 마우스 커서가 있다면
                    {
                        CreateTooltip(Inven[index]); // 툴팁 그리기
                        showTooltipInven = true; // 인벤토리 퉅팁 온

                        //마우스 드래그 이벤트 시작
                        if (currentEvent.button == 0 && currentEvent.type == EventType.mouseDrag && !draggingItem)
                        {
                            draggingItem = true; // 드래그 온
                            prevIndex = index; // 인벤토리 아이템의 위치
                            draggedItem = Inven[index]; // 아이템 내용 저장
                            Inven[index] = new Item(); // 빈 공간으로 전환
                        }
                        //마우스 드래그 이벤트 종료 -1. 같은 인벤토리 안
                        if (currentEvent.type == EventType.mouseUp && draggingItem)
                        {
                            Inven[prevIndex] = Inven[index];
                            Inven[index] = draggedItem;
                            draggedItem = null;
                            draggingItem = false;
                        }

                        //아이템 왼클릭. -> 아이템 사용
                        //아이템 클릭 -> 변동 이동 그런거 없음

                        if (currentEvent.isMouse && currentEvent.type == EventType.mouseDown && currentEvent.button == 0 && !draggingItem)//1 = right, 0 = left
                        {

                            if (Inven[index].itemType == Item.ItemType.Consumable) //해당 아이템 == 회복아이템
                            {
                                //회복 연동
                            }

                            if (Inven[index].itemType == Item.ItemType.Battery) // 해당 아이템 == 배터리
                            {
                                Inven[index] = AddEquip(Inven[index]); //아이템을 장비창에 등록
                            }
                        }

                        ////아이템 왼클릭 -> 아이템사용
                        //if (currentEvent.isMouse && currentEvent.type == EventType.mouseDown && currentEvent.button == 0)
                        //{
                        //        //이큅창에 있다 -> 마시는거나 수류탄이면 등록후 빈공간 반환 및 저장
                        //        //              -> 무기가 이미 있으니 선택한 무기를 다시 반환시킴
                        //        //이큅창에 없다 -> 새로 아이템을 등록하고 빈 공간을 반환.
                        //        //이큅창에 없는데 가득찼다 -> 넣으려던 아이템을 다시 반환한다. 
                        //}

                    }

                }

                else // 해당위치에 아이템이 없다면.
                {
                    ////아무것도 없는 곳에 아이템을 놓는다.
                    //if (slotRect.Contains(currentEvent.mousePosition))
                    //{
                    //    if (currentEvent.type == EventType.mouseUp && draggingItem)
                    //    {

                    //        Inven[index] = draggedItem;
                    //        draggedItem = null;
                    //        draggingItem = false;
                    //    }
                    //}
                }

                if (tooltip == "")
                {
                    showTooltipInven = false;
                }

                ++index;
            }
        }
    }
    */

    //
    void DrawEquip()
    {
        Event currentEvent = Event.current;
        int index = 0;

        for (int i = 0; i < equipslot; ++i)
        {
            Rect slotRect = new Rect(((Screen.width / 10)) + (i * 50), 100, 50, 50);
            GUI.Box(slotRect, "", skin.GetStyle("Slot"));

            if (Equip[i].itemName != null) //contain item
            {
                GUI.DrawTexture(slotRect, Equip[i].itemIcon);
                if (slotRect.Contains(currentEvent.mousePosition))
                {
                    CreateTooltip(Equip[i]);
                    showTooltipEquip = true;

                    //아이템 끌기 시작
                    if (currentEvent.button == 0 && currentEvent.type == EventType.MouseDrag && !draggingItem)
                    {
                        draggingItem = true;
                        prevIndex = i;
                        draggedItem = Equip[i];
                        Equip[i] = new Item();
                        locateChecker = false;
                    }

                    if (currentEvent.type == EventType.MouseUp && draggingItem)// && slotRect.Contains(currentEvent.mousePosition))
                    {
                        if (locateChecker == false)
                        {
                            //inven[previndex] <-> equip[i]
                            Equip[prevIndex] = Equip[i];
                            Equip[i] = draggedItem;
                        }
                        else
                        {
                            Inven[prevIndex] = Equip[i];
                            Equip[i] = draggedItem;
                        }
                        draggedItem = null;
                        draggingItem = false;
                    }

                    //마우스 오른클릭 -> 인벤토리로 이동
                    //마우스 클릭 -> 이벤트 없음

                    if (currentEvent.isMouse && currentEvent.type == EventType.MouseDown && currentEvent.button == 1)//1 = right, 0 = left
                    {
                        Equip[index] = Addinven(Equip[index]);
                    }

                    //else if (currentEvent.isMouse && currentEvent.type == EventType.mouseDown && currentEvent.button == 1)
                    //{
                    //    Equip[index] = Addinven(Equip[index]);
                    //}
                }
            }

            else
            {
                if (currentEvent.type == EventType.MouseUp && draggingItem && slotRect.Contains(currentEvent.mousePosition))
                {
                    if (locateChecker == false)
                    {
                        Equip[index] = draggedItem;
                        draggedItem = null;
                        draggingItem = false;
                    }

                    else
                    {
                        if(draggedItem.itemType == Item.ItemType.Battery)
                        {
                            Equip[index] = draggedItem;
                            draggedItem = null;
                            draggingItem = false;
                        }
                        else
                        {
                            Inven[prevIndex] = draggedItem;
                            draggedItem = null;
                            draggingItem = false;
                        }
                    }
                }

                //if (!(EquipRect.Contains(currentEvent.mousePosition)))
                //{
                //    if (currentEvent.type == EventType.mouseUp && draggingItem && draggedItem.itemType == Item.ItemType.Battery)
                //    {
                //        Equip[i] = draggedItem;
                //        draggedItem = null;
                //        draggingItem = false;
                //    }
                //    if (locateChecker == false)
                //    {
                //    }
                //}
            }

            if (tooltip == "")
            {
                showTooltipEquip = false;
            }

            ++index;

        }
    }


    //툴팁설명창
    string CreateTooltip(Item item)
    {
        tooltip = "<color=#ff3030>" + item.itemName + "\n\n" + "</color>";
        tooltipDesc = "<color=#ffffff>" + item.itemDesc + "\n\n" + "</color>";
        //tooltip3 = "<color=#ffffff>" + "Count : " + item.count + "</color>";

        string[] lines = Regex.Split(tooltipDesc, "\n"); // 아이템 설명의 구분자 : @

        foreach (string line in lines)
        {
            tooltip += line + "\n";
        }

        //tooltip += tooltip2;
        //tooltip += "<color=#fff319>" + "Count : " + item.count + "</color>";

        return tooltip;
    }

    //아이템 추가
    public void AddItem(int id)
    {
        //인벤토리에 아이템이 존재하지 않음.
        for (int i = 0; i < Inven.Count; ++i) //전체 인벤토리에서
        {
            if (Inven[i].itemName == null) //빈곳을 찾고,
            {
                for (int j = 0; j < database.items.Count; ++j) // DB를 뒤짐.
                {
                    if (database.items[j].itemID == id) // 같은 이름의 아이템을 찾음
                    {
                        Inven[i] = database.items[j]; // 새로 내용 저장
                        break;
                    }
                }
                break;
            }
        }
    }

    //인벤창 -> 장비창
    Item AddEquip(Item item)
    {
        //인벤창 -> 장비창
        //오직 배터리만
        //장비창에 빈칸이 있다 -> 배터리 추가 + 배터리 연동
        //장비창에 빈칸이 없다 -> 잔여전력 가장 낮은 배터리와 교체 or 그냥 맨 위에거랑 교체

        bool itemswitch = false;
        Item minBattery = new Item();
        int minBatteryloc = 0;

        //if (Equip.Count == equipslot) { } // 비었다 해도 빈칸으로 있긴 있음. 무쓸모


        for (int i = 0; i < equipslot; ++i)
        {
            if (Equip[i].itemID == -1)
            {
                Equip[i] = item;
                itemswitch = true;
                //배터리 연동
                break;
            }

            if (Equip[i].itemSize < minBattery.itemSize)
            {
                minBattery = Equip[i];
                minBatteryloc = i;
            }

        }

        if(itemswitch == true)
        {
            return (new Item());
        }
        else
        {
            Item switching = minBattery;
            Equip[minBatteryloc] = item;
            return switching;
        }


    }


    //장비창 -> 인벤창
    Item Addinven(Item item)
    {
        //장비창 -> 인벤창
        //그냥 빼는 용도임. 교체가 아님.
        //인벤창에 빈 칸이 있다 -> 빈칸에 넣는다.
        //인벤창에 빈 칸이 없다 -> 변동이 없다.

        for(int i = 0; i < (invenslotX * invenslotY); ++i)
        {
            if(Inven[i].itemID == -1) //비어있다.
            {
                Inven[i] = item;
                return (new Item());
            }
        }

        return item;
    }

}
