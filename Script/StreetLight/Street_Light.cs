using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Handle general StreetLight process
/// </summary>
/// <author> YeHun </author>
public class Street_Light : MonoBehaviour
{ 
    // Property
    public STATE_STREETLIGHT State { get { return _state; } }

    private STATE_STREETLIGHT _state;   // 가로등 상태
    private PlayerManager _player;  // 플레이어
    private GameObject _light;  // 가로등 빛
    private GameObject _light_lights;   // 깜빡임을 위한 가로등 빛 메쉬 정보
    private AudioSource _audio; // 사운드
    private Dictionary<string, Sound> _soundlist;   // 사운드 리스트

    private bool _isOn; // 가로등이 켜져있는가 확인하는 것.
    private bool _isValid;  // 가로등에 접근했는지, 가로등을 사용할 수 있는지 확인하는 것.

    private float _battery; // 배터리 양.

    private float _time;

    private void Awake()
    {
        Init();
    }

    // Use this for initialization
    void Start()
    {
        _audio = gameObject.AddComponent<AudioSource>();
        _soundlist = AudioManager.instance.SoundInfo.GetSubDir("Objects").GetSubDir("StreetLight").GetSoundList(_audio);
    }

    // Update is called once per frame
    void Update()
    {

        if (_state == STATE_STREETLIGHT.BT0)
            return;

        if (_isValid == true)
        {
            if (Input.GetKeyDown(KeyCode.F))
            {
                if (_isOn == true)
                {
                    _light.SetActive(false);
                    _isOn = false;

                    _soundlist["StreetLight_On"].Play();
                }
                else
                {
                    _light.SetActive(true);
                    _isOn = true;

                    _soundlist["StreetLight_On"].Play();
                }
            }
        }

        // If StreetLight is turn on, consume battery.
        if (_isOn == true)
        {
            StateControl();

            _time += Time.deltaTime;

            if (_time > 1.0f)
            {
                Debug.Log(_battery);
                Debug.Log(_state);
                _battery -= 10.0f;
                _time = 0;
            }
        }
    }

    /// <summary>
    /// Initialize StreetLight
    /// </summary>
    private void Init()
    {
        _state = STATE_STREETLIGHT.BT100;
        _light = transform.Find("Bulb").gameObject.transform.Find("Light").gameObject;
        _light_lights = transform.Find("Bulb").gameObject.transform.Find("Light").gameObject.transform.Find("Lights").gameObject;
        _light.SetActive(false);
        _isOn = false;
        _isValid = false;
        _battery = 100f;
        _time = 0;

        _player = null;
    }

    /// <summary>
    /// Control StreetLight by Battery Amount
    /// </summary>
    private void StateControl()
    {
        float _flickertime = 0; // 깜빡이는 시간을 측정하기위한 변수.

        // Battery 상태에 따른 설정.
        // 100
        if (_state == STATE_STREETLIGHT.BT100)
        {
            if (_battery <= 50f)
                _state = STATE_STREETLIGHT.BT50;
        }
        // 50
        else if (_state == STATE_STREETLIGHT.BT50)
        {
            // 2초에 한 번씩 깜빡
            if (_flickertime > 2.0f)
            {
                StartCoroutine(FlickerLight());
                _flickertime = 0;
            }
            // 상태조정
            if (_battery <= 25f)
                _state = STATE_STREETLIGHT.BT25;
        }
        // 25
        else if (_state == STATE_STREETLIGHT.BT25)
        {
            // 1초에 한 번씩 깜빡
            if (_flickertime > 1.0f)
            {
                StartCoroutine(FlickerLight());
                _flickertime = 0;
            }
            // 상태조정
            if (_battery <= 10f)
                _state = STATE_STREETLIGHT.BT10;
        }
        // 10
        else if (_state == STATE_STREETLIGHT.BT10)
        {
            // 0.5초에 한 번씩 깜빡
            if (_flickertime > 0.5f)
            {
                StartCoroutine(FlickerLight());
                _flickertime = 0;
            }
            // 상태조정
            if (_battery <= 0f)
            {
                _state = STATE_STREETLIGHT.BT0;
                _light.SetActive(false);
                _isValid = false;
                _isOn = false;

                // 플레이어 상태 변경(Safe -> Normal)
                _player.ExitToStreetlight();
            }
        }

        if (_state != STATE_STREETLIGHT.BT100)
            _flickertime += Time.deltaTime;
    }

    /// <summary>
    /// Coroutine for Flicker situation
    /// </summary>
    /// <returns></returns>
    private IEnumerator FlickerLight()
    {
        _light_lights.SetActive(false);

        yield return new WaitForSeconds(0.1f);

        _light_lights.SetActive(true);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            _isValid = true;
            _player = other.gameObject.GetComponent<PlayerManager>();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            _isValid = false;
            _player = null;
        }
    }
}
