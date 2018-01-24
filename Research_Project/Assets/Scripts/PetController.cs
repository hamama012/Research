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

	private bool collisionCheck = false;
	private bool walkStart = false;
    public int dirStart = 0; //視線先チェック変数（カメラ方向を1、ハンド方向を2、自由方向を0）

    public int colCount = 0; //衝突判定時間カウント
    public int colCount2 = 0; //衝突離脱時間カウント

    private Animator animator;

    public GameObject refObj;
    public GameObject colHand;

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

        //顔だけ向けて一時停止
        if(playerControll.release == false)
        {
            animator.SetBool("walk", false);
            movePet.enabled = false;
            GetComponent<HeadLookController>().enabled = true;
        }

        //トリガーが深く引かれた際にプレイヤー側に回転、移動
        if (playerControll.call == true && playerControll.release == false)
        {
            animator.SetBool("walk", true);

            //回転処理
            targetRotation = Quaternion.LookRotation(playerPos - transform.position);
            dogContact = true;
        }

        //触れ合い可能状態から待機状態へ戻す
        else if (playerControll.release == true && movePet.enabled == false) {
 
            movePet.enabled = true;
            playerControll.call = false;

            GetComponent<HeadLookController>().enabled = false;
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
            animator.SetBool("run", false);
            animator.SetBool("walk", false);
            playerControll.call = false;
            walkStart = false;
            dogContact = false;
            movePet.enabled = false;
        }

        //衝突判定なし時間カウント
        if (colCount > 0)
            colCount++;
        if (colCount == 40)
        {
            colCount = 0;
        }

        //衝突離脱時間カウント
        if (colCount2 > 0)
            colCount2++; //衝突離脱以降カウント
        if (colCount2 > 80)
        {
            //離脱後衝突せず30フレーム経過した時
            //尻尾停止、衝突離脱カウントリセット、視線方向をカメラに
            animator.SetBool("tail", false);
            colCount2 = 0;
            dirStart = 1;
        }

        //犬の座標固定
        var elr = transform.rotation.eulerAngles;
        transform.rotation = Quaternion.Euler(0, elr.y, elr.z); //Updateごとに強制的にx=0
    }

	//衝突判定(尻尾モーション)
	void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject == colHand && colCount == 0)
        {
            dirStart = 2;
            animator.SetBool("tail", true);
            colCount++;

        }

        colCount2 = 0;
    }

    void OnCollisionExit(Collision collision)
    {
        if (colCount > 0)
            colCount2++;

        collisionCheck = true;

    }


}


