using UnityEngine;
using System.Collections;

public class PlayerContoroll : MonoBehaviour {
	Vector3 screenPoint;

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {

		this.screenPoint = Camera.main.WorldToScreenPoint(transform.position);
		Vector3 a = new Vector3 (Input.mousePosition.x,Input.mousePosition.y,screenPoint.z); //ワールド座標に変換
		transform.position = Camera.main.ScreenToWorldPoint (a); //代入
	}
	void OnCollisionEnter(Collision collision){
		Debug.Log ("衝突検知");
	}
}
