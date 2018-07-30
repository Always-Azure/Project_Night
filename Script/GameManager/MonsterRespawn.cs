using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Manage monster spawn.
/// 1. Load Monster prefabs in advance.
/// 2. Create Monster in range of player(-15 ~ 15)
/// </summary>
/// <author> YeHun </author>
public class MonsterRespawn : MonoBehaviour {

    private Dictionary<string, GameObject> _cash;   // All Loaded Monster Prefabs
    private int[] _unit = { -1, 1 }; // 플레이어 주위에 몬스터를 생성하기 위한 unit

    private void Awake()
    {
        _cash = new Dictionary<string, GameObject>();
        LoadPrefabs();
    }

    /// <summary>
    /// Load Monsters Prefabs to _cash
    /// </summary>
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

    /// <summary>
    ///  Create Monsters in 10 units
    /// </summary>
    /// <param name="pos"> Player Location </param>
    /// <param name="name"> Monster name </param>
    public void CreateMonster (Transform pos, string name)
    {
        Vector3 tmp = pos.position;
        float x, z;
    
        // Check the created location
        while (true)
        {
            x =(Random.Range(0, 15) * _unit[Random.Range(0, 2)]);
            z =(Random.Range(0, 15) * _unit[Random.Range(0, 2)]);

            if (100 > ((x * x) + (z * z)))
                continue;

            break;
        }
        tmp.x += x;
        tmp.z += z;
        tmp.y = tmp.y + 5;

        GameObject obj = Instantiate(_cash[name], tmp, new Quaternion());
        Scene scene = SceneManager.GetSceneAt(1);
        SceneManager.MoveGameObjectToScene(obj, scene);

        Debug.Log("MonsterRespawn - CreateMonster, name : " + name);

    }
}
