using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enermy : MonoBehaviour
{
    public int hp;
    public float damage;
    public float timeAttack;    // 공격 주기
    public float timeAttacked;  // 피격 주기

    private Material _material;
    private STATE_ENERMY _state;
    private TYPE_ENERMY _type;
    private Animator anim;
    private AudioSource _audio;
    private Dictionary<string, Sound> _soundlist;

    // Navigation 관련
    private Transform _tfPlayer;
    private NavMeshAgent _navAgent;

    // Common
    private float _timer;

    private void Awake()
    {
        init();
    }

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (_state == STATE_ENERMY.ALIVE)
        {
            LookPlayer();

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

    private void init()
    {
        anim = transform.GetComponent<Animator>();
        _state = STATE_ENERMY.SPAWNING;
        _type = TYPE_ENERMY.RABBIT;
        hp = 100;
        damage = 5.0f;

        _audio = gameObject.AddComponent<AudioSource>();
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

    public void OnDead()
    {
        _state = STATE_ENERMY.DEAD;
        // 죽고 나서는 불 필요한 충돌을 없애기 위해 끄기.
        GetComponent<NavMeshAgent>().enabled = false;
        GetComponent<Rigidbody>().useGravity = false;
        transform.GetComponent<SphereCollider>().enabled = false;
        transform.GetComponent<CapsuleCollider>().enabled = false;
        //GetComponent<Renderer>().material = deadMaterial;

        // Colider을 제거하니까 땅에 서있지 못한다
        // GetComponent<SphereCollider>().enabled = false;

        anim.CrossFade("OnDead", 0.1f);
        anim.SetBool("Idle", false);
        anim.SetBool("OnDead", true);

        // Sound
        _soundlist["Death"].Play();

        // 몬스터가 죽고나면 실행되는 GameManager함수.
        GameManager.instance.MonsterDead();
    }

    private void OnAnimAttack()
    {
        anim.CrossFade("OnAttack", 0.1f);
    }

    private void OnAnimAttacked()
    {
        anim.CrossFade("OnAttacked", 0.1f);

        // Sound
        _soundlist["Attacked"].Play();
    }

    // Look Player
    void LookPlayer()
    {
        Quaternion result = transform.rotation;

        Vector3 vectorToTarget = GameObject.Find("Player").transform.position - transform.position;
        //float angle = Mathf.Atan2(vectorToTarget.x, vectorToTarget.z) * Mathf.Rad2Deg;

        //result = Quaternion.AngleAxis(angle, transform.up);

        //vectorToTarget.x = Mathf.Sqrt(vectorToTarget.x * vectorToTarget.x + vectorToTarget.z * vectorToTarget.z);

        //angle = Mathf.Atan2(vectorToTarget.y, vectorToTarget.x) * Mathf.Rad2Deg;

        ////result.set

        //result.SetLookRotation(vectorToTarget);

        transform.rotation = result;
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

                //GetComponent<Cap>().size = new Vector3(0.3f, 0.16f, 0.16f);

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
            Debug.Log("인식했다!!!!");

            if (_state != STATE_ENERMY.ATTACK)
            {
                Debug.Log("공격한다!!!!");
                //other.gameObject.GetComponent<Player>().OnAttacked(damage);

                //OnAttack();
                _state = STATE_ENERMY.ATTACK;
                StartCoroutine(Attack(other.gameObject));
            }
        }
    }

    // 몬스터 공격 관련 코루틴. 애니메이션과의 연동을 위해, 애니메이션 시간을 잘 활용하자.
    // 애니메이션 실행 시간을 조절할 수 있으면 좋을텐데...
    private IEnumerator Attack(GameObject obj)
    {
        Debug.Log("몬스터 공격");

        OnAnimAttack();

        yield return new WaitForSeconds(0.3f);

        obj.GetComponent<PlayerManager>().TakeDamage(damage);

        yield return new WaitForSeconds(1.7f);

        _state = STATE_ENERMY.ALIVE;
    }
}

