using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateObject : MonoBehaviour {

    public GameObject House;
    public GameObject HouseLoc1;
    public GameObject HouseLoc2;
    public GameObject HouseLoc3;
    public GameObject ItemBox;

    int BoxNum = 8;

    // Use this for initialization
    void Start () {
        int RandNum = Random.Range(1, 30);
        switch(RandNum/10)
        {
            case 0:
                Instantiate(House, HouseLoc1.transform.position, Quaternion.Euler(0, 0, 0));
                break;
            case 1:
                Instantiate(House, HouseLoc2.transform.position, Quaternion.Euler(0, 0, 0));
                break;
            default:
                Instantiate(House, HouseLoc3.transform.position, Quaternion.Euler(0, 0, 0));
                break;
        }

        for(int i=0; i<BoxNum; ++i)
        {
            Instantiate(
                ItemBox, 
                new Vector3(Random.Range(0, 500), 200, Random.Range(0, 500)),
                Quaternion.Euler(0, 0, 0)
                );
        }

    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
