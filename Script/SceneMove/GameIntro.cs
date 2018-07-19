using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement; // define SceneManager
using UnityEngine.EventSystems; // for using OnPointerEnter
using UnityEngine.UI;

public class GameIntro : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{

    private Dictionary<string, Sprite> _cash_image;
    private Image _Image;

    // Use this for initialization
    void Start()
    {
        object[] temp;

        _cash_image = new Dictionary<string, Sprite>();
        _Image = GetComponent<Image>();

        temp = Resources.LoadAll<Sprite>("Image/Button");

        foreach (object tmp in temp)
        {
            Sprite sprite = tmp as Sprite;
            _cash_image.Add(sprite.name, sprite);
        }
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void OnPointerEnter(PointerEventData eventData)
    {

        _Image.sprite = _cash_image["start_btn_highlighted_black"];

    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (eventData.selectedObject.tag == "StartButton")
        {
            _Image.sprite = _cash_image["start_btn_normal_black"];
        }
    }
}
