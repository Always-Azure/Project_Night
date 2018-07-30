using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using UnityEngine.UI;

/// <summary>
/// Process Game start
/// </summary>
/// <author> SangJun, YeHun </author>
public class GameStartButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler {

    public AudioClip[] _soundlist;  // 음원 파일

    private Dictionary<string, Sprite> _cash_image; // All Loaded Button Image
    private Image _Image;   // Now Button Image
    private AudioSource _audio;

    private void Awake()
    {
        object[] temp;

        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;

        // Load Button Images
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

    /// <summary>
    /// Start Game.
    /// </summary>
    public void GameStart()
    {
        SceneManager.LoadScene("Manager");
        SceneManager.LoadScene("Stage1", LoadSceneMode.Additive);

        Debug.Log("Game Start");
    }

    /// <summary>
    /// Execute when Move Mouse Pointer to Button
    /// </summary>
    public void OnPointerEnter(PointerEventData eventData)
    {
        _Image.sprite = _cash_image["start_btn_highlighted_white"];

        _audio.clip = _soundlist[1];
        _audio.Play();
    }

    /// <summary>
    /// Execute when Move out Mouse Pointer from Button
    /// </summary>
    public void OnPointerExit(PointerEventData eventData)
    {
        _Image.sprite = _cash_image["start_btn_normal_white"];
    }

    /// <summary>
    /// Execute when Click Exit Button
    /// </summary>
    public void OnPointerDown(PointerEventData eventData)
    {
        _Image.sprite = _cash_image["start_btn_clicked_white"];

        _audio.clip = _soundlist[0];
        _audio.Play();

        //GameStart();
    }
}
