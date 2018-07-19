using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NextStageTrigger : MonoBehaviour {

    private int stageNum = 0;
    // Use this for initialization
    void Start()
    {
        stageNum = GameManager.instance.stageNum;
    }

    void OnTriggerStay(Collider col)
    {
        if (col.gameObject.tag == "Player")
        {
            if (Input.GetKeyDown(KeyCode.F))
            {
                if(stageNum == 3)
                {
                    SceneManager.LoadScene("GameClearScene");
                }
                else
                {
                    string temp = "Stage" + (stageNum + 1);
                    Debug.Log(stageNum + "언로드할겁니다");
                    SceneManager.UnloadSceneAsync(stageNum);
                    SceneManager.LoadScene(temp, LoadSceneMode.Additive);
                }
            }
        }
    }

}
