using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enermy : MonoBehaviour {

    public Material deadMaterial;
    public int hp;
    public float damage;
    public float time;

    private Material _material;
    private STATE_ENERMY _state;

    // Use this for initialization
    void Start()
    {
        _state = STATE_ENERMY.ALIVE;
        hp = 100;
        damage = 0.1f;
        _material = GetComponent<Renderer>().material;
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void OnDead()
    {
        _state = STATE_ENERMY.DEAD;
        GetComponent<Renderer>().material = deadMaterial;

        // Colider을 제거하니까 땅에 서있지 못한다
        // GetComponent<SphereCollider>().enabled = false;
    }

    private void OnAttack()
    {

    }

    private void OnTriggerStay(Collider other)
    {
        if (_state == STATE_ENERMY.DEAD)
            return;

        if(other.gameObject.tag == "Light")
        {
            // 나중에 Light와 Attack에 대한 time을 나누자.
            time += Time.deltaTime;

            if (time > 0.5)
            {
                hp -= 10;
                Debug.Log(hp);

                if(hp == 0)
                {
                    OnDead();
                }

                time = 0;
            }
        }
        
        // 몬스터 공격에 대한 것은 어떻게 할지 정하지 않았지만, 일단 몬스터의 영역에 일정시간이 있으면 공격하는 걸로 처리해놨다.
        if(other.gameObject.tag == "Player")
        {
            // 나중에 Light와 Attack에 대한 time을 나누자.
            time += Time.deltaTime;

            if (time > 1)
            {
                other.gameObject.GetComponent<Player>().OnAttaced(damage);

                time = 0;
            }
        }
    }
}

