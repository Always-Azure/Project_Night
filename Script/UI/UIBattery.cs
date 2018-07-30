using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Handle UI about Battery
/// </summary>
/// <author> YeHun </author>
public class UIBattery : MonoBehaviour
{
    private Slider _batterySlider;  // 배터리 UI
    private Dictionary<string, GameObject> _batteryImage;   // 배터리 이미지

    private void Awake()
    {
        _batterySlider = GameObject.Find("BatteryBar").GetComponent<Slider>();
        _batterySlider.onValueChanged.AddListener(delegate { OnBatteryChanged(_batterySlider.value); });

        // Get Battery Image.
        _batteryImage = new Dictionary<string, GameObject>();
        _batteryImage.Add("Battery3/3", GameObject.Find("Battery3/3").gameObject);
        _batteryImage.Add("Battery2/3", GameObject.Find("Battery2/3").gameObject);
        _batteryImage.Add("Battery1/3", GameObject.Find("Battery1/3").gameObject);
    }
    
    /// <summary>
    /// Change Image from value of battery.
    /// Execute when batterySlider's value is changed.
    /// </summary>
    /// <param name="value"> Battery Amount </param>
    private void OnBatteryChanged(float value)
    {
        if (value < 0.33)
        {
            _batteryImage["Battery3/3"].SetActive(false);
            _batteryImage["Battery2/3"].SetActive(false);
            _batteryImage["Battery1/3"].SetActive(false);
        }
        else if (value < 0.66)
        {
            _batteryImage["Battery3/3"].SetActive(false);
            _batteryImage["Battery2/3"].SetActive(false);
            _batteryImage["Battery1/3"].SetActive(true);
        }
        else if (value < 1)
        {
            _batteryImage["Battery3/3"].SetActive(false);
            _batteryImage["Battery2/3"].SetActive(true);
            _batteryImage["Battery1/3"].SetActive(false);
        }
        else
        {
            _batteryImage["Battery3/3"].SetActive(true);
            _batteryImage["Battery2/3"].SetActive(false);
            _batteryImage["Battery1/3"].SetActive(false);
        }
    }

    /// <summary>
    /// Set Battery Amount
    /// </summary>
    /// <param name="value"> Battery Amount </param>
    public void SetBatteryValue(float value)
    {
        _batterySlider.value = value;
    }
}
