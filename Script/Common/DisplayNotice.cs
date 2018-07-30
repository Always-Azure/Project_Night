using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Show Object's notice to player camera
/// </summary>
/// <author> YeHun </author>
public class DisplayNotice : MonoBehaviour {

    private Transform _tr;   // Canvas Transform
    private Transform _mainCameraTr; // Main Camera's Trasnform

    private void Awake()
    {
        _tr = GetComponent<Transform>();
        _mainCameraTr = Camera.main.transform;
    }
	
	void Update () {
        // Main Camera 방향으로 Canas를 돌려줌으로써, 안내화면을 Player가 볼 수 있다.
        _tr.LookAt(_mainCameraTr);

        // Camera쪽으로 바라보면 글이 좌, 우 반전되어 보인다. 그렇기에 180도 돌려주는 것.
        _tr.Rotate(0, 180, 0);
	}
}
