using UnityEngine;
using UnityEngine.UI;
using System;
using Tech.C;
using Tech.C.Audio;

namespace Tech.C.UI
{
    /// <summary>
    /// GoodEnd・BadEnd画面のUI制御
    /// </summary>
    public class EndSceneUI : MonoBehaviour
    {
        [Header("UI要素")]
        [SerializeField] private Button retryButton;     // リトライボタン
        [SerializeField] private Button titleButton;     // タイトルボタン
        [SerializeField] private Button quitButton;      // 終了ボタン

        private void Start()
        {
            SetupButtons();
        }

        private void SetupButtons()
        {
            // リトライボタンの設定
            if (retryButton != null)
            {
                retryButton.onClick.AddListener(OnRetryButtonClicked);
            }

            // タイトルボタンの設定
            if (titleButton != null)
            {
                titleButton.onClick.AddListener(OnTitleButtonClicked);
            }

            // 終了ボタンの設定
            if (quitButton != null)
            {
                quitButton.onClick.AddListener(OnQuitButtonClicked);
            }
        }

        /// <summary>
        /// リトライボタンがクリックされた時の処理
        /// </summary>
        private void OnRetryButtonClicked()
        {
            // SE再生
            if (AudioManager.I != null)
            {
                AudioManager.I.PlaySE(SeType.ButtonClick);
            }

            // 最後にプレイしたステージに戻る
            string lastSceneName = PlayerPrefs.GetString("lastPlayedScene");
            if (!string.IsNullOrEmpty(lastSceneName))
            {
                if (Enum.TryParse<SceneType>(lastSceneName, out SceneType sceneType))
                {
                    SceneController.I.LoadScene(sceneType);
                }
                else
                {
                    // 万一変換できない場合はStage1に戻る
                    SceneController.I.LoadScene(SceneType.Stage1);
                }
            }
            else
            {
                // 記録がない場合はStage1に戻る
                SceneController.I.LoadScene(SceneType.Stage1);
            }
        }

        /// <summary>
        /// タイトルボタンがクリックされた時の処理
        /// </summary>
        private void OnTitleButtonClicked()
        {
            // SE再生
            if (AudioManager.I != null)
            {
                AudioManager.I.PlaySE(SeType.ButtonClick);
            }

            // タイトル画面に戻る
            SceneController.I.LoadScene(SceneType.Title);
        }

        /// <summary>
        /// 終了ボタンがクリックされた時の処理
        /// </summary>
        private void OnQuitButtonClicked()
        {
            // SE再生
            if (AudioManager.I != null)
            {
                AudioManager.I.PlaySE(SeType.ButtonClick);
            }

            // ゲーム終了
            #if UNITY_EDITOR
                UnityEditor.EditorApplication.isPlaying = false;
            #else
                Application.Quit();
            #endif
        }

        private void OnDestroy()
        {
            // ボタンのイベントを解除してメモリリークを防ぐ
            if (retryButton != null)
                retryButton.onClick.RemoveAllListeners();
            
            if (titleButton != null)
                titleButton.onClick.RemoveAllListeners();
            
            if (quitButton != null)
                quitButton.onClick.RemoveAllListeners();
        }
    }
}