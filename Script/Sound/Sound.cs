using UnityEngine.Audio;
using UnityEngine;
using System;

public class Sound : IDisposable{

    // Property
    public AudioClip Clip { get { return _clip; } }
    public AudioSource AudioSource { get { return _audioSource; } set { _audioSource = value; } }
    public string Name { get { return _name; } }
    public string Path { get { return _path; } }
    public float Volume { get { return _volume; } }
    public float Pitch { get { return _pitch; } }

    private AudioClip _clip;    // 음원 파일
    private AudioSource _audioSource; // 실행되는 위치
    private string _name;   // 음원 이름
    private string _path;   // 음원 경로. Root = Asset
    private float _volume; // 소리 크기
    private float _pitch; // 소리 높낮이

    public Sound(string name, string path)
    {
        //AudioClip clip = AssetDatabase.LoadAssetAtPath<AudioClip>(path);
        AudioClip clip = Resources.Load<AudioClip>(path);
        _clip = clip;
        _audioSource = null;
        _name = name;
        _path = path;
        _volume = 1.0f;
        _pitch = 0.0f;
    }
    public Sound(AudioSource audioSource, string name, string path)
    {
        AudioClip clip = Resources.Load<AudioClip>(path);
        _clip = clip;
        _audioSource = audioSource;
        _name = name;
        _path = path;
        _volume = 1.0f;
        _pitch = 0.0f;
    }

    public void Play()
    {
        if (_clip == null)
        {
            Debug.LogWarning("사운드 파일이 없습니다.");
            return;
        }

        if(_audioSource == null)
        {
            Debug.LogWarning("AudioSoure 정보가 없습니다.");
            return;
        }

        _audioSource.clip = _clip;
        _audioSource.Play();
    }
    public void Play(AudioSource audioSource)
    {
        if(audioSource == null)
        {
            Debug.LogWarning("AudioSoure 정보가 없습니다.");
            return;
        }

        if (_clip == null)
        {
            Debug.LogWarning("사운드 파일이 없습니다.");
            return;
        }

        audioSource.clip = _clip;
        audioSource.Play();
    }

    // 리소스 해제
    #region IDisposable Support
    private bool disposedValue = false; // 중복 호출을 검색하려면

    protected virtual void Dispose(bool disposing)
    {
        if (!disposedValue)
        {
            if (disposing)
            {
                _clip.UnloadAudioData();
                _audioSource = null;
                _name = null;
                _path = null;
                _volume = 0;
                _pitch = 0;
                // TODO: 관리되는 상태(관리되는 개체)를 삭제합니다.
            }

            // TODO: 관리되지 않는 리소스(관리되지 않는 개체)를 해제하고 아래의 종료자를 재정의합니다.
            // TODO: 큰 필드를 null로 설정합니다.

            disposedValue = true;
        }
    }

    // TODO: 위의 Dispose(bool disposing)에 관리되지 않는 리소스를 해제하는 코드가 포함되어 있는 경우에만 종료자를 재정의합니다.
    // ~Sound() {
    //   // 이 코드를 변경하지 마세요. 위의 Dispose(bool disposing)에 정리 코드를 입력하세요.
    //   Dispose(false);
    // }

    // 삭제 가능한 패턴을 올바르게 구현하기 위해 추가된 코드입니다.
    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }
    #endregion
}
