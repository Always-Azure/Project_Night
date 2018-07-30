using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

/// <summary>
/// Basic Enermy Data
/// </summary>
/// <author> YeHun </author>
public class Enermy : MonoBehaviour
{
    public int hp;  // 체력
    public float damage;    // 공격력
    public float timeAttack;    // 공격 주기
    public float timeAttacked;  // 피격 주기

    private STATE_ENERMY _state;    // 상태
    private TYPE_ENERMY _type;  // 타입
    private Material _material;
    private Animator _anim;  // 애니메이션
    private AudioSource _audio; // 사운드
    private Dictionary<string, Sound> _soundlist; // 사운드 리스트

    // Navigation 관련
    private Transform _tfPlayer;    // Player Location
    private NavMeshAgent _navAgent;

    // Common
    private float _timer;

    private void Awake()
    {
        init();
    }

    // Update is called once per frame
    void Update()
    {
        if (_state == STATE_ENERMY.ALIVE)
        {
            // Track Player per second
            if (_timer > 1.0f)
            {
                _navAgent = gameObject.GetComponent<NavMeshAgent>();
                _tfPlayer = GameManager.instance.GetPlayerLocation();

                _navAgent.destination = _tfPlayer.position;

                _timer = 0;
            }

            _timer += Time.deltaTime;
        }
    }

    /// <summary>
    /// Initialize Enermy data.
    /// </summary>
    private void init()
    {
        hp = 100;
        damage = 5.0f;

        _state = STATE_ENERMY.SPAWNING;
        _type = TYPE_ENERMY.RABBIT;
        _anim = transform.GetComponent<Animator>();
        _audio = gameObject.AddComponent<AudioSource>();
        
        // Get Soundlist by enermy type
        switch(_type)
        {
            case TYPE_ENERMY.BAT:
                _soundlist = AudioManager.instance.SoundInfo.GetSubDir("Monster").GetSubDir("Bat").GetSoundList(_audio);
                break;

            case TYPE_ENERMY.RABBIT:
                _soundlist = AudioManager.instance.SoundInfo.GetSubDir("Monster").GetSubDir("Rabbit").GetSoundList(_audio);
                break;
        }   
    }

    /// <summary>
    /// Process when enermy dead
    /// </summary>
    public void OnDead()
    {
        _state = STATE_ENERMY.DEAD;
        GetComponent<NavMeshAgent>().enabled = false;
        Destroy(GetComponent<Rigidbody>());
        transform.GetComponent<SphereCollider>().enabled = false;
        transform.GetComponent<CapsuleCollider>().enabled = false;

        // Animation
        _anim.CrossFade("OnDead", 0.1f);
        _anim.SetBool("Idle", false);
        _anim.SetBool("OnDead", true);

        // Sound
        _soundlist["Death"].Play();

        // 몬스터가 죽고나면 실행되는 GameManager함수.
        GameManager.instance.MonsterDead();
    }

    /// <summary>
    /// Attack Animation
    /// </summary>
    private void OnAnimAttack()
    {
        _anim.CrossFade("OnAttack", 0.1f);
    }

    /// <summary>
    /// Attacked Animation
    /// </summary>
    private void OnAnimAttacked()
    {
        _anim.CrossFade("OnAttacked", 0.1f);

        // Sound
        _soundlist["Attacked"].Play();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (_state == STATE_ENERMY.SPAWNING)
        {
            if (collision.transform.tag == "Ground")
            {
                transform.position += new Vector3(0, 2, 0);

                transform.Find("MESH").gameObject.SetActive(true);
                GetComponent<Animator>().enabled = true;
                GetComponent<SphereCollider>().enabled = true;
                GetComponent<Enermy>().enabled = true;
                GetComponent<NavMeshAgent>().enabled = true;

                _state = STATE_ENERMY.ALIVE;
            }
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (_state == STATE_ENERMY.DEAD)
            return;

        if (other.gameObject.tag == "Light")
        {
            timeAttacked += Time.deltaTime;

            if (timeAttacked > 0.5)
            {
                hp -= 10;
                Debug.Log(hp);
                OnAnimAttacked();

                if (hp == 0)
                {
                    OnDead();
                }

                timeAttacked = 0;
            }
        }

        // when Enermy take the street light, dead.
        if(other.gameObject.tag == "StreetLight")
        {
            OnDead();
        }

        // when player get in the attack range, 
        if (other.gameObject.tag == "Player")
        {
            // check attack now
            if (_state != STATE_ENERMY.ATTACK)
            {
                _state = STATE_ENERMY.ATTACK;
                StartCoroutine(Attack(other.gameObject));
            }
        }
    }

    /// <summary>
    /// This Coroutine is related with Enermy Attack.
    /// You must controll Animation time and Attack method execute time.
    /// </summary>
    /// <param name="obj"> Object that enermy should attack </param>
    private IEnumerator Attack(GameObject obj)
    {
        OnAnimAttack();

        yield return new WaitForSeconds(0.3f);

        obj.GetComponent<PlayerManager>().TakeDamage(damage, TYPE_ATTACK.ENERMY);

        yield return new WaitForSeconds(1.7f);

        _state = STATE_ENERMY.ALIVE;
    }
}

