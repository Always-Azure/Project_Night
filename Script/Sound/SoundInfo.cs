using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Sound Information.
/// You can handle this like windows folder(sound folder).
/// </summary>
/// <author> YeHun </author>
public class SoundInfo {

    // Property
    public string Name { get { return _name; } }
    public string Path { get { return _path; } }

    private string _name;   // 이름
    private string _path;   // 경로
    private Dictionary<string, SoundInfo> _subdirList;  // 디렉토리 리스트
    private Dictionary<string, Sound> _soundList;   // 사운드 리스트

    public SoundInfo(string name, string path)
    {
        _name = name;
        _path = path;
        _subdirList = new Dictionary<string, SoundInfo>();
        _soundList = new Dictionary<string, Sound>();
    }

    /// <summary>
    /// Add sub directory
    /// </summary>
    /// <param name="name"> File name </param>
    /// <param name="fullname"> File path </param>
    public void AddDIr(string name, string fullname)
    {
        // Check duplication
        if(_subdirList.ContainsKey(name) == true)
        {
            Debug.LogWarning("Already data exist");
            return;
        }

        SoundInfo newinfo = new SoundInfo(name, fullname);
        _subdirList.Add(name, newinfo);
    }

    /// <summary>
    /// Add Sound data
    /// </summary>
    /// <param name="name"> File name </param>
    /// <param name="fullname"> Sound data </param>
    public void AddSound(string name, Sound sound)
    {
        if(_soundList.ContainsKey(name) == true)
        {
            Debug.LogWarning("Already data exits");
            return;
        }
        _soundList.Add(name, sound);
    }

    /// <summary>
    /// Update sub directory
    /// </summary>
    /// <param name="name"></param>
    /// <param name="newName"></param>
    /// <param name="path"></param>
    public void UpdateSubDir(string name, string newName, string path)
    {
        if (_subdirList.ContainsKey(name) == false)
        {
            Debug.LogWarning("There is no key : " + name);
            return;
        }

        _subdirList[name]._name = newName;
        _subdirList[name]._path = path;
    }
    /// <summary>
    /// Update Sound data
    /// </summary>
    /// <param name="name"></param>
    /// <param name="sound"></param>
    public void UpdateSound(string name, Sound sound)
    {
        if(_soundList.ContainsKey(name) == false)
        {
            Debug.LogWarning("There is no key : " + name);
            return;
        }
        // Delete origin data
        Sound temp = _soundList[name];
        temp.Dispose();

        // Update sound file
        _soundList[name] = sound;
    }

    /// <summary>
    /// Get Sub directory data
    /// </summary>
    /// <param name="name"></param>
    /// <returns></returns>
    public SoundInfo GetSubDir(string name)
    {
        if (_subdirList.ContainsKey(name) == false)
        {
            Debug.LogWarning("There is no key : " + name);
            return null;
        }

        return _subdirList[name];
    }
    /// <summary>
    /// Get Sound data
    /// </summary>
    /// <param name="name"></param>
    /// <returns></returns>
    public Sound GetSound(string name)
    {
        if (_soundList.ContainsKey(name) == false)
        {
            Debug.LogWarning("There is no key : " + name);
            return null;
        }

        return _soundList[name];
    }
    /// <summary>
    /// Get All Sound data in this directory
    /// </summary>
    /// <param name="audioSource"></param>
    /// <returns></returns>
    public Dictionary<string, Sound> GetSoundList(AudioSource audioSource = null)
    {
        Dictionary<string, Sound> temp = new Dictionary<string, Sound>(_soundList);

        if(audioSource != null)
        {
            foreach (Sound sound in temp.Values)
            {
                sound.AudioSource = audioSource;
            }
        }
        return temp;
    }

    /// <summary>
    /// Add soundlist in origin soundlist. Concatenate Soundlist
    /// Why here? Because if i make this method in soundlist, i have to make new data type for concatenate method.
    /// Making cat mathod just for concatenate is not inefficient.
    /// So I make cat mathod here.
    /// </summary>
    /// <param name="destination"> origin soundlist </param>
    /// <param name="audiosource"> Audiosource in object that this soundlist must be played </param>
    /// <returns></returns>
    public Dictionary<string, Sound> ConcatenateSoundList(Dictionary<string, Sound> destination, AudioSource audiosource)
    {
        Dictionary<string, Sound> temp = new Dictionary<string, Sound>(_soundList);

        foreach (KeyValuePair<string, Sound> value in temp)
        {
            value.Value.AudioSource = audiosource;
            destination.Add(value.Key, value.Value);
        }

        return destination;
    }
}
