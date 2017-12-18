using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DirectionManager : MonoBehaviour {
    public GameObject player; //メインカメラ
    public GameObject hand; //仮想ハンド
    public GameObject refObj1; //コントローラ
    public GameObject refObj2; //ペット
    public GameObject nextObj; //次の移動先である視線先ターゲット（衝突判定に使用）

    public Vector3 direction;
    public Vector3 lookPos; //LookHeadで指定している視線先ターゲットの位置
    public Transform nextTarget; //次の移動先である視線先ターゲットの位置
    public float speed = 0.1f; //移動速度
    HeadLookController headLook;

    // Use this for initialization
    void Start () {
        //player = GameObject.FindGameObjectWithTag("MainCamera");
        //hand = GameObject.FindGameObjectWithTag("Hand").transform;
    }
	
	// Update is called once per frame
	void Update () {
        PlayerContoroll trackedObject = refObj1.GetComponent<PlayerContoroll>(); //コントローラのスクリプト取得
        headLook = refObj2.GetComponent<HeadLookController>();

        Transform playerPos = player.transform;
        Transform handPos = hand.transform;

        //タッチパッドを触ったとき
        if (trackedObject.faceDirection == true) {

            //次の視線先ターゲットをセット
            if (headLook.lookTarget == handPos)
            {
                nextObj = player;
                nextTarget = playerPos;
                Debug.Log(nextTarget);
            }
            else
            {
                Debug.Log(headLook.lookTarget);
                nextObj = hand;
                nextTarget = handPos;
            }

            headLook.lookTarget = transform; //現在の視線先ターゲットを自分に切り替え
            trackedObject.faceDirection = false;

        }
        if(nextTarget == null)
            direction = headLook.lookTarget.position - transform.position;
        else
            direction = nextTarget.position - transform.position;

        //移動（遅すぎるので何とかしないといけない）
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
