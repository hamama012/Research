using UnityEngine;
using System.Collections;

public class PlayerContoroll : MonoBehaviour {
    public bool call = false; //呼ぶ
    public bool faceDirection = false; //顔の方向

    //VR利用変数
    public SteamVR_TrackedObject trackedObject;
    public SteamVR_Controller.Device device;

    // Use this for initialization
    void Start () {
	}

    //衝突判定
    public void RelayOnCollisionEnter(Collision collision)
    {
        //振動
        if(collision.gameObject.tag == "Pet")
            device.TriggerHapticPulse(200);
    }

    // Update is called once per frame
    void Update () {
        trackedObject = GetComponent<SteamVR_TrackedObject>();
        device = SteamVR_Controller.Input((int)trackedObject.index);

        //トリガーを深く引いた際
        if (device.GetPressDown(SteamVR_Controller.ButtonMask.Trigger))
        {
            call = true;
        }

        //タッチパッドをクリックした
        if (device.GetPressDown(SteamVR_Controller.ButtonMask.Touchpad))
        {
            faceDirection = true;
        }

        // 着席モードで位置トラッキングをリセットする
        if (device.GetPressDown(SteamVR_Controller.ButtonMask.ApplicationMenu))
        {
            SteamVR.instance.hmd.ResetSeatedZeroPose();
        }
    }
}
