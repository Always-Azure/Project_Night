using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class GameStartButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler {

    private Dictionary<string, Sprite> _cash_image;
    private Image _Image;
    private AudioSource _audio;

    public AudioClip[] _soundlist;
    
    // Use this for initialization
    void Start () {
        object[] temp;

        _cash_image = new Dictionary<string, Sprite>();
        _Image = GetComponent<Image>();
        _audio = gameObject.AddComponent<AudioSource>();

        temp = Resources.LoadAll<Sprite>("Image/Button");

        foreach (object tmp in temp)
        {
            Sprite sprite = tmp as Sprite;
            _cash_image.Add(sprite.name, sprite);
        }
    }

    public void GameStart()
    {
        // Player choose GameStart button.
        // Scene change the Scene that player died.
        SceneManager.LoadScene("Manager");
        SceneManager.LoadScene("Stage1", LoadSceneMode.Additive);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        _Image.sprite = _cash_image["start_btn_highlighted_white"];

        _audio.clip = _soundlist[1];
        _audio.Play();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        _Image.sprite = _cash_image["start_btn_normal_white"];
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        _Image.sprite = _cash_image["start_btn_clicked_white"];

        _audio.clip = _soundlist[0];
        _audio.Play();

        //GameStart();
    }
}
