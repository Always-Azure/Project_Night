using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement; // define SceneManager

public class GameEndTrigger : MonoBehaviour
{
    private SaveInfoManager _Manager;
    int StageNo = 0;
    // Use this for initialization
    void Start()
    {
        _Manager = GameObject.Find("SaveManager").GetComponent<SaveInfoManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if (StageNo == 0)
        {
            StageNo = _Manager.GetStageNo();
            Debug.Log(StageNo);
        }
    }

    void OnTriggerStay(Collider col)
    {
        if (col.gameObject.tag == "Player")
        {
            if (Input.GetKeyDown(KeyCode.F))
            {
                if (StageNo - 2 == 2) // Last Stage
                {

                    SceneManager.LoadScene("GameClearScene");
                    _Manager.Setddlod();
                }
                else
                {
                    string temp = "Stage";
                    int tmp = StageNo - 1;
                    temp += (tmp);
                    Debug.Log(temp);
                    //_Manager.Setddlod();
                    SceneManager.LoadScene(temp);
                }
            }
        }
    }

}
