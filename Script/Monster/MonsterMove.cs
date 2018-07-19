using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MonsterMove : MonoBehaviour {

    public Transform player;
    //PlayerHealth playerHealth;
    //EnemyHealth enemyHealth;
    UnityEngine.AI.NavMeshAgent nav; //navigation move


    void Awake()
    {
        //player = GameObject.FindGameObjectWithTag("Player").transform; //finding tag -> finding player
        //playerHealth = player.GetComponent<PlayerHealth>();
        //enemyHealth = GetComponent<EnemyHealth>();
        //nav = GetComponent<UnityEngine.AI.NavMeshAgent>();
        nav = GetComponent<NavMeshAgent>();
    }

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        
        nav.SetDestination(player.position);
        //if (enemyHealth.currentHealth > 0 && playerHealth.currentHealth > 0 && playerHealth.clear != true)
        //{
        //    nav.SetDestination(player.position);
        //}

        //else// if enemy has died,
        //{

        //    nav.enabled = false;
        //}

        //if (playerHealth.clear == true)
        //{
        //    nav.enabled = false;
        //}

    }
}
