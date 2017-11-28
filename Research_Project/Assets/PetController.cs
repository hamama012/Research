using UnityEngine;
using System.Collections;

public class PetController : MonoBehaviour {
	public Transform player; //メインカメラの位置
	public Transform hand; //仮想ハンドの位置
    bool dogContact = false; //回転開始
    float rotationSpeed= 0; //回転時の速度

	public float speed = 3.0f; //移動速度
	public float limitDistance = 2.0f; //プレイヤーに一定間空ける距離

//	private bool collisionCheck = false;
	private bool walkStart = false;
	private Animator animator;

	// Use this for initialization
	void Start () {
        //StartCoroutine(dogloop());
        //タグのつけられたオブジェクトの位置取得
        player = GameObject.FindGameObjectWithTag("MainCamera").transform;
		hand = GameObject.FindGameObjectWithTag("Hand").transform;

		//アニメーション
		animator = GetComponent<Animator>();
	}
	
	// Update is called once per frame
	void Update () {
        Vector3 playerPos = player.position; //プレイヤーの位置
		Vector3 direction = player.position - transform.position; //方向

        Quaternion targetRotation = Quaternion.LookRotation(playerPos - transform.position);

        //y軸にモデルが動かないようxz軸間のみの距離を取得
        playerPos.y = 0f;
		Vector3 direction2 = playerPos - transform.position;
		float distance = direction2.sqrMagnitude; //距離

		direction = direction.normalized; //単位化（距離要素を取り除く）
		direction.y = 0f;

		//左クリックでプレイヤーの方に回転、移動
		if (Input.GetMouseButtonDown(0)) {
			GetComponent<HeadLookController>().enabled = true;
            //Quaternion targetRotation = Quaternion.LookRotation(playerPos - transform.position);

         // 犬をゆっくり振り向かせる
            targetRotation = Quaternion.LookRotation(playerPos - transform.position);
            dogContact = true;
			walkStart = true;

		}

        rotationSpeed += 0.0015f;


        if (dogContact)
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed);
            
            if (rotationSpeed >= 1) rotationSpeed = 0;
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

        var elr = transform.rotation.eulerAngles;
        transform.rotation = Quaternion.Euler(0, elr.y, elr.z); //Updateごとに強制的にx=0
        //Time.deltaTimeはどんなPC環境でも1秒ごとにフレームを更新するもの、と考えておけばよい
    }

    //IEnumerator dogloop(Quaternion targetRotation)
    //{
    //    Debug.Log("VVVVVVV");

    //    while (true)
    //    {
    //        yield return new WaitForSeconds(10);
    //        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime);
    //        Debug.Log("AAAAAAAAA");
    //    }
    //}

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


