using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 *<Summery>
 * Information of player.
 */
public class Player : MonoBehaviour {

    public float speedMove; // 움직임 속도
    public float speedRotate; // 회전 속도
    public float HP; // 체력

    private STATE_PLAYER _state;
    private GameManager _gameManager;
    private Lantern _lantern; // 렌턴
    private Vector3 _movement; // 전체 움직임의 방향
    private float _moveVerti; // 상, 하 움직임
    private float _moveHori; // 좌, 우 움직임

	// Use this for initialization
	void Awake () {
        HP = 100.0f;
        _state = STATE_PLAYER.ALIVE;
        _gameManager = GameManager.instance;
        _lantern = transform.Find("Lantern").gameObject.GetComponent<Lantern>();
        Debug.Log(_lantern);
	}
	
	// Update is called once per frame
	void Update () {
        _moveVerti = Input.GetAxisRaw("Vertical");
        _moveHori = Input.GetAxisRaw("Horizontal");

        PlayerMove();
        PlayerAction();
	}

    public void OnAttacked(float damage)
    {
        PlayerAttacked(damage);
    }

    private void PlayerMove()
    {
        // 움직임이 없으면 함수 종료
        if (_moveVerti == 0 && _moveHori == 0)
        {
            return;
        }

        _movement.Set(_moveHori, 0, _moveVerti);
        _movement = _movement.normalized * speedMove * Time.deltaTime;

        transform.position += _movement;

        // Player가 이동하는 방향으로 회전
        Quaternion newRocation = Quaternion.LookRotation(_movement);
        transform.rotation = Quaternion.Slerp(transform.rotation, newRocation, speedRotate * Time.deltaTime);
    }

    private void PlayerAction()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Debug.Log("space 입력");

            if (_lantern.CanUse())
                _lantern.OnLight(false);
            else
                _lantern.OnLight(true);
        }
    }

    private void PlayerAttacked(float damage)
    {
        if(_state == STATE_PLAYER.ALIVE)
        {
            HP -= damage;
            Debug.Log(HP);

            if (HP <= 0)
                _state = STATE_PLAYER.DEAD;
        }
    }
}
