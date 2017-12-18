using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandControll : MonoBehaviour {

    PlayerContoroll colliderParent;

    // Use this for initialization
    void Start()
    {
        GameObject objColliderParent = gameObject.transform.parent.gameObject;
        colliderParent = objColliderParent.GetComponent<PlayerContoroll>();
    }

    void OnCollisionEnter(Collision collision)
    {
        colliderParent.RelayOnCollisionEnter(collision);
    }

    // Update is called once per frame
    void Update () {
	}
}
