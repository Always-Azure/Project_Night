using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BatteryController : MonoBehaviour
{ 
    public float batteryConsume;

    private GameManager _gameManager;
    private UIManager _uIManager;
    private float _batteryAmount;
    private float _batteryLimit;
    private STATE_BATTERY _state;

    // Use this for initialization
    void Awake()
    {
        _gameManager = GameManager.instance;
        
        batteryConsume = 0.001f;
        _batteryAmount = 5.0f;
        _batteryLimit = 5.0f;
        _state = STATE_BATTERY.USE;
    }

    private void Start()
    {
        _uIManager = UIManager.instance;
    }

    // Battery 사용가능한지 확인.
    public bool CanUseBattery()
    {
        return (_state == STATE_BATTERY.USE) ? true : false;
    }

    // ChangeValue()와 따로 해주는 이유는, 함부로 ChangeValue()를 실행시키지 않기 위해서이다.
    public void UseBattery()
    {
        _batteryAmount -= batteryConsume;
        
        if (_batteryAmount <= 0.0f)
        {
            // _batteryAmount가 딱 0.0이 될 수 없기에, 만들어준다.
            _batteryAmount = 0.0f;
            _state = STATE_BATTERY.EMPTY;
        }

        // 0.0 ~ 1.0 사이의 값을 보내줘야 함.
        ChangeValue(_batteryAmount / _batteryLimit);
    }

    private void GetBattery()
    {

    }

    // Change the battery UI
    private void ChangeValue(float value)
    {
        _uIManager.SetBatteryValue(value);
    }
}
