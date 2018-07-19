using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lantern : MonoBehaviour {

    public Transform tr; // Player model에서 전등이 위치할 bone 위치 정보를 저장하는 곳.
    public STATE_LIGHT State { get { return _state; } } // return state of lantern
    public STATE_BATTERY StateBattery { get { return _stateBattery; } } // return state of battery

    private STATE_LIGHT _state;
    private STATE_BATTERY _stateBattery;
    private GameObject _light;
    private AudioSource _audio;
    private Dictionary<string, Sound> _soundlist;

    // battery
    private NewInventory _inventory;
    private float _batteryConsume;
    private float _batteryAmount;
    private float _batteryMax;

    // UI
    private UIBattery _uiBattery;

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
        // 랜턴 위치 관련
        UpdateLocation();
        UpdateBattery();

        if (_state == STATE_LIGHT.ON)
        {
            // check battery is empty
            if (CanUse() == false)
            {
                _state = STATE_LIGHT.OFF;
                OnLight(false);
            }

            UseBattery();
        }
    }

    // Set latern state.
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

    // check battery can use or not
    public bool CanUse()
    {
        return (_stateBattery == STATE_BATTERY.USE) ? true : false;
    }

    // Set Battery amount
    // Can use this function when a new battery is used in battery inventory.
    public void SetBatteryAmount(float value)
    {
        _batteryAmount = value;
    }

    private void Init()
    {
        // Lantern
        _light = transform.Find("Light").gameObject;
        _light.SetActive(false);
        _state = STATE_LIGHT.OFF;

        // Battery
        _batteryConsume = 0.001f;
        _batteryAmount = 0;
        _batteryMax = 0;
        _stateBattery = STATE_BATTERY.USE;
    }

    // Execute when Light is on.
    private void UseBattery()
    {
        // _batteryAmount -= _batteryConsume;
        _inventory.UseBattery();
        //_batteryAmount = _inventory.CurrentBatterySize();

        if(_batteryAmount <= 0.0f)
        {
            _batteryAmount = 0.0f;
            _stateBattery = STATE_BATTERY.EMPTY;
        }

        // set UI
        _uiBattery.SetBatteryValue(_batteryAmount / _batteryMax);
    }
    
    // Update Lantern location by player hand location
    private void UpdateLocation()
    {
        Vector3 temp;
        Quaternion temp2;

        temp = tr.position;
        temp2 = tr.rotation;
        transform.SetPositionAndRotation(temp, temp2);
    }

    private void UpdateBattery()
    {
        _batteryAmount = _inventory.CurrentBatterySize();

        // newinventory에서 database가 awake에서 초기화된다면 ㅇㅇ.
        // 나중에 start에서 한 번만 실행시킬 수 있도록 바꾸기.
        _batteryMax = _inventory.TotalBatterySize();

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
