using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lantern : MonoBehaviour {

    public Transform tr;
    
    private STATE_LIGHT _state { get; set; }
    private GameManager _gameManager;
    private GameObject _light;

    Vector3 temp;
    Quaternion temp2;

    private void Awake()
    {
        _light = transform.Find("Light").gameObject;
        _light.SetActive(false);
        _state = STATE_LIGHT.OFF;
    }

    // Use this for initialization
    void Start () {
        Debug.Log("asdf");
        _gameManager = GameManager.instance;

        Debug.Log(_gameManager);
    }

    // Update is called once per frame
    void Update () {
        // 랜턴 위치 관련
        temp = tr.position;
        temp2 = tr.rotation;
        transform.SetPositionAndRotation(temp,temp2);

        if(_state == STATE_LIGHT.ON)
        {
            if(_gameManager.UseLantern() == false)
            {
                _state = STATE_LIGHT.OFF;
                OnLight(false);
            }
        }
	}

    public void OnLight(bool value)
    {
        if (value)
        {
            _light.SetActive(true);
            _state = STATE_LIGHT.ON;
        }
        else
        {
            _light.SetActive(false);
            _state = STATE_LIGHT.OFF;
        }
    }

    public bool GetState()
    {
        return (_state == STATE_LIGHT.ON) ? true : false;
    }
    
}
