using UnityEngine;
using System.Collections;

public class PetController : MonoBehaviour {
	public Transform player; //プレイヤー(メインカメラ）の位置
	public Transform hand; //仮想ハンドの位置

	public float speed = 3.0f; //移動速度
	public float limitDistance = 2.0f; //プレイヤーに一定間空ける距離

//	private bool collisionCheck = false;
	private bool walkStart = false;
	private Animator animator;

	// Use this for initialization
	void Start () {
		//タグのつけられたオブジェクトの位置
		player = GameObject.FindGameObjectWithTag("MainCamera").transform;
		hand = GameObject.FindGameObjectWithTag("Hand").transform;

		//アニメーション
		animator = GetComponent<Animator>();
	}
	
	// Update is called once per frame
	void Update () {
		Vector3 playerPos = player.position; //プレイヤーの位置
		Vector3 direction = playerPos - transform.position; //方向

		//y軸にモデルが動かないようxz軸間のみの距離を取得
		playerPos.y = 0f;
		Vector3 direction2 = playerPos - transform.position;
		float distance = direction2.sqrMagnitude; //距離

		direction = direction.normalized; //単位化（距離要素を取り除く）
		direction.y = 0f;

		//左クリックでプレイヤーの方に回転
		if (Input.GetMouseButtonDown(0)) {
			GetComponent<HeadLookController>().enabled = true;
			transform.rotation = Quaternion.LookRotation(direction); 
			walkStart = true;
		}

		//プレイヤーとの間に一定距離空ける
		if(distance >= limitDistance && walkStart == true){
			animator.SetBool ("walk", true);
			transform.position = transform.position + (direction * speed * Time.deltaTime);
			GetComponent<MovePet>().enabled = false;

		} else if (distance < limitDistance) {
			animator.SetBool ("walk", false);
			walkStart = false;
		}

		//右クリックで視線先ターゲット変更
		if (Input.GetMouseButtonDown (1)) {
			HeadLookController codeH = GetComponent<HeadLookController> ();

			if (codeH.lookTarget == hand) codeH.lookTarget = player;
			else codeH.lookTarget = hand;
		}

		//Time.deltaTimeはどんなPC環境でも1秒ごとにフレームを更新するもの、と考えておけばよい
	}

	//衝突判定
	void OnCollisionStay(Collision collision){

		//視線先ターゲットの変更
		//if (collision.gameObject.tag == "Hand") {
			//HeadLookController codeH = GetComponent<HeadLookController> ();
			//codeH.lookTarget = hand;
		
		//}
	}

	//衝突離脱判定
	void OnCollisionExit(Collision collision){
		//codeH.lookTarget = player;
	}


}


