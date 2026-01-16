using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace Tech.C.UI
{
    /// <summary>
    /// ポーズ画面のUIを管理するクラス
    /// </summary>
    public class PauseUI : MonoBehaviour
    {
        [Header("UI要素")]
        [SerializeField] private GameObject pausePanel; // ポーズ画面全体のパネル
        [SerializeField] private TextMeshProUGUI pauseText; // "PAUSED"テキスト
        [SerializeField] private Button resumeButton; // 再開ボタン（戻る）
        [SerializeField] private Button retryButton; // リトライボタン
        [SerializeField] private Button titleButton; // タイトルボタン
        
        [Header("設定")]
        [SerializeField] private bool showButtons = true; // ボタンを表示するか
        [SerializeField] private bool enableHoverEffect = true; // ホバー効果を有効にするか
        
        private void Start()
        {
            // 初期化は PauseManager に任せる（パネルの表示/非表示はPauseManagerが制御）
            
            // ボタンのイベント設定
            SetupButtons();
            
            // ホバー効果を追加
            if (enableHoverEffect)
            {
                AddHoverEffects();
            }
        }
        
        /// <summary>
        /// ボタンのイベントを設定
        /// </summary>
        private void SetupButtons()
        {
            if (resumeButton != null)
            {
                resumeButton.onClick.AddListener(OnResumeButtonClicked);
                resumeButton.gameObject.SetActive(showButtons);
            }
            
            if (retryButton != null)
            {
                retryButton.onClick.AddListener(OnRetryButtonClicked);
                retryButton.gameObject.SetActive(showButtons);
            }
            
            if (titleButton != null)
            {
                titleButton.onClick.AddListener(OnTitleButtonClicked);
                titleButton.gameObject.SetActive(showButtons);
            }
        }
        
        /// <summary>
        /// ポーズUIを表示
        /// </summary>
        public void ShowPauseUI()
        {
            if (pausePanel != null)
            {
                pausePanel.SetActive(true);
            }
        }
        
        /// <summary>
        /// ポーズUIを非表示
        /// </summary>
        public void HidePauseUI()
        {
            if (pausePanel != null)
            {
                pausePanel.SetActive(false);
            }
        }
        
        /// <summary>
        /// 再開ボタン（戻る）がクリックされた時の処理
        /// </summary>
        private void OnResumeButtonClicked()
        {
            if (Tech.C.System.PauseManager.I != null)
            {
                Tech.C.System.PauseManager.I.Resume();
            }
        }
        
        /// <summary>
        /// リトライボタンがクリックされた時の処理
        /// </summary>
        private void OnRetryButtonClicked()
        {
            // ポーズを解除してから現在のシーンをリロード
            if (Tech.C.System.PauseManager.I != null)
            {
                Tech.C.System.PauseManager.I.Resume();
            }
            
            // RetryManagerを使用してゲームをリスタート
            RetryManager retryManager = FindFirstObjectByType<RetryManager>();
            if (retryManager != null)
            {
                retryManager.RestartGame();
            }
            else
            {
                // RetryManagerが見つからない場合は、SceneControllerでリトライ
                if (SceneController.I != null)
                {
                    SceneController.I.RestartCurrentScene();
                }
            }
        }
        
        /// <summary>
        /// タイトルボタンがクリックされた時の処理
        /// </summary>
        private void OnTitleButtonClicked()
        {
            // ポーズを解除してからタイトルシーンに遷移
            if (Tech.C.System.PauseManager.I != null)
            {
                Tech.C.System.PauseManager.I.Resume();
            }
            
            // タイトルシーンに遷移
            if (SceneController.I != null)
            {
                SceneController.I.LoadScene(SceneType.Title);
            }
        }
        
        /// <summary>
        /// ボタンにホバー効果を追加
        /// </summary>
        private void AddHoverEffects()
        {
            // 各ボタンにホバー効果コンポーネントを追加
            AddHoverEffectToButton(resumeButton);
            AddHoverEffectToButton(retryButton);
            AddHoverEffectToButton(titleButton);
        }
        
        /// <summary>
        /// 指定されたボタンにホバー効果を追加
        /// </summary>
        private void AddHoverEffectToButton(Button button)
        {
            if (button == null) return;
            
            // 既にホバー効果が付いていないかチェック
            if (button.GetComponent<ButtonHoverEffect>() == null)
            {
                button.gameObject.AddComponent<ButtonHoverEffect>();
            }
        }
    }
}
