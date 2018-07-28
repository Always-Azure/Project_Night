using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI; // use hp ui

public class PlayerManager : MonoBehaviour {

    public STATE_PLAYER State { get { return _state; } }

    private GameManager _gameManager;   // GameManager
    private NewInventory _newInven; // Inventory
    private STATE_PLAYER _state;    // Player Condition
    private PlayerState _playerState;   // Player State
    private float _playerMaxHp = 100.0f;
    private float _playerCurrentHp;
    private float _degree = 1f;
    
    private AudioSource _audioMovement; // Movement Sound
    private AudioSource _audioEffectInner; // Effect Sound by myself
    private AudioSource _audioEffectOutter; // Effect Sound by others
    private Dictionary<string, Sound> _soundlist; // Sound List

    private Lantern _lantern;   // Lantern
    private UIHp _uiHp;     // UI(HP) Controller

    private float _degreeTime = 0.5f;

    private float _timer;
    private float _PosionTimer;
    public bool _PosionOn = false;

    private void Awake()
    {
        InitState();
    }

    // Use this for initialization
    void Start()
    {
        Init();
    }

    // Update is called once per frame
    void Update()
    {
        if (!_newInven.GetSlotOnCheck())
            PlayerAction();

        if (_PosionTimer < 1.2f && _PosionOn == false)
            _PosionTimer += Time.deltaTime;
        else
            _PosionOn = true;
    }

    // 플레이어 상태 초기화
    public void InitState()
    {
        _playerCurrentHp = _playerMaxHp;
        _degree = 1f;
    }
    // 플레이어에서 필요한 Component들 초기화
    private void Init()
    {
        _lantern = transform.Find("Lantern").GetComponent<Lantern>();
        _uiHp = GameObject.Find("UIManager").GetComponent<UIHp>();
        _gameManager = GameManager.instance;
        _playerState = new PlayerState();
        _newInven = GameObject.Find("InventorySystem").GetComponent<NewInventory>();
        
        // Audio
        _audioMovement = gameObject.AddComponent<AudioSource>();
        _audioMovement.loop = true;
        _audioEffectInner = gameObject.AddComponent<AudioSource>();
        _audioEffectOutter = gameObject.AddComponent<AudioSource>();
        _soundlist = AudioManager.instance.SoundInfo.GetSubDir("Player").GetSoundList();
    }

    public void TakeDamage(float dmg)
    {
        if(_state == STATE_PLAYER.ALIVE)
        {
            if (_gameManager == null)
                _gameManager = GameManager.instance;

            _playerCurrentHp -= dmg;

            _uiHp.SetHpValue(_playerCurrentHp);

            // Sound
            // 환경으로 인해 피해 입는 소리랑, 몬스터한테 맞는 소리랑 같기에 게임 플레이하면서 소리가 너무 시끄럽다. 그래서 주석처리해둠.
            //_soundlist["Attacked"].Play();
            
            if (_playerCurrentHp <= 0.0f)
            {
                PlayerDeath();
            }
        }
    }

    public void TakePosion(float posion)
    {
        if (_state == STATE_PLAYER.ALIVE && _PosionTimer > 1.2f)
        {
            if (_gameManager == null)
                _gameManager = GameManager.instance;

            _playerCurrentHp += posion;

            if (_playerCurrentHp > 100)
                _playerCurrentHp = 100;
            
            _uiHp.SetHpValue(_playerCurrentHp);
            _PosionTimer = 0f;
            _PosionOn = false;
        }
    }

    // Controll The Player Action.
    private void PlayerAction()
    {
        // Click Left Mouse Button
        if (Input.GetMouseButtonDown(0))
        {
            Debug.Log("왼쪽 마우스 on");

            _lantern.OnLight(true);

        }
        // Up Left Mouse Button
        else if (Input.GetMouseButtonUp(0))
        {
            Debug.Log("왼쪽 마우스 off");

            _lantern.OnLight(false);
        }

        // Player Jump
        if (Input.GetButtonDown("Jump"))
        {
            _playerState.Jump();
            _soundlist["Jump"].Play(_audioEffectInner);

            _audioMovement.Stop();
        }

        // [ Player Movement ]
        if (Input.GetButton("Vertical") || Input.GetButton("Horizontal"))
        {
            // 조건문에서 GetButtonDown을 써주고 싶지만, 앞으로만 가는 상황에서 jump했다가 땅에 착지하면 해당 조건문은 실행이 되지 않기에, GetButton으로 했다.
            if (_playerState.IsStanding == true)
            {
                // IsStanding == true로 확인할 수 있지만, 움직이려고 하는 것이기 때문에, Walking == false로 확인해줌.
                if (_playerState.IsWalking == false && _playerState.IsJump == false)
                {
                    _playerState.Walking();
                    _soundlist["Walk"].Play(_audioMovement);
                }
            }
        }
        // 멈췄을 때 코드.
        else if(Input.GetButton("Vertical") == false && Input.GetButton("Horizontal") == false)
        {
            if(_playerState.IsStanding == false)
            {
                _playerState.Standing();
                _audioMovement.Stop();
            }
        }
    }

    // Execute when player die.
    void PlayerDeath()
    {
        //isDead = true;
        _state = STATE_PLAYER.DEAD;
        Debug.Log("PlayerDead");

        SceneManager.LoadScene("GameOverScene");

    }

    // 따로 만들어주는 이유는, StreetLight가 시간이 다되서 꺼지게 되면, 그 때, player의 TriggerExit가 실행되지 않기 때문!
    public void ExitToStreetlight()
    {
        Debug.Log("위험해졌습니다!");
        _state = STATE_PLAYER.ALIVE;
    }

    //Trigger & Collision with Player Object
    void OnControllerColliderHit(ControllerColliderHit hit) // Player Controller가 있을 떄, 물리 충돌 감지
    {
        if (hit.gameObject.tag == "Wall")
        {
            Debug.Log("Wall Collision");
        }

        if(hit.gameObject.tag == "Ground")
        {
            if(_playerState.OnGround == false)
            {
                if(_playerState.IsJump == true)
                {
                    _playerState.LandGround();
                    _soundlist["Land"].Play(_audioEffectInner);
                }
            }
        }
    }

    void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.tag == "Posion")
        {
            Debug.Log("Near Posion");
            if (Input.GetKeyDown(KeyCode.F))
            {
            }
        }

        if (col.gameObject.tag == "Btr")
        {
            Debug.Log("Near Btr");
        }

        if(col.gameObject.tag == "StreetLight")
        {
            Debug.Log("안전해졌습니다!");
            _state = STATE_PLAYER.SAFETY;
        }
    }

    void OnTriggerStay(Collider col)
    {
        if (col.gameObject.tag == "Posion")
        {
            if (Input.GetKeyDown(KeyCode.F))
            {
                Debug.Log("Get Posion");
            }
        }

        if (col.gameObject.tag == "Btr")
        {
            //Debug.Log("Near Btr");
        }

        if (col.gameObject.tag == "StreetLight")
        {
            if(_state != STATE_PLAYER.SAFETY)
                _state = STATE_PLAYER.SAFETY;
        }
    }

    private void OnTriggerExit(Collider col)
    {
        if(col.gameObject.tag == "StreetLight")
        {
            //_state = STATE_PLAYER.ALIVE;
            ExitToStreetlight();
        }
    }
}
