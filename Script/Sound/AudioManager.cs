using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class AudioManager : MonoBehaviour {

    public static AudioManager instance;
    public SoundInfo SoundInfo { get { return _soundinfo; } }

    private SoundInfo _soundinfo;
    private AudioSource _background;
    
    private void Awake()
    {
        instance = this;
        string rootPath = "Assets/Sounds";
        _soundinfo = new SoundInfo("Sounds", rootPath);
        LoadSound(rootPath, _soundinfo);

        _background = gameObject.AddComponent<AudioSource>();
        _background.loop = true;
        SoundPlayBackground("Background_Playing");
    }

    private void Start()
    {
    }

    private void LoadSound(string rootPath, SoundInfo soundInfo)
    {
        System.IO.DirectoryInfo dirinfo = new System.IO.DirectoryInfo(rootPath);
        
        // 디렉토리 탐색
        foreach (System.IO.DirectoryInfo dir in dirinfo.GetDirectories())
        {
            string path = rootPath + "/" + dir.Name;

            soundInfo.AddDIr(dir.Name, path);
            LoadSound(path, soundInfo.GetSubDir(dir.Name));
        }

        // 파일 탐색
        foreach(System.IO.FileInfo file in dirinfo.GetFiles())
        {
            // meta파일 거르기
            if (file.Extension.CompareTo(".meta") == 0)
                continue;

            string name = file.Name.Split('.')[0];
            string path = rootPath + "/" + file.Name;
            
            Sound temp = new Sound(gameObject.GetComponent<AudioSource>(), name, path);
            soundInfo.AddSound(name, temp);
        }
    }

    public void SoundPlayBackground(string name)
    {
        _background.clip = _soundinfo.GetSubDir("Backgrounds").GetSound(name).Clip;
        _background.Play();
    }
}
