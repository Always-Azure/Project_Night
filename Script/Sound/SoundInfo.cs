using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundInfo {

    public string Name { get { return _name; } }
    public string Path { get { return _path; } }
    // public Dictionary<string, Sound> SoundList { get { return new Dictionary<string, Sound>(_soundList); } }

    private string _name;
    private string _path;
    private Dictionary<string, SoundInfo> _subdirList;
    private Dictionary<string, Sound> _soundList;

    public SoundInfo(string name, string path)
    {
        _name = name;
        _path = path;
        _subdirList = new Dictionary<string, SoundInfo>();
        _soundList = new Dictionary<string, Sound>();
    }

    // 서브 폴더 추가
    public void AddDIr(string name, string fullname)
    {
        // 중복인지 체크
        if(_subdirList.ContainsKey(name) == true)
        {
            // try-catch문을 사용할까...?
            Debug.LogWarning("Already data exist");
            return;
        }

        // 중복이 없다면 새로 추가
        SoundInfo newinfo = new SoundInfo(name, fullname);
        _subdirList.Add(name, newinfo);
    }
    // sound 추가.
    // sound 객채를 받아서 추가할 것인가? 사운드 정보를 전부 받아서 내부에서 추가할 것인가????
    public void AddSound(string name, Sound sound)
    {
        if(_soundList.ContainsKey(name) == true)
        {
            Debug.LogWarning("Already data exits");
            return;
        }
        _soundList.Add(name, sound);
    }

    // 서브폴더리스트 업데이트
    public void UpdateSubDir(string name, string foldername, string path)
    {
        if (_subdirList.ContainsKey(name) == false)
        {
            Debug.LogWarning("There is no key : " + name);
            return;
        }

        _subdirList[name]._name = foldername;
        _subdirList[name]._path = path;
    }
    public void UpdateSound(string name, Sound sound)
    {
        if(_soundList.ContainsKey(name) == false)
        {
            Debug.LogWarning("There is no key : " + name);
            return;
        }
        // 기존 sound 파일 삭제
        Sound temp = _soundList[name];
        temp.Dispose();

        // Update sound file
        _soundList[name] = sound;
    }

    // 서브폴더 정보 반환
    public SoundInfo GetSubDir(string name)
    {
        if (_subdirList.ContainsKey(name) == false)
        {
            Debug.LogWarning("There is no key : " + name);
            return null;
        }

        return _subdirList[name];
    }
    // sound file 반환
    public Sound GetSound(string name)
    {
        if (_soundList.ContainsKey(name) == false)
        {
            Debug.LogWarning("There is no key : " + name);
            return null;
        }

        return _soundList[name];
    }
    // sound list 반환
    public Dictionary<string, Sound> GetSoundList(AudioSource audioSource)
    {
        Dictionary<string, Sound> temp = new Dictionary<string, Sound>(_soundList);

        foreach(Sound sound in temp.Values)
        {
            sound.AudioSource = audioSource;
        }

        return temp;
    }
}
