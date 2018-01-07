using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerContoroll : MonoBehaviour
{
    public bool stop = false; //呼んだとき一時停止
    public bool call = false; //呼ぶ
    public bool release = true; //自由状態

    public bool faceDirection = false; //顔の方向
    public AudioSource sound;

    //VR利用変数
    public SteamVR_TrackedObject trackedObject;
    public SteamVR_Controller.Device device;

    // Use this for initialization
    void Start()
    {
        sound = GetComponent<AudioSource>();
    }

    //衝突判定
    public void RelayOnCollisionEnter(Collision collision)
    {
        //振動
        if (collision.gameObject.tag == "Pet")
            device.TriggerHapticPulse(200);
    }

    // Update is called once per frame
    void Update()
    {
        trackedObject = GetComponent<SteamVR_TrackedObject>();
        device = SteamVR_Controller.Input((int)trackedObject.index);

        //トリガーを深く引いた際
        if (device.GetPressDown(SteamVR_Controller.ButtonMask.Trigger))
        {
            if(release == true)
            {
                sound.PlayOneShot(sound.clip);
                release = false;

                StartCoroutine(Checking(() => {
                }));
            }
            else if(release == false)
            {
                release = true;
            }
        }


        // 着席モードで位置トラッキングをリセットする
        if (device.GetPressDown(SteamVR_Controller.ButtonMask.ApplicationMenu))
        {
            SteamVR.instance.hmd.ResetSeatedZeroPose();
        }
    }

    public delegate void functionType();
    private IEnumerator Checking(functionType callback)
    {
        while (true)
        {
            yield return new WaitForFixedUpdate();
            if (!sound.isPlaying)
            {
                call = true;
                break;
            }
        }
    }
}
