using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PetController2 : MonoBehaviour {
    private Animator animator;

    public int colCount = 0; //衝突判定時間カウント
    public int colCount2 = 0; //衝突離脱時間カウント

    public GameObject colHand;

    // Use this for initialization
    void Start () {
        //アニメーション
        animator = GetComponent<Animator>();
    }
	
	// Update is called once per frame
	void Update () {

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
        }

    }

    //衝突判定(尻尾モーション)
    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject == colHand && colCount == 0)
        {
            animator.SetBool("tail", true);
            colCount++;
        }

        colCount2 = 0;
    }

    void OnCollisionExit(Collision collision)
    {
        if (colCount > 0)
            colCount2++;

    }
}
