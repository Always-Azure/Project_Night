using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI; // use hp ui

/// <summary>
/// Handle general player process
/// </summary>
/// <author> SangJun, YeHun </author>
public class PlayerManager : MonoBehaviour {

    // Property
    public STATE_PLAYER State { get { return _state; } }

    private GameManager _gameManager;   // GameManager
    private NewInventory _newInven; // Inventory
    private STATE_PLAYER _state;    // Player Condition
    private PlayerState _playerState;   // Player State
    private float _playerMaxHp = 100.0f;    // Max Hp
    private float _playerCurrentHp; // Current Hp
    private float _degree = 1f; // Decrease amount of HP
    
    private AudioSource _audioMovement; // Movement Sound
    private AudioSource _audioEffectInner; // Effect Sound by myself
    private AudioSource _audioEffectOutter; // Effect Sound by others
    private Dictionary<string, Sound> _soundlist; // Sound List

    private Lantern _lantern;   // Lantern
    private UIHp _uiHp;     // UI(HP) Controller

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
        // check inventory is opened now
        if (!_newInven.GetSlotOnCheck())
            PlayerAction();

        // Delay check consume potion.
        if (_PosionTimer < 1.2f && _PosionOn == false)
            _PosionTimer += Time.deltaTime;
        else
            _PosionOn = true;
    }

    /// <summary>
    /// Initialize Player state
    /// This method will execute when a new stage start
    /// </summary>
    public void InitState()
    {
        _playerCurrentHp = _playerMaxHp;
        _degree = 1f;
    }

    /// <summary>
    /// Initialize player's components
    /// </summary>
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

        Debug.Log("PlayerManager - Init");
    }

    /// <summary>
    /// Handle the Player attacked situations
    /// </summary>
    /// <param name="dmg"> Damage </param>
    public void TakeDamage(float dmg, TYPE_ATTACK type)
    {
        if(_state == STATE_PLAYER.ALIVE)
        {
            if (_gameManager == null)
                _gameManager = GameManager.instance;

            _playerCurrentHp -= dmg;

            // Set UI
            _uiHp.SetHpValue(_playerCurrentHp);

            // Sound
            switch (type)
            {
                case TYPE_ATTACK.ENERMY:
                    _soundlist["Attacked"].Play(_audioEffectOutter);
                    break;

                case TYPE_ATTACK.ENVIRONMENT:
                    break;
            }
            
            // Check is player death or not
            if (_playerCurrentHp <= 0.0f)
            {
                PlayerDeath();
            }
        }
    }

    /// <summary>
    /// Handle the player consume posions
    /// </summary>
    /// <param name="posion"> Amount Increase HP </param>
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

    /// <summary>
    /// Controll The Player Action.
    /// </summary>
    private void PlayerAction()
    {
        // Click Left Mouse Button
        if (Input.GetMouseButtonDown(0))
        {
            _lantern.OnLight(true);
        }
        // Up Left Mouse Button
        else if (Input.GetMouseButtonUp(0))
        {
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
            if (_playerState.IsStanding == true)
            {
                if (_playerState.IsWalking == false && _playerState.IsJump == false)
                {
                    _playerState.Walking();
                    _soundlist["Walk"].Play(_audioMovement);
                }
            }
        }
        // Player Stop or Stand
        else if(Input.GetButton("Vertical") == false && Input.GetButton("Horizontal") == false)
        {
            if(_playerState.IsStanding == false)
            {
                _playerState.Standing();
                _audioMovement.Stop();
            }
        }
    }

    /// <summary>
    /// Execute when player die.
    /// </summary>
    private void PlayerDeath()
    {
        _state = STATE_PLAYER.DEAD;
        
        // Scene change -> GameOverScene
        SceneManager.LoadScene("GameOverScene");
    }

    /// <summary>
    /// Handle when player exit to streetlight
    /// Why I make this method, because when streetlight turn of by zero battery, player's TriggerExit will not execute.
    /// So I make this method for handling that player exit to streetlight
    /// </summary>
    public void ExitToStreetlight()
    {
        _state = STATE_PLAYER.ALIVE;
    }

    // Player Controller가 있을 떄, 물리 충돌 감지
    void OnControllerColliderHit(ControllerColliderHit hit)
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
        if(col.gameObject.tag == "StreetLight")
        {
            _state = STATE_PLAYER.SAFETY;
        }
    }

    void OnTriggerStay(Collider col)
    {
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
            ExitToStreetlight();
        }
    }
}
