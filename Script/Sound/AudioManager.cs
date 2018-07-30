using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Handle general Audio process
/// Load all sound data before game start
/// You can find and use sound data like window folders
/// </summary>
/// <author> YeHun </author>
public class AudioManager : MonoBehaviour {

    public static AudioManager instance;

    // Property
    public SoundInfo SoundInfo { get { return _soundinfo; } }

    private SoundInfo _soundinfo;   // 사운드 정보
    private AudioSource _background;    // 배경 음악
    
    private void Awake()
    {
        string rootPath = "Assets/Resources/Sounds";

        instance = this;
        
        // Load Sound
        _soundinfo = new SoundInfo("Sounds", rootPath);
        LoadSound(rootPath, _soundinfo);

        // Load Background Sound
        _background = gameObject.AddComponent<AudioSource>();
        _background.loop = true;
        SoundPlayBackground("Background_Playing");

        Debug.Log("AudioManager - Init");
    }

    private void Start()
    {
    }

    /// <summary>
    /// Load Sound datas.
    /// 1. Search all directory in "Sounds" folder
    /// 2. FInd all sound files
    /// 3. Read sound file and make sound data.
    /// 4. Insert SoundInfo var.
    /// Result : You can find and use sound data like window folders
    /// </summary>
    /// <param name="rootPath"> rootPath </param>
    /// <param name="soundInfo"> sound information </param>
    private void LoadSound(string rootPath, SoundInfo soundInfo)
    {
        System.IO.DirectoryInfo dirinfo = new System.IO.DirectoryInfo(rootPath);

        // Search Directorys
        foreach (System.IO.DirectoryInfo dir in dirinfo.GetDirectories())
        {
            string path = rootPath + "/" + dir.Name;

            soundInfo.AddDIr(dir.Name, path);
            LoadSound(path, soundInfo.GetSubDir(dir.Name));
        }

        // Search Files
        foreach(System.IO.FileInfo file in dirinfo.GetFiles())
        {
            // filter ".meta" meta data
            if (file.Extension.CompareTo(".meta") == 0)
                continue;

            // get file name remove extension
            string name = file.Name.Split('.')[0];
            string path = rootPath + "/" + name;
            string folderPath = path.Substring(17);
            
            // make sound file.
            Sound temp = new Sound(name, folderPath);
            soundInfo.AddSound(name, temp);
        }
    }

    /// <summary>
    /// Play BGM
    /// </summary>
    /// <param name="name"> file name </param>
    public void SoundPlayBackground(string name)
    {
        _background.clip = _soundinfo.GetSubDir("Backgrounds").GetSound(name).Clip;
        _background.Play();
    }
}
