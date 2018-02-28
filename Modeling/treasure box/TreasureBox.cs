using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreasureBox : MonoBehaviour {

    public Item item;
    public int asdf;

    private GameObject Notice;
    private STATE_BOX state;
    private bool isAvailable;

    private Animator anim; // 애니메이션

	// Use this for initialization
	void Start () {
        Notice = transform.Find("Canvas").gameObject;
        state = STATE_BOX.CLOSE;
        anim = GetComponent<Animator>();
        isAvailable = false;
	}
	
	// Update is called once per frame
	void Update () {
	}

   private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Player")
        {
            if (state == STATE_BOX.OPEN)
                return;

            Notice.SetActive(true);
            isAvailable = true;
            // anim.CrossFade("BoxOpen", 1); // 애니메이션을 부드럽게 이어주는 것.
            Debug.Log("진입");
        }
    }

    /*
     * 음... TriggerStay로 해주는게 좋을지, Flag를 걸어서 Update에 해주는게 좋을지 모르겠다.
     */
    private void OnTriggerStay(Collider other)
    {
        if(other.gameObject.tag == "Player")
        {
            if (Input.GetKeyDown(KeyCode.F) && state == STATE_BOX.CLOSE)
            {
                state = STATE_BOX.OPEN;
                Notice.SetActive(false);
                anim.Play("BoxOpen");
                item.OnBoxOpen();
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if(other.gameObject.tag == "Player")
        {
            Notice.SetActive(false);
            isAvailable = false;
            Debug.Log("탈출");
        }
    }
}
