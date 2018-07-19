using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ResourcesManager : MonoBehaviour {

    private Dictionary<string, GameObject> _cash;

	// Use this for initialization
	void Start () {
        _cash = new Dictionary<string, GameObject>();
        LoadPrefabs();
	}

    private void LoadPrefabs()
    {
        Debug.Log("ResourcesManager - LoadPrefabs");

        object[] temp = Resources.LoadAll("Objects");

        foreach(GameObject obj in temp)
        {
            Debug.Log(obj.name);
            _cash[obj.name] = obj;
        }
    }

    public void CreateObject(Vector3 pos, string name)
    {
        Debug.Log("ResourcesManager - CreateObject");

        GameObject obj = Instantiate(_cash[name], pos, new Quaternion());
        // Hierarchy에 나와있는 Scene순서대로 num을 생각해, 그 순서의 scene정보를 획득.
        Scene scene = SceneManager.GetSceneAt(1);

        // 생성된 object를 해당 Scene으로 옮긴다.
        // 왜 해주냐면, 해당 씬에서 사용한 정보를 한 번에 옮기기 위해서!
        SceneManager.MoveGameObjectToScene(obj, scene);
    }
}