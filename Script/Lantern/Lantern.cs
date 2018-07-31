using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Handle general lantern process
/// </summary>
/// <author> YeHun </author>
public class Lantern : MonoBehaviour {

    // Property
    public STATE_LIGHT State { get { return _state; } } // return state of lantern
    public STATE_BATTERY StateBattery { get { return _stateBattery; } } // return state of battery

    public Transform tr; // Player model에서 전등이 위치할 bone 위치 정보를 저장하는 곳.

    private STATE_LIGHT _state; // 랜턴 상태
    private GameObject _light;  // 랜턴 빛
    private AudioSource _audio; // 사운드
    private Dictionary<string, Sound> _soundlist;   // 사운드 리스트

    // battery
    private STATE_BATTERY _stateBattery;    //배터리 상태
    private NewInventory _inventory;    // 인벤토리 참조
    private float _batteryAmount;   // 현재 배터리 량
    private float _batteryMax;  // 최대 배터리 량

    // UI
    private UIBattery _uiBattery;   // 배터리 UI 참조

    // Init inner values
    private void Awake()
    {
        Init();   
    }

    // Use this for initialization
    void Start () {
        _inventory = GameObject.Find("InventorySystem").GetComponent<NewInventory>();
        _uiBattery = GameObject.Find("UIManager").GetComponent<UIBattery>();

        _audio = gameObject.AddComponent<AudioSource>();
        _soundlist = AudioManager.instance.SoundInfo.GetSubDir("Objects").GetSubDir("Lantern").GetSoundList(_audio);
    }

    // Update is called once per frame
    void Update () {
        UpdateLocation();   // 랜턴 위치 Update
        UpdateBattery();    // 배터리량 Update from Inventory(Equip)

        // Check the lantern is using now
        if (_state == STATE_LIGHT.ON)
        {
            // check battery is empty
            if (CanUse() == false)
            {
                _state = STATE_LIGHT.OFF;
                OnLight(false);
            }
            else
            {
                UseBattery();
            }
        }
    }

    /// <summary>
    /// Set latern state
    /// </summary>
    /// <param name="value"> true : On, false : Off </param>
    public void OnLight(bool value)
    {
        // 사운드를 여기 놔둔 이유는, 꺼지든 켜지든 스위치를 누르기 때문. 배터리가 없어도 스위치를 누르기 때문!
        _soundlist["Switch_On"].Play();

        if (value)
        {
            // check battery is empty
            if (CanUse() == false)
                return;

            _light.SetActive(true);
            _state = STATE_LIGHT.ON;
        }
        else
        {
            _light.SetActive(false);
            _state = STATE_LIGHT.OFF;
        }
    }

    /// <summary>
    /// check battery can use or not
    /// </summary>
    public bool CanUse()
    {
        return (_stateBattery == STATE_BATTERY.USE) ? true : false;
    }

    /// <summary>
    /// Set Battery amount
    /// Can use this function when a new battery is used in battery inventory.
    /// </summary>
    /// <param name="value"> Battery Amount </param>
    public void SetBatteryAmount(float value)
    {
        _batteryAmount = value;
    }

    /// <summary>
    /// Initialize Lantern object.
    /// </summary>
    private void Init()
    {
        // Lantern
        _light = transform.Find("Light").gameObject;
        _light.SetActive(false);
        _state = STATE_LIGHT.OFF;

        // Battery
        _batteryAmount = 0;
        _batteryMax = 0;
        _stateBattery = STATE_BATTERY.USE;

        Debug.Log("Lantern - Init");
    }

    /// <summary>
    /// Execute when Light is on.
    /// </summary>
    private void UseBattery()
    {
        _inventory.UseBattery();

        if(_batteryAmount <= 0.0f)
        {
            _batteryAmount = 0.0f;
            _stateBattery = STATE_BATTERY.EMPTY;
        }

        // set UI
        _uiBattery.SetBatteryValue(_batteryAmount / _batteryMax);
    }

    /// <summary>
    /// Update Lantern location by player hand location
    /// </summary>    
    private void UpdateLocation()
    {
        Vector3 temp;
        Quaternion temp2;

        temp = tr.position;
        temp2 = tr.rotation;
        transform.SetPositionAndRotation(temp, temp2);
    }

    /// <summary>
    /// Update Battery Amount from Equped battery.
    /// </summary>    
    private void UpdateBattery()
    {
        // Initialize max battery amount
        if (_batteryMax == 0f)
            _batteryMax = _inventory.GetTotalBatterySize();

        _batteryAmount = _inventory.GetCurrentBatterySize();
        
        // 배터리 양이 0에서 바뀌면 사용가능하게 만들어야한다.
        if (_stateBattery == STATE_BATTERY.EMPTY)
        {
            if (_batteryAmount > 0f)
            {
                _stateBattery = STATE_BATTERY.USE;
            }
        }

        _uiBattery.SetBatteryValue(_batteryAmount / _batteryMax);
    }
}
