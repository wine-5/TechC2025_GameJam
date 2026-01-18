using UnityEngine;
using UnityEngine.UI;
using Tech.C.Audio;

namespace Tech.C.UI.Audio
{
    /// <summary>
    /// 音量スライダーとAudioManagerを連携させるコントローラー
    /// </summary>
    public class VolumeSliderController : MonoBehaviour
    {
        [Header("音量タイプ")]
        [SerializeField] private VolumeType volumeType = VolumeType.Master;

        [Header("UIコンポーネント")]
        [SerializeField] private Slider volumeSlider;

        private void Start()
        {
            // スライダーの参照を自動取得
            if (volumeSlider == null)
            {
                volumeSlider = GetComponent<Slider>();
            }

            if (volumeSlider == null)
            {
                Debug.LogError($"[VolumeSliderController] Sliderコンポーネントが見つかりません: {gameObject.name}");
                return;
            }

            // 初期化
            InitializeSlider();
            
            // スライダーの値変更イベントを登録
            volumeSlider.onValueChanged.AddListener(OnVolumeChanged);
        }

        private void OnDestroy()
        {
            // イベントの解除
            if (volumeSlider != null)
            {
                volumeSlider.onValueChanged.RemoveListener(OnVolumeChanged);
            }
        }

        /// <summary>
        /// スライダーの初期設定
        /// </summary>
        private void InitializeSlider()
        {
            // スライダーの範囲設定
            volumeSlider.minValue = 0f;
            volumeSlider.maxValue = 1f;

            // AudioManagerから現在の音量値を取得してスライダーに反映
            if (AudioManager.I != null)
            {
                float currentVolume = GetCurrentVolume();
                volumeSlider.value = currentVolume;
            }
        }

        /// <summary>
        /// 現在の音量値を取得
        /// </summary>
        private float GetCurrentVolume()
        {
            if (AudioManager.I == null) return 1f;

            switch (volumeType)
            {
                case VolumeType.Master:
                    return AudioManager.I.MasterVolume;
                case VolumeType.BGM:
                    return AudioManager.I.BGMVolume;
                case VolumeType.SE:
                    return AudioManager.I.SEVolume;
                default:
                    return 1f;
            }
        }

        /// <summary>
        /// スライダーの値が変更されたときに呼ばれる
        /// </summary>
        private void OnVolumeChanged(float value)
        {
            if (AudioManager.I == null) return;

            // 音量タイプに応じてAudioManagerの対応するメソッドを呼び出し
            switch (volumeType)
            {
                case VolumeType.Master:
                    AudioManager.I.SetMasterVolume(value);
                    break;
                case VolumeType.BGM:
                    AudioManager.I.SetBGMVolume(value);
                    break;
                case VolumeType.SE:
                    AudioManager.I.SetSEVolume(value);
                    break;
            }

            // デバッグログ（必要に応じてコメントアウト）
            // Debug.Log($"[VolumeSliderController] {volumeType} Volume set to: {value:F2}");
        }

        /// <summary>
        /// 外部から音量タイプを設定
        /// </summary>
        public void SetVolumeType(VolumeType type)
        {
            volumeType = type;
            
            // 音量タイプが変更されたらスライダーの値も更新
            if (volumeSlider != null && AudioManager.I != null)
            {
                float currentVolume = GetCurrentVolume();
                volumeSlider.value = currentVolume;
            }
        }

        /// <summary>
        /// スライダーの値を手動で更新（AudioManagerの音量に合わせる）
        /// </summary>
        public void UpdateSliderValue()
        {
            if (volumeSlider != null && AudioManager.I != null)
            {
                float currentVolume = GetCurrentVolume();
                volumeSlider.value = currentVolume;
            }
        }

        /// <summary>
        /// 音量を0に設定（ミュート）
        /// </summary>
        public void MuteVolume()
        {
            if (volumeSlider != null)
            {
                volumeSlider.value = 0f;
            }
        }

        /// <summary>
        /// 音量を最大に設定
        /// </summary>
        public void MaxVolume()
        {
            if (volumeSlider != null)
            {
                volumeSlider.value = 1f;
            }
        }
    }

    /// <summary>
    /// 音量の種類を定義する列挙型
    /// </summary>
    public enum VolumeType
    {
        Master,  // マスター音量
        BGM,     // BGM音量
        SE       // SE音量
    }
}