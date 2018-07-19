using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIHp : MonoBehaviour {

    private Slider _hpSlider;

	// Use this for initialization
	private void Awake () {
        _hpSlider = GameObject.Find("HpBar").GetComponent<Slider>();
        _hpSlider.onValueChanged.AddListener(delegate { OnHpChanged(); });
	}
	
    // functions that execute when HpSlider is changed
    private void OnHpChanged()
    {

    }

    public void SetHpValue(float value)
    {
        _hpSlider.value = value;
    }
}
