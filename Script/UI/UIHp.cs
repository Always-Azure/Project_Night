using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Handle UI about player HP
/// </summary>
/// <author> YeHun </author>
public class UIHp : MonoBehaviour {

    private Slider _hpSlider;   // HP UI

	// Use this for initialization
	private void Awake () {
        _hpSlider = GameObject.Find("HpBar").GetComponent<Slider>();
        _hpSlider.onValueChanged.AddListener(delegate { OnHpChanged(); });
	}

    /// <summary>
    /// functions that execute when HpSlider is changed
    /// </summary>
    private void OnHpChanged()
    {

    }

    /// <summary>
    /// Set HP Amount
    /// </summary>
    /// <param name="value"> HP Amount </param>
    public void SetHpValue(float value)
    {
        _hpSlider.value = value;
    }
}
