using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

/// <summary>
/// Process return to main scene
/// </summary>
/// <author> SangJun, YeHun </author>
public class GameToMainButton : MonoBehaviour, IPointerEnterHandler, IPointerDownHandler {

    public AudioClip[] _soundlist;  // 음원 파일

    private Animator anim;  // Animator
    private AudioSource _audio;

    private void Awake()
    {
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;

        anim = GetComponent<Animator>();
        _audio = gameObject.AddComponent<AudioSource>();
    }

    /// <summary>
    /// Go to Main Scene
    /// </summary>
    public void GoMain()
    {
        SceneManager.LoadScene("GameIntroScene");

        Debug.Log("Go to main scene");
    }

    /// <summary>
    /// Execute when Move Mouse Pointer to Button
    /// </summary>
    public void OnPointerEnter(PointerEventData eventData)
    {
        anim.SetTrigger("Is_Highlight");
        _audio.clip = _soundlist[1];
        _audio.Play();
    }

    /// <summary>
    /// Execute when Click Exit Button
    /// </summary>
    public void OnPointerDown(PointerEventData eventData)
    {
        _audio.clip = _soundlist[0];
        _audio.Play();
    }
}
