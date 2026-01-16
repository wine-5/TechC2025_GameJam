using UnityEngine;
using UnityEngine.UI;
using Tech.C;
using Tech.C.Audio;

public class MultiSceneLoadManager : MonoBehaviour
{
    [System.Serializable]
    public class ButtonScenePair
    {
        public Button button;          // UIボタン
        public string sceneName;       // このボタンが遷移するシーン名
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

        foreach (var pair in buttonScenePairs)
        {
            if (pair.button != null && !string.IsNullOrEmpty(pair.sceneName))
            {
                string targetScene = pair.sceneName; // クロージャ対策でローカルにコピー

                pair.button.onClick.AddListener(() =>
                {
                    AudioManager.I.PlaySE(SeType.ButtonClick);
                    StartCoroutine(LoadSceneAsyncWithUI(targetScene));
                });
            }
            else
            {
                Debug.LogWarning("ボタンまたはシーン名が未設定のエントリがあります");
            }
        }
    }

    // 非同期ロード＋UI更新を行うコルーチン
    private System.Collections.IEnumerator LoadSceneAsyncWithUI(string sceneName)
    {
        if (loadingPanel != null)
            loadingPanel.SetActive(true);

        AsyncOperation asyncLoad = UnityEngine.SceneManagement.SceneManager.LoadSceneAsync(sceneName);
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
}

