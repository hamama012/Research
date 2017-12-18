using UnityEngine;
using System.Collections;

public class PetController : MonoBehaviour {
	public Transform player; //メインカメラ
	public Transform hand; //仮想ハンド
    public Transform complition; //補完用オブジェクト
    public Transform lastTarget; //lookTarget

    bool dogContact = false; //回転開始
    float rotationSpeed= 0; //回転時の速度

	public float speed = 3.0f; //移動速度
	public float limitDistance = 2.0f; //プレイヤーに一定間空ける距離

//	private bool collisionCheck = false;
	private bool walkStart = false;
	private Animator animator;

    public GameObject refObj;

    // Use this for initialization
    void Start () {
        //StartCoroutine(dogloop());
        //タグのつけられたオブジェクトの位置取得
        player = GameObject.FindGameObjectWithTag("MainCamera").transform;
        //hand = GameObject.FindGameObjectWithTag("Hand").transform;
        complition = GameObject.Find("FaceComplition").transform;

		//アニメーション
		animator = GetComponent<Animator>();
	}
	
	// Update is called once per frame
	void Update () {
        Vector3 playerPos = player.position; //プレイヤーの位置
		Vector3 direction = playerPos - transform.position; //方向
        PlayerContoroll playerControll = refObj.GetComponent<PlayerContoroll>(); //コントローラのスクリプト取得
        Quaternion targetRotation = Quaternion.LookRotation(playerPos - transform.position);
        MovePet movePet = GetComponent<MovePet>();

        //y軸にモデルが動かないようxz軸間のみの距離を取得
        playerPos.y = 0f;
		Vector3 direction2 = playerPos - transform.position;
		float distance = direction2.sqrMagnitude; //距離

		direction = direction.normalized; //単位化（距離要素を取り除く）
		direction.y = 0f;

        //トリガーが深く引かれた際にプレイヤー側に回転、移動
        if (playerControll.call == true && movePet.enabled == true)
        {
            Debug.Log("1");
            GetComponent<HeadLookController>().enabled = true;

            //回転処理
            targetRotation = Quaternion.LookRotation(playerPos - transform.position);
            dogContact = true;

        }

        //触れ合い可能状態から待機状態へ戻す
        else if (playerControll.call == true && movePet.enabled == false) {
            Debug.Log("aaaa");
            movePet.enabled = true;
            playerControll.call = false;
        }

            rotationSpeed += 0.0015f;

        //回転処理（犬をゆっくり振り向かせる部分）
        if (dogContact)
        {
            animator.SetBool("walk", true);　
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed);

            walkStart = true;
            if (rotationSpeed >= 1) rotationSpeed = 0;
        }

        //プレイヤーとの間に一定距離空けつつ移動
        if(distance >= limitDistance && walkStart == true){
            animator.SetBool("run", true);
            transform.position = transform.position + (direction * speed * Time.deltaTime);

        }

        //停止処理
        else if (distance < limitDistance && walkStart == true) {
            Debug.Log("ssss");
            animator.SetBool("run", false);
            animator.SetBool("walk", false);
            playerControll.call = false;
            walkStart = false;
            dogContact = false;
            movePet.enabled = false;
        }



        //犬の座標固定
        var elr = transform.rotation.eulerAngles;
        transform.rotation = Quaternion.Euler(0, elr.y, elr.z); //Updateごとに強制的にx=0
    }

	//衝突判定
	void OnCollisionEnter(Collision collision){
        
        //衝突時尻尾を振る処理
        if(collision.gameObject.tag == "Hand")
        {
            //Debug.Log("尻尾が動くよ");
        }

        //壁を避ける行動
    }

    void OnCollisionExit(Collision collision)
    {

        //衝突時尻尾を振る処理
        if (collision.gameObject.tag == "Hand")
        {
            //Debug.Log("尻尾が止まるよ");
        }
    }

}


