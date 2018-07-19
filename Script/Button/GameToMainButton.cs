using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

public class GameToMainButton : MonoBehaviour, IPointerEnterHandler {

    private Animator anim;

	// Use this for initialization
	void Start () {
        anim = GetComponent<Animator>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void GoMain()
    {
        SceneManager.LoadScene("GameIntroScene");
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        anim.SetTrigger("Is_Highlight");
    }
}
