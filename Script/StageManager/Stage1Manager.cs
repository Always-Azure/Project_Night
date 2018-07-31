using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Handle general Stage 1 data and process
/// </summary>
/// <author> YeHun </author>
public class Stage1Manager : MonoBehaviour {

    // manager
    private GameManager _gameManager;

    // location
    private Vector3 _location_player;   // 플레이어 시작 위치
    private Vector3 _location_house;    // 집 위치
    private Vector3 _location_streetlight;  // 가로등 위치

    // Common
    private float _timer;
    private int _monsterLimit;  // 최대 몬스터 생성 수

    private void Awake()
    {
        init();
    }
    
    private void Update()
    {
        if (_timer > 2.0f)
        {
            if (_gameManager.GetPlayerState() == STATE_PLAYER.SAFETY)
                return;

            //_gameManager.CreateMonster("Rabbit", _monsterLimit);
            _timer = 0;
        }

        _timer += Time.deltaTime;
    }

    // Initiallize All
    private void init()
    {
        _gameManager = GameManager.instance;
        _gameManager.InitStage();
        _gameManager.stageNum = 1;

        _monsterLimit = 20;

        InitMap();  // House, StreetLight initialize
        TreasureboxInit(2); // TreasureBox initialize

        Debug.Log("Stage 1 Init");
    }

    /// <summary>
    /// Initialize Map Information.
    /// </summary>
    private void InitMap()
    {
        GameObject all_location_house;
        GameObject all_location_streetlight;

        all_location_house = GameObject.Find("Location_House").gameObject;
        all_location_streetlight = GameObject.Find("Location_StreetLight").gameObject;

        // 집위치 랜덤으로 초기화
        int random = Random.Range(0, 3);
        _location_house = all_location_house.transform.GetChild(random).position;

        // 가로등 위치 랜덤으로 초기화
        random = Random.Range(0, 3);
        _location_streetlight = all_location_streetlight.transform.GetChild(random).position;

        // player 위치 초기화
        _location_player = GameObject.Find("Location_Player").transform.position;

        // Create Objects
        _gameManager.SetPlayerLocation(_location_player);
        _gameManager.CreateObject(_location_house, "House");
        _gameManager.CreateObject(_location_streetlight, "Street_Light");
    }

    /// <summary>
    /// Initialize TreasureBox
    /// 1. Check Area - just Area have 1 treasurebox
    /// 2. Check Location
    /// 3. Check constraint location
    /// 4. Create TreasureBox
    /// </summary>
    /// <param name="boxnum"> number you want to create TreasureBox </param>
    private void TreasureboxInit(int boxnum)
    {
        GameObject all_location_treasurebox = GameObject.Find("Location_Treasurebox");
        int[,] location = new int[boxnum, 2];   // [boxnum, 0] = Area, [boxnum, 1] = Location.
        List<string> locationlist = new List<string>();
        int randomNum = 0;
        string locationName = null;

        // Add constraint data - location that must not exist together
        Dictionary<string, List<string>> errorlocation = new Dictionary<string, List<string>>();
        errorlocation.Add("a2", new List<string>());
        errorlocation["a2"].Add("b1");
        errorlocation.Add("b1", new List<string>());
        errorlocation["b1"].Add("a2");
        errorlocation["b1"].Add("c1");
        errorlocation.Add("c1", new List<string>());
        errorlocation["c1"].Add("b1");
        errorlocation.Add("c2", new List<string>());
        errorlocation["c2"].Add("d1");
        errorlocation.Add("d1", new List<string>());
        errorlocation["d1"].Add("c2");
        errorlocation["d1"].Add("a3");
        errorlocation.Add("a3", new List<string>());
        errorlocation["a3"].Add("d1");
        
        // a,b,c,d 중 생성 위치 선정
        for (int i = 0; i<boxnum; i++)
        {
            location[i,0] = Random.Range(1, 5);

            // 중복체크
            for(int j = 0; j<i; j++)
            {
                if(location[i, 0] == location[j, 0])
                {
                    location[i, 0] = Random.Range(1, 5);
                    j = -1;
                }
            }
        }

        // 박스 생성.
        int count = 0;
        while (count < boxnum)
        {
            switch (location[count, 0])
            {
                case 1: // a위치
                    randomNum = Random.Range(1, 4);
                    locationName = "a";
                    break;

                case 2: // b위치
                    randomNum = Random.Range(1, 3);
                    locationName = "b";
                    break;

                case 3: // c위치
                    randomNum = Random.Range(1, 4);
                    locationName = "c";
                    break;

                case 4: // d위치
                    randomNum = Random.Range(1, 3);
                    locationName = "d";
                    break;
            }

            // 위치 체크
            string current = locationName + randomNum;

            if(locationlist.Count == 0)
            {
                locationlist.Add(current);
                location[count, 1] = randomNum;

                count++;
                continue;
            }

            // 상자들이 서로 제한위치에 생성되었는지 확인.
            // 제한 위치에 있다면, 다시 랜덤 생성
            for (int i = 0; i < locationlist.Count; i++)
            {
                if (errorlocation.ContainsKey(current) == true)
                {
                    if (errorlocation[current].Contains(locationlist[i]) == true)
                    {
                        break;
                    }
                }

                if (i + 1 == locationlist.Count)
                {
                    locationlist.Add(current);
                    location[count, 1] = randomNum;

                    count++;
                    break;
                }
            }
        }

        // 해당 위치에 상자 생성.
        for(int i = 0; i<boxnum; i++)
        {
            Vector3 pos = all_location_treasurebox.transform.Find("Area" + location[i, 0]).transform.Find("Loc" + location[i, 1]).transform.position;
            _gameManager.CreateObject(pos, "treasurebox(with Notice)");
        }
    }
}
