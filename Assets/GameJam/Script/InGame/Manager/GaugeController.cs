using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Tech.C.Audio;

namespace Tech.C
{
    /// <summary>
    /// 各スライダーの管理と、UIの更新を行う
    /// </summary>
    public class GaugeController : Singleton<GaugeController>
    {

        [SerializeField] private Slider gambleSlider;
        [SerializeField] private Slider entertainmentSlider;

        private int gambleValue = 0;
        private int entertainmentValue = 0;

        private const int MAX_GAMBLE = 100;
        private const int MAX_ENTERTAINMENT = 100;
        protected override bool UseDontDestroyOnLoad => true;

        /// <summary>
        /// 外部からSlider参照を再設定するためのメソッド
        /// 各シーンのスクリプトから呼び出す
        /// </summary>
        public void SetSliders(Slider gambleSlider, Slider entertainmentSlider)
        {
            this.gambleSlider = gambleSlider;
            this.entertainmentSlider = entertainmentSlider;
            
            // 設定後に初期化を実行
            InitializeSliders();
        }

        private void InitializeSliders()
        {
            if (gambleSlider != null)
            {
                gambleSlider.maxValue = MAX_GAMBLE;
                gambleSlider.value = gambleValue; // 現在の値を反映
                gambleSlider.interactable = false; // マウス操作を無効化
            }
            
            if (entertainmentSlider != null)
            {
                entertainmentSlider.maxValue = MAX_ENTERTAINMENT;
                entertainmentSlider.value = entertainmentValue; // 現在の値を反映
                entertainmentSlider.interactable = false; // マウス操作を無効化
            }
        }

        private void Start()
        {
            // 初期化時にSliderの最大値と初期値を設定
            if (gambleSlider != null)
            {
                gambleSlider.maxValue = MAX_GAMBLE;
                gambleSlider.value = 0;
                gambleSlider.interactable = false; // マウス操作を無効化
            }
            
            if (entertainmentSlider != null)
            {
                entertainmentSlider.maxValue = MAX_ENTERTAINMENT;
                entertainmentSlider.value = 0;
                entertainmentSlider.interactable = false; // マウス操作を無効化
            }
        }

        private void UpdateGambleUI()
        {
            if (gambleSlider != null)
            {
                gambleSlider.value = gambleValue;
            }
        }

        private void UpdateEntertainmentUI()
        {
            if (entertainmentSlider != null)
            {
                entertainmentSlider.value = entertainmentValue;
            }
        }

        public void AddGamble(int amount)
        {
            gambleValue = Mathf.Clamp(gambleValue + amount, 0, MAX_GAMBLE);
            UpdateGambleUI(); // Sliderを即座に更新
            
            if (gambleValue >= MAX_GAMBLE)
            {
                AudioManager.I.PlaySE(SeType.GamblingGaugeFull);
                PlayerPrefs.SetString("lastPlayedScene", SceneManager.GetActiveScene().name);
                SceneController.I.LoadScene(SceneType.BadEnd);
            }
        }

        public void AddEntertainment(int amount)
        {
            entertainmentValue = Mathf.Clamp(entertainmentValue + amount, 0, MAX_ENTERTAINMENT);
            UpdateEntertainmentUI(); // Sliderを即座に更新
            
            if (entertainmentValue >= MAX_ENTERTAINMENT)
            {
                AudioManager.I.PlaySE(SeType.EntertainmentGaugeFull);
                PlayerPrefs.SetString("lastPlayedScene", SceneManager.GetActiveScene().name);
                SceneController.I.LoadScene(SceneType.GoodEnd);
            }
        }

    }
}
