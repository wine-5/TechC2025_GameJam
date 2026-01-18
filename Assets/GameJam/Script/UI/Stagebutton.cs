using UnityEngine;
using UnityEngine.UI;
using Tech.C;
using Tech.C.Audio;

public class MultiSceneLoadManager : MonoBehaviour
{
    [System.Serializable]
    public class ButtonScenePair
    {
        public Button button;
        public SceneType sceneType;
    }

    [Header("ボタンと遷移先シーンの対応リスト")]
    [SerializeField]
    private ButtonScenePair[] buttonScenePairs;

    [Header("ロード中表示用UI")]
    [SerializeField] private GameObject loadingPanel;  // ロード中パネル
    [SerializeField] private Slider progressBar;       // プログレスバー（任意）

    private void Start()
    {
        // ロード中パネルは最初非表示に
        if (loadingPanel != null)
            loadingPanel.SetActive(false);

        // ボタンクリックイベントを登録
        RegisterButtonEvents();
    }

    private void OnEnable()
    {
        // オブジェクトが再度アクティブになった時にイベントを再登録
        RegisterButtonEvents();
    }

    private void OnDestroy()
    {
        // ボタンのイベントを解除してリークを防ぐ
        UnregisterButtonEvents();
    }

    private void RegisterButtonEvents()
    {
        if (buttonScenePairs == null)
            return;
        
        foreach (var pair in buttonScenePairs)
        {
            if (pair.button != null && pair.sceneType != SceneType.None)
            {
                SceneType targetScene = pair.sceneType;
                
                // 既存のリスナーを削除してから追加（重複防止）
                pair.button.onClick.RemoveAllListeners();
                pair.button.onClick.AddListener(() =>
                {
                    if (AudioManager.I != null)
                    {
                        AudioManager.I.PlaySE(SeType.ButtonClick);
                    }
                    StartCoroutine(LoadSceneAsyncWithUI(targetScene));
                });
            }
        }
    }

    private void UnregisterButtonEvents()
    {
        foreach (var pair in buttonScenePairs)
        {
            if (pair.button != null)
            {
                pair.button.onClick.RemoveAllListeners();
            }
        }
    }

    // 非同期ロード＋UI更新を行うコルーチン
    private System.Collections.IEnumerator LoadSceneAsyncWithUI(SceneType sceneType)
    {
        if (loadingPanel != null)
            loadingPanel.SetActive(true);

        // BGMを再生（シーン読み込み前に）
        PlayBGMForScene(sceneType);

        AsyncOperation asyncLoad = UnityEngine.SceneManagement.SceneManager.LoadSceneAsync(sceneType.ToString());
        asyncLoad.allowSceneActivation = false;

        while (!asyncLoad.isDone)
        {
            // 進捗は0〜0.9の範囲なので正規化
            float progress = Mathf.Clamp01(asyncLoad.progress / 0.9f);

            if (progressBar != null)
                progressBar.value = progress;

            // プログレスバーがほぼ満タンになったら切り替え許可
            if (asyncLoad.progress >= 0.9f)
                asyncLoad.allowSceneActivation = true;

            yield return null;
        }

        if (loadingPanel != null)
            loadingPanel.SetActive(false);
    }

    private void PlayBGMForScene(SceneType sceneType)
    {
        if (AudioManager.I == null)
            return;

        switch (sceneType)
        {
            case SceneType.Title:
                AudioManager.I.PlayBGM(BgmType.Title);
                break;
            case SceneType.Stage1:
            case SceneType.Stage2:
            case SceneType.Stage3:
            case SceneType.Stage4:
                AudioManager.I.PlayBGM(BgmType.InGame);
                break;
            case SceneType.GoodEnd:
                AudioManager.I.PlayBGM(BgmType.GoodEnd);
                break;
            case SceneType.BadEnd:
                AudioManager.I.PlayBGM(BgmType.BadEnd);
                break;
        }
    }
}

