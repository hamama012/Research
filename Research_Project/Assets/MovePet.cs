using UnityEngine;
using System.Collections;

public class MovePet : MonoBehaviour {
	public float speed = 3.0f; //速度
	private float rotationSmooth = 1.0f;
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
		float sqrDistanceToTarget = Vector3.SqrMagnitude (transform.position - targetPosition);
		if (sqrDistanceToTarget < changeTargetSqrDistance) {
			targetPosition = GetRandomPositionOnLevel ();
		}

		Quaternion targetRotation = Quaternion.LookRotation (targetPosition - transform.position);
		transform.rotation = Quaternion.Slerp (transform.rotation, targetRotation, Time.deltaTime * rotationSmooth);

		animator.SetBool ("walk", true);
		transform.Translate (Vector3.forward * speed * Time.deltaTime);
	
	}

	public Vector3 GetRandomPositionOnLevel()
	{
		float levelSize = 55f;
		return new Vector3 (Random.Range (-levelSize, levelSize), 0, Random.Range (-levelSize, levelSize));
	}
}
