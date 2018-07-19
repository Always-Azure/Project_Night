using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour {

    public static UIManager instance;

    private Slider _hpSlider;
    private Slider _batterySlider;

    private Dictionary<string, GameObject> _batteryImage;

    private void Awake()
    {
        instance = this;
    }

    // Use this for initialization
    void Start () {        
        _hpSlider = GameObject.Find("HpBar").GetComponent<Slider>();
        _batterySlider = GameObject.Find("BatteryBar").GetComponent<Slider>();

        // 이렇게 2가지 방법이 가능하다.
        _hpSlider.onValueChanged.AddListener(delegate { OnHpChanged(); });
        _batterySlider.onValueChanged.AddListener(OnBatteryChanged);

        _batteryImage = new Dictionary<string, GameObject>();
        _batteryImage.Add("Battery3/3",GameObject.Find("Battery3/3").gameObject);
        _batteryImage.Add("Battery2/3", GameObject.Find("Battery2/3").gameObject);
        _batteryImage.Add("Battery1/3", GameObject.Find("Battery1/3").gameObject);
    }

    // Update Hp Bar
    private void OnHpChanged()
    {
        // Use changed value -> _hpSlider.value;
    }

    // Update Battery Bar
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

    public void SetHpValue(float value)
    {
        _hpSlider.value = value;
    }

    public void SetBatteryValue(float value)
    {
        _batterySlider.value = value;
    }
}
