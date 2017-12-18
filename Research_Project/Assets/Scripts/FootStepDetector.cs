using UnityEngine;
using System;
using System.Collections;

/// <summary>
/// 足の接地を感知して音を鳴らす
/// </summary>
public class FootStepDetector : MonoBehaviour
{

    public FootStepSounds footStepSounds;

    private AudioSource aSource;
    // 足音の最短間隔
    private const float PLAY_INTERVAL_MIN_SEC = 0.4f;
    // 最後に足音鳴らした時刻
    private DateTime lastStepTime;

    #region unity

    void Start()
    {
        aSource = gameObject.GetComponent<AudioSource>();
        lastStepTime = DateTime.Now;
    }

    #endregion

    void OnTriggerEnter(Collider col)
    {
        if ((DateTime.Now - lastStepTime).TotalSeconds < PLAY_INTERVAL_MIN_SEC)
        {
            // タップダンス的な足音連打事故を防ぐ
            return;
        }

        FootStepSounds.StepInfo info = footStepSounds.stepInfos.Find((i) => i.groundTag == col.gameObject.tag);
        if (info.HasValue())
        {
            aSource.clip = info.GetClipRandomely();
            aSource.Play();
            lastStepTime = DateTime.Now;
        }
    }
}