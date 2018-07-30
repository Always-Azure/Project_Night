using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Handle general TreasureBox process
/// </summary>
/// <author> YeHun </author>
public class TreasureBox : MonoBehaviour
{
    // Property
    public STATE_BOX State { get { return state; } }
    public Item[] ItemList { get { return _itemList; } }

    private Item[] _itemList;   // 상자에 들어있는 아이템 정보.
    private int _itemSize; // 아이템 갯수.
    private int _itemMaxSize; // 최대 가질 수 있는 아이템 갯수.
    private GameObject Notice; // 상자에 접근했을 때, 나오는 안내문
    private STATE_BOX state; // 상자의 상태.
    private NewInventory _inven;    // inventory 정보.
    private Animator anim; // 애니메이션
    private AudioSource _audio; // 사운드
    private Dictionary<string, Sound> _sounds;  // 사운드 리스트

    private void Awake()
    {
        _itemMaxSize = 3;
        _itemList = new Item[_itemMaxSize];
        _itemSize = 0;
    }

    // Use this for initialization
    void Start()
    {
        // Initiate inner item.
        InitItem();
        DisableAllObjects();

        Notice = transform.Find("Canvas").gameObject;
        state = STATE_BOX.CLOSE;
        anim = GetComponent<Animator>();
        _inven = GameObject.Find("InventorySystem").GetComponent<NewInventory>();
        _audio = gameObject.AddComponent<AudioSource>();

        // Sound 초기화
        SoundInfo soundinfo = AudioManager.instance.SoundInfo;
        _sounds = soundinfo.GetSubDir("Objects").GetSubDir("TreasureBox").GetSoundList(_audio);
    }

    /// <summary>
    /// When insert item in the box, Get ItemList and Update ItemList
    /// </summary>
    /// <param name="item"> Item </param>
    /// <param name="idx"> Insert location index </param>
    /// <returns> true : Success, false : Fail </returns>
    public bool InsertItem(Item item, int idx)
    {
        // Max size 벗어나는지 확인.
        if (_itemSize > _itemMaxSize)
            return false;

        // idx범위를 벗어나는지 확인.
        if (idx < 0) return false;

        if (_itemList[idx] != null) return false;

        _itemList[idx] = item;
        _itemSize++;

        Draw(DRAWTYPE_BOX.INSERT, idx);

        return true;
    }
    public bool InsertItem(List<Item> itemList)
    {
        Debug.Log("InsertItem execute");

        // null값이 들어오는지 확인.
        if (itemList == null)
            return false;
        
        // insert된 위치 정보없이 아이템리스트가 업데이트 되었으니, 상자를 열었을 때 처럼 모든 아이템 떠오르게하기.
        Draw(DRAWTYPE_BOX.OPEN);

        return true;
    }
    public bool InsertItem(List<Item> itemList, int idx)
    {
        Debug.Log("InsertItem execute");

        // null값이 들어오는지 확인.
        if (itemList == null)
            return false;

        Draw(DRAWTYPE_BOX.INSERT, idx);

        return true;
    }

    /*
     * 
     * true - success change
     * false - fail change
     */
    /// <summary>
    /// change item location in itemlist.
    /// </summary>
    /// <param name="start"> Start location index </param>
    /// <param name="end"> End location index </param>
    /// <returns> true : Success, false : Fail </returns>
    public bool ChangeItem(int start, int end)
    {
        try
        {
            if (start < 0 || _itemMaxSize <= start) throw new System.Exception();    
            if (end < 0 || _itemMaxSize <= end) return false;
            if (start == end) return false;

            Item temp = _itemList[start];
            _itemList[start] = _itemList[end];
            _itemList[end] = temp;
        }
        catch (System.Exception ex)
        {
            Debug.Log("아이템을 바꾸는 과정에서 에러가 발생했습니다.");
            return false;
        }

        Draw(DRAWTYPE_BOX.REMOVE);

        return true;
    }

    /// <summary>
    /// remove item.
    /// </summary>
    /// <returns> true : Success, false : Fail </returns>
    public bool RemoveItem(int idx)
    {
        Debug.Log("RemoveItem execute");

        {
            if (_itemSize == 0) return false;

            if (idx < 0 || _itemMaxSize <= idx) return false;
        }

        _itemList[idx] = null;
        _itemSize--;

        Draw(DRAWTYPE_BOX.REMOVE);

        return true;
    }
    public bool RemoveItem(Item item, int idx)
    {
        Debug.Log("RemoveItem execute");

        // 예외처리
        {
            // 제거할 아이템이 없는 상황.
            if (_itemSize == 0)
                return false;

            // 범위를 벗어나는 idx 값.
            if (idx < 0 || idx > (_itemSize - 1))
                return false;
        }

        // 아이템 제거
        _itemList[idx] = null;

        Draw(DRAWTYPE_BOX.REMOVE);

        return true;
    }

    /// <summary>
    /// Update Item List
    /// </summary>
    /// <param name="itemList"></param>
    public void UpdateItemList(Item[] itemList)
    {
        Debug.Log("UpdateItemList execute");

        try
        {
            //_itemList = itemList;
            for(int i = 0; i<_itemMaxSize; i++)
            {
                _itemList[i] = itemList[i];
            }

            UpdateItemSize();
        }
        catch
        {
            Debug.Log("아이템리스트를 업데이트하는 과정에서 에러가 발생했습니다.");
        }
    }

