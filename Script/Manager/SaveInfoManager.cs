using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SaveInfoManager : MonoBehaviour {
    private int StageNo = 0; // 스테이지 저장. 재도전시 사용.
    private bool ddload = false;


    // Use this for initialization
    void Start () {
        if (ddload == false)
        {
            DontDestroyOnLoad(gameObject);
            ddload = true;
        }
        else
        {
            Destroy(gameObject);
        }
    }
	
	// Update is called once per frame
	void Update () {
        if (SceneManager.GetActiveScene().buildIndex != 0
            && SceneManager.GetActiveScene().buildIndex != StageNo)
        {
            StageNo = SceneManager.GetActiveScene().buildIndex; // 현재 씬의 정보 저장.
            Debug.Log(StageNo);
        }
    }

    public int GetStageNo()
    {
        return StageNo;
    }
    public void Setddlod()
    {
        ddload = !ddload;
    }
}
