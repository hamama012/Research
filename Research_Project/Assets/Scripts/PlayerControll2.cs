using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControll2 : MonoBehaviour {
    Vector3 screenPoint;

    // Use this for initialization
    void Start () {

        this.screenPoint = Camera.main.WorldToScreenPoint(transform.position);
        Vector3 a = new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenPoint.z); //ワールド座標に変換
        transform.position = Camera.main.ScreenToWorldPoint(a); //代入
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
