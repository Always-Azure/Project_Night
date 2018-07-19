using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class GameManager : MonoBehaviour {

    public static GameManager instance;

    public float coldDamage;
    public int stageNum { get; set; }

    //private Player _player;
    private PlayerManager _player;
    private MonsterRespawn _monsterRespawn;

    // Resource
    private ResourcesManager _resourcesManager;

    // Common
    private float _timer;
    private int _monsterCount;

	// Use this for initialization
	void Awake () {
        instance = this;

        coldDamage = 1.0f;

        //_player = GameObject.Find("Player").GetComponent<Player>();
        _player = GameObject.Find("Player").GetComponent<PlayerManager>();
        _monsterRespawn = GetComponent<MonsterRespawn>();
        _resourcesManager = GameObject.Find("ResourcesManager").GetComponent<ResourcesManager>();

        _timer = 0;
    }
	
	// Update is called once per frame
	void Update () {
		if(_timer > 1.0f)
        {
            _player.TakeDamage(coldDamage);

            _timer = 0f;
        }
        _timer += Time.deltaTime;
	}

    // Use move player location when new stage start
    public Transform GetPlayerLocation()
    {
        return _player.transform;
    }

    // Get Player State
    public STATE_PLAYER GetPlayerState()
    {
        return _player.State;
    }

    // Initiallize Stage
    public void InitStage()
    {
        _player.InitState();
        _monsterCount = 0;
    }

    // Move Player Location
    public void MovePlayerLocation(Vector3 pos)
    {
        _player.transform.position = pos;
    }

    // Create Monster
    public void CreateMonster(string name, int monsterLimit)
    {
        if (monsterLimit > _monsterCount)
        {
            _monsterRespawn.CreateMonster(_player.transform, name);

            _monsterCount++;
        }
    }

    // Create Objects
    public void CreateObject(Vector3 pos, string name)
    {
        _resourcesManager.CreateObject(pos, name);
    }

    // Handle the Hp, when the player is attacked
    public void PlayerAttacked(float dmg)
    {
        if(_player.State == STATE_PLAYER.ALIVE)
            _player.TakeDamage(dmg);
    }

    // when Player die, change Scene.
    private void PlayerDeath()
    {
        SceneManager.LoadScene("GameOverScene");
    }

    // Execute when monster dead.
    public void MonsterDead()
    {
        _monsterCount--;
    }

    public void SetPlayerState(STATE_PLAYER state)
    {

    }
}
