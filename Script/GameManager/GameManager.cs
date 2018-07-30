using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

/// <summary>
/// Manage general game data and system.
/// You can use moster spawner and resource data.
/// </summary>
/// <author> YeHun </author>
public class GameManager : MonoBehaviour {

    // Property
    public int stageNum { get; set; }

    public static GameManager instance; // 싱글톤을 위한 인스턴스

    public float coldDamage;    // 추위 피해량

    private PlayerManager _player;  // Player 참조
    private MonsterRespawn _monsterRespawn; // Monster 생성 관리
    private ResourcesManager _resourcesManager; // Resource 관리

    private float _timer;   // 추위로 인한 피해를 입는 효과를 위한 시간 저장 변수
    private int _monsterCount;  // 생성된 몬스터 수 (20 이하)

	// Use this for initialization
	void Awake () {
        init();
    }
	
	// Update is called once per frame
	void Update () {
		if(_timer > 1.0f)
        {
            _player.TakeDamage(coldDamage, TYPE_ATTACK.ENVIRONMENT);

            _timer = 0f;
        }
        _timer += Time.deltaTime;
	}

    /// <summary>
    /// 생성자(싱글톤 패턴 사용)
    /// </summary>
    private GameManager()
    {
        if (instance == null)
            instance = this;
    }

    /// <summary>
    /// Initialize GameManager Object
    /// </summary>
    private void init()
    {
        instance = this;

        coldDamage = 1.0f;

        _player = GameObject.Find("Player").GetComponent<PlayerManager>();
        _monsterRespawn = GetComponent<MonsterRespawn>();
        _resourcesManager = GameObject.Find("ResourcesManager").GetComponent<ResourcesManager>();

        _timer = 0;

        Debug.Log("GameManager - Init");
    }

    /// <summary>
    /// When start new Stage, Initiallize Data.
    /// </summary>
    public void InitStage()
    {
        _player.InitState();
        _monsterCount = 0;

        Debug.Log("GameManager - Init stage");
    }

    /// <summary>
    ///  Use move player location when new stage start
    /// </summary>
    /// <returns> Player's Transform. </returns>
    public Transform GetPlayerLocation()
    {
        return _player.transform;
    }

    /// <summary>
    ///  Get Player's state
    /// </summary>
    /// <returns> Player's State. </returns>
    public STATE_PLAYER GetPlayerState()
    {
        return _player.State;
    }

    /// <summary>
    ///  Set Player's Location
    /// </summary>
    public void SetPlayerLocation(Vector3 pos)
    {
        _player.transform.position = pos;
    }

    /// <summary>
    ///  Create Monster
    /// </summary>
    /// <param name="name"> Monster name </param>
    /// <param name="monsterLimit"> Max monster count </param>
    public void CreateMonster(string name, int monsterLimit)
    {
        if (monsterLimit > _monsterCount)
        {
            _monsterRespawn.CreateMonster(_player.transform, name);

            _monsterCount++;
        }
    }
    
    /// <summary>
    /// Create Objects
    /// </summary>
    /// <param name="pos"> Create Location </param>
    /// <param name="name"> Object name </param>
    public void CreateObject(Vector3 pos, string name)
    {
        _resourcesManager.CreateObject(pos, name);
    }

    /// <summary>
    /// Execute when monster dead.
    /// </summary>
    public void MonsterDead()
    {
        _monsterCount--;
    }
}