    /// <summary>
    /// Initiate inner item randomly.
    /// </summary>
    private void InitItem()
    {
        int random_itemCnt = Random.Range(1,3);
        
        // type을 랜덤하게 받아서 아이템 갯수만큼 생성. 아이템 배열에 넣어준다.
        for (int i = 0; i < random_itemCnt; i++)
        {
            Item tmp;
            int random_type = Random.Range(0, 2);

            switch (random_type)
            {
                case 0:
                    tmp = ItemDatabase.instance.FetchItemByID(0);
                    break;

                case 1:
                    tmp = ItemDatabase.instance.FetchItemByID(1);
                    break;

                default:
                    tmp = new Item();
                    break;
            }

            InsertItem(tmp, i);
        }
    }
    
    /// <summary>
    /// Draw Item in the box.
    /// </summary>
    /// <param name="type"> type of What are you doing in box </param>
    /// <param name="insertIdx"> Insert location index(just use in insertItem()) </param>
    private void Draw(DRAWTYPE_BOX type, int insertIdx = 0)
    {
        // Change itemlist(Array) to itemlist(List).
        int count = 0;

        // Clear Item Draw before redraw
        DisableAllObjects();

        // Don't need to Draw when itemSIze is 0.
        if (_itemSize == 0)
            return;

        // Select draw location of item depends on item size.
        Transform loc = transform.Find("Loc" + _itemSize).transform;

        // ReDraw Item by ordered position in the box.
        for (int i = 0; i < _itemMaxSize; i++)
        {
            // 아이템 슬롯이 비어있는지 체크.
            if (_itemList[i] == null)
                continue;

            Transform pos = loc.transform.Find("Pos" + (++count)).transform;
            GameObject tmp = null;

            // 아이템 타입에 따라 일치하는 아이템을 활성화 시켜주기.
            switch (_itemList[i].itemType)
            {
                case Item.ItemType.Battery:
                    tmp = pos.GetChild(0).gameObject;
                    tmp.SetActive(true);
                    break;

                case Item.ItemType.Consumable:
                    tmp = pos.GetChild(1).gameObject;
                    tmp.SetActive(true);
                    break;
            }

            if (type == DRAWTYPE_BOX.OPEN)
                tmp.GetComponent<Animator>().Play("Appear");
            else if (type == DRAWTYPE_BOX.INSERT && (i == insertIdx))
                tmp.GetComponent<Animator>().Play("Appear");
            else if (type == DRAWTYPE_BOX.CLOSE)
                tmp.GetComponent<Animator>().Play("Disappear");
            else
                tmp.GetComponent<Animator>().Play("Rotate");
        }
    }

    /// <summary>
    /// All object in treasurebox be inactivated for Closed box
    /// </summary>
    private void DisableAllObjects()
    {
        int idx = 1;

        // 상자에 담길 수 있는 만큼 돌리면 된다.
        for (; idx <= _itemMaxSize; idx++)
        {
            if (idx == 3)
                break;
            Transform loc = transform.Find("Loc" + idx).transform;

            for (int i = 1; i <= idx; i++)
            {
                Transform pos = loc.transform.Find("Pos" + i).transform;

                // item 종류 수 만큼 돌리면 된다.
                for (int j = 0; j < 2; j++)
                {
                    pos.GetChild(j).gameObject.SetActive(false);
                }
            }
        }
    }

    /// <summary>
    /// Update ItemList Size.
    /// </summary>
    private void UpdateItemSize()
    {
        _itemSize = 0;

        for (int i = 0; i < _itemMaxSize; i++)
        {
            if (_itemList[i] != null)
                _itemSize++;
        }
    }

    /// <summary>
    /// Transfer data type Array to List.
    /// </summary>
    /// <returns> ItemList type List<Item> </returns>
    private List<Item> ArrayToList()
    {
        List<Item> list = new List<Item>();

        for(int i = 0; i<3; i++)
        {
            if (_itemList[i] == null)
                continue;

            list.Add(_itemList[i]);
        }

        return list;
    }

    /// <summary>
    /// Process when Box open
    /// </summary>
    private void BoxOpen()
    {
        state = STATE_BOX.OPEN;
        Notice.SetActive(false);

        anim.Play("BoxOpen");

        _inven.LinkTreasureBox(this);

        _sounds["Box_Open"].Play();

        Draw(DRAWTYPE_BOX.OPEN);
    }

    /// <summary>
    /// Process when Box close
    /// </summary>
    private void BoxClose()
    {
        state = STATE_BOX.CLOSE;
        _inven.CutOffTreasureBox();

        _sounds["Box_Close"].Play();

        Draw(DRAWTYPE_BOX.CLOSE);
        
        anim.Play("BoxClose");
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            Debug.Log("박스 진입");
            if (state == STATE_BOX.CLOSE)
            {
                Notice.SetActive(true);
            }
        }
    }
    
    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            if (Input.GetKeyDown(KeyCode.F) && state == STATE_BOX.CLOSE)
            {
                BoxOpen();
            }
            else if(Input.GetKeyDown(KeyCode.F) && state == STATE_BOX.OPEN)
            {
                BoxClose();

                // 접근한 상태에서 상자를 닫았기 때문에, 다시 한 번 메시지를 띄워줘야 한다.
                Notice.SetActive(true);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            Debug.Log("박스 탈출");

            if (state == STATE_BOX.OPEN)
            {
                BoxClose();
            }
            else if (state == STATE_BOX.CLOSE)
            {
                Notice.SetActive(false);
            }
        }
    }
}
