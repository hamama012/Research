using UnityEngine;
using System.Collections;

public class MovePet : MonoBehaviour {
	public float speed = 3.0f; //速度
    private float rotationSmooth = 0.1f;
	private float changeTargetSqrDistance = 10.0f; //目標位置を切り替える距離

	public bool  wander = true; //徘徊行動判定
	private Vector3 targetPosition;
	private Animator animator;

	// Use this for initialization
	void Start () {
		targetPosition = GetRandomPositionOnLevel ();

		//アニメーション
		animator = GetComponent<Animator>();
	}

	// Update is called once per frame
	void Update () {

        //目標位置
		float sqrDistanceToTarget = Vector3.SqrMagnitude (transform.position - targetPosition);
		if (sqrDistanceToTarget < changeTargetSqrDistance) {
			targetPosition = GetRandomPositionOnLevel ();
		}

        //回転実行
		Quaternion targetRotation = Quaternion.LookRotation (targetPosition - transform.position);
		transform.rotation = Quaternion.Slerp (transform.rotation, targetRotation, Time.deltaTime * rotationSmooth);

        //アニメーション
		animator.SetBool ("walk", true);

        //移動実行
		transform.Translate (Vector3.forward * speed * Time.deltaTime);
	
	}

    //目標位置設定
	public Vector3 GetRandomPositionOnLevel()
	{
		float levelSize = 55f;
		return new Vector3 (Random.Range (-levelSize, levelSize), 0, Random.Range (-levelSize, levelSize));
	}
}
