using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HPController : MonoBehaviour {

    private UIManager _uIManager;

    // Use this for initialization
    void Awake () {
    }

    private void Start()
    {
        _uIManager = UIManager.instance;
    }

    public void OnUpdated(float HP)
    {
        ChangeValue(HP);
    }

    private void ChangeValue(float value)
    {
        _uIManager.SetHpValue(value);
    }
}
