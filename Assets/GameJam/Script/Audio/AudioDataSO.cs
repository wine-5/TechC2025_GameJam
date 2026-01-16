using System;
using UnityEngine;

namespace Tech.C.Audio
{
    /// <summary>
    /// SE用のオーディオデータ
    /// </summary>
    [Serializable]
    public class SeAudioData
    {
        [SerializeField] private SeType seType;
        [SerializeField] private AudioClip audioClip;
        [SerializeField, Range(0f, 2f)] private float volumeMultiplier = 1.0f;

        public SeType SeType => seType;
        public AudioClip AudioClip => audioClip;
        public float VolumeMultiplier => volumeMultiplier;
    }

    /// <summary>
    /// BGM用のオーディオデータ
    /// </summary>
    [Serializable]
    public class BgmAudioData
    {
        [SerializeField] private BgmType bgmType;
        [SerializeField] private AudioClip audioClip;
        [SerializeField, Range(0f, 2f)] private float volumeMultiplier = 1.0f;
        [SerializeField] private bool loop = true;
        
        [Header("フェード設定")]
        [SerializeField] private bool useFadeIn = false;
        [SerializeField] private float fadeInDuration = 2f;
        [SerializeField] private bool useFadeOut = false;
        [SerializeField] private float fadeOutDuration = 2f;

        public BgmType BgmType => bgmType;
        public AudioClip AudioClip => audioClip;
        public float VolumeMultiplier => volumeMultiplier;
        public bool Loop => loop;
        public bool UseFadeIn => useFadeIn;
        public float FadeInDuration => fadeInDuration;
        public bool UseFadeOut => useFadeOut;
        public float FadeOutDuration => fadeOutDuration;
    }

    /// <summary>
    /// オーディオデータを管理するScriptableObject
    /// </summary>
    [CreateAssetMenu(fileName = "AudioData", menuName = "Tech.C/AudioData")]
    public class AudioDataSO : ScriptableObject
    {
        [Header("SE List")]
        [SerializeField] private SeAudioData[] seAudioDataList;
        
        [Header("BGM List")]
        [SerializeField] private BgmAudioData[] bgmAudioDataList;

        public SeAudioData[] SeAudioDataList => seAudioDataList;
        public BgmAudioData[] BgmAudioDataList => bgmAudioDataList;
    }
}
