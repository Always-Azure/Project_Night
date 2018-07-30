using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Handle general Object data.
/// Load all sound data before game start.
/// You can get resources by name.
/// </summary>
/// <author> YeHun </author>
public class ResourcesManager : MonoBehaviour {

    private Dictionary<string, GameObject> _cash;   // All Loaded Resources datas

    private void Awake()
    {
        _cash = new Dictionary<string, GameObject>();
        LoadPrefabs();

        Debug.Log("ResourcesManager - Init");
    }

    // Use this for initialization
    void Start () {
	}

    /// <summary>
    /// Load Prefabs in "Asset/Resources"
    /// </summary>
    private void LoadPrefabs()
    {
        Debug.Log("ResourcesManager - LoadPrefabs");

        object[] temp = Resources.LoadAll("Objects");

        foreach(GameObject obj in temp)
        {
            _cash[obj.name] = obj;
        }
    }

    /// <summary>
    /// Create Objects by already loaded data
    /// </summary>
    /// <param name="pos"> position </param>
    /// <param name="name"> object name </param>
    public void CreateObject(Vector3 pos, string name)
    {
        Debug.Log("ResourcesManager - CreateObject");

        GameObject obj = Instantiate(_cash[name], pos, new Quaternion());

        // Move Object to stage scene
        Scene scene = SceneManager.GetSceneAt(1);
        SceneManager.MoveGameObjectToScene(obj, scene);
    }
}