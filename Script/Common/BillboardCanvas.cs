using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BillboardCanvas : MonoBehaviour {

    private Transform tr;
    private Transform mainCameraTr;

	// Use this for initialization
	void Start () {
        tr = GetComponent<Transform>();
        mainCameraTr = Camera.main.transform;
	}
	
	// Update is called once per frame
	void Update () {
        tr.LookAt(mainCameraTr);

        // Camera쪽으로 바라보면 글이 좌, 우 반전되어 보인다. 그렇기에 180도 돌려주는 것.
        tr.Rotate(0, 180, 0);
	}
}
