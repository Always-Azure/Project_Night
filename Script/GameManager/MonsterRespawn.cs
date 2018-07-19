using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MonsterRespawn : MonoBehaviour {

    private Dictionary<string, GameObject> _cash;
    private int[] unit = { -1, 1 };

	// Use this for initialization
	void Start () {
        _cash = new Dictionary<string, GameObject>();
        LoadPrefabs();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    // Load Monsters Prefabs to _cash
    private void LoadPrefabs()
    {
        Debug.Log("MonsterRespawn - LoadPrefabs");

        object[] tmp = Resources.LoadAll("Monsters");

        foreach (GameObject obj in tmp)
        {
            Debug.Log(obj.name);
            _cash[obj.name] = obj;
        }
    }

    // Create Monsters out of 10 units
    public void CreateMonster (Transform pos, string name)
    {
        Debug.Log("MonsterRespawn - CreateMonster");

        Vector3 tmp = pos.position;
        float x, z;
    
        while (true)
        {
            x =(Random.Range(0, 15) * unit[Random.Range(0, 2)]);
            z =(Random.Range(0, 15) * unit[Random.Range(0, 2)]);


            if (100 > ((x * x) + (z * z)))
                continue;

            // Terrain 위치 수정하고 돌려볼 것.
            //if (x > 70 || x < 430 || z > 70 || z < 430)
            //    continue;

            break;
        }
        tmp.x += x;
        tmp.z += z;
        tmp.y = tmp.y + 5;

        GameObject obj = Instantiate(_cash[name], tmp, new Quaternion());
        Scene scene = SceneManager.GetSceneAt(1);
        SceneManager.MoveGameObjectToScene(obj, scene);
    }
}
