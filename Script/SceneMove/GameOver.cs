using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement; // define SceneManager

public class GameOver : MonoBehaviour {

	// Use this for initialization
	void Start () {
        AudioManager.instance.SoundPlayBackground("Background_Fail");
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    //public void RetryButton()
    //{
    //    //Player choose GameStart button.
    //    //Scene change the Scene that player died.

    //    SceneManager.LoadScene("GameIntroScene");
    //}

    public void GoMainButton()
    {
        //Player choose GameStart button.
        //Scene change the Scene that player died.

        SceneManager.LoadScene("GameIntroScene");
    }
}
