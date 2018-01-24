using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DirectionManager : MonoBehaviour {
    public GameObject player; //メインカメラ
    public GameObject hand; //仮想ハンド
    public GameObject pet; //ペット

    public GameObject refObj2; //ペット
    public GameObject nextObj; //次の移動先である視線先ターゲット（衝突判定に使用）

    public Vector3 direction;
    public Vector3 lookPos; //LookHeadで指定している視線先ターゲットの位置
    public Transform nextTarget; //次の移動先である視線先ターゲットの位置
    public float speed = 0.3f; //移動速度
    HeadLookController headLook;

    private bool colCheck = false;

    // Use this for initialization
    void Start () {
        //player = GameObject.FindGameObjectWithTag("MainCamera");
        //hand = GameObject.FindGameObjectWithTag("Hand").transform;
    }
	
	// Update is called once per frame
	void Update () {
        PetController colPet = pet.GetComponent<PetController>(); //コントローラのスクリプト取得
        headLook = refObj2.GetComponent<HeadLookController>();

        Transform playerPos = player.transform;
        Transform handPos = hand.transform;

        //視線先設定
        if (colPet.dirStart == 1) //ハンドが離脱して30フレーム経過した
        {

            //次の視線先ターゲットをプレイヤー（カメラ）に設定
            nextObj = player;
            nextTarget = playerPos;

            //現在の視線先ターゲットをFaceComplisionに切り替え
            headLook.lookTarget = transform;

            colPet.dirStart = 0;

        }
        else if (colPet.dirStart == 2 && headLook.lookTarget != handPos) //ペットとハンドが衝突した
        {
            Debug.Log("ターゲットをFaceComplisionに一時移動");

            //次の視線先ターゲットをハンドに設定
            nextObj = hand;
            nextTarget = handPos;

            //現在の視線先ターゲットをFacaComplisionに切り替え
            headLook.lookTarget = transform;

            colPet.dirStart = 0;
        }


        //移動距離
        if (nextTarget == null)
            direction = headLook.lookTarget.position - transform.position;
        else
            direction = nextTarget.position - transform.position;

        //移動
        direction = direction.normalized;
        transform.position = transform.position + (direction * speed * Time.deltaTime);

    }

    //衝突判定(視線先ターゲット切り替え)
    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject == nextObj)
        {
            headLook.lookTarget = nextTarget;
            nextTarget = null;
            nextObj = null;
        }
    }
    void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject == nextObj)
        {

            headLook.lookTarget = nextTarget;
            nextTarget = null;
            nextObj = null;

        }
    }
}
