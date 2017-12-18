using UnityEngine;
using System;
using System.Collections.Generic;

/// <summary>
/// 足音音源を管理
/// </summary>
public class FootStepSounds : MonoBehaviour
{
    /// <summary>
    /// 地面と音の対応情報
    /// </summary>
    [Serializable]
    public struct StepInfo
    {
        // 地面タグ
        public string groundTag;
        // 地面に対応した音
        public List<AudioClip> clips;

        public bool HasValue()
        {
            return !string.IsNullOrEmpty(groundTag) && clips != null && clips.Count > 0;
        }

        public AudioClip GetClipRandomely()
        {
            return clips[UnityEngine.Random.Range(0, clips.Count)];
        }
    }

    public List<StepInfo> stepInfos;
}