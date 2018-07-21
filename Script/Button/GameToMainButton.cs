using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

public class GameToMainButton : MonoBehaviour, IPointerEnterHandler, IPointerDownHandler {

    private Animator anim;
    private AudioSource _audio;

    public AudioClip[] _soundlist;

    // Use this for initialization
    void Start () {
        anim = GetComponent<Animator>();
        _audio = gameObject.AddComponent<AudioSource>();
    }

    public void GoMain()
    {
        SceneManager.LoadScene("GameIntroScene");
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        anim.SetTrigger("Is_Highlight");
        _audio.clip = _soundlist[1];
        _audio.Play();
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        _audio.clip = _soundlist[0];
        _audio.Play();
    }
}
