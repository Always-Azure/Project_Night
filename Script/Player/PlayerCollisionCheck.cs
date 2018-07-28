using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCollisionCheck : MonoBehaviour {
    
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if(hit.gameObject.tag == "Wall")
        {
            Debug.Log("Wall Collision");
        }
    }

    void OnTriggerEnter(Collider col)
    {
        if(col.gameObject.tag == "Posion")
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
    }

}
