using UnityEngine;
using UnityEngine.SceneManagement;
using System;
using System.Collections;
using Tech.C.Audio;

namespace Tech.C
{
    public class SceneController : Singleton<SceneController>
    {
        public void LoadScene(SceneType sceneType)
        {
            PlayBGMForScene(sceneType);
            SceneManager.LoadScene(sceneType.ToString());
        }
        
        // Button OnClickイベント用の個別メソッド
        public void LoadTitle() => LoadScene(SceneType.Title);
        public void LoadStage1() => LoadScene(SceneType.Stage1);
        public void LoadStage2() => LoadScene(SceneType.Stage2);
        public void LoadStage3() => LoadScene(SceneType.Stage3);
        public void LoadStage4() => LoadScene(SceneType.Stage4);
        public void LoadGoodEnd() => LoadScene(SceneType.GoodEnd);
        public void LoadBadEnd() => LoadScene(SceneType.BadEnd);
        
        public void LoadScene(string sceneName)
        {
            if (Enum.TryParse<SceneType>(sceneName, out SceneType sceneType))
            {
                LoadScene(sceneType);
            }
            else
            {
                Debug.LogWarning($"Unknown scene name: {sceneName}");
                SceneManager.LoadScene(sceneName);
            }
        }
        
        private void PlayBGMForScene(SceneType sceneType)
        {
            if (AudioManager.I == null) return;
            
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

        public void LoadScene(int sceneIndex)
        {
            SceneManager.LoadScene(sceneIndex);
        }

        /// <summary>
        /// 現在のシーンをリロード（リトライ機能）
        /// </summary>
        public void RestartCurrentScene()
        {
            string currentSceneName = SceneManager.GetActiveScene().name;
            SceneManager.LoadScene(currentSceneName);
        }

        public void LoadSceneAsync(string sceneName)
        {
            StartCoroutine(LoadSceneAsyncCoroutine(sceneName));
        }

        private IEnumerator LoadSceneAsyncCoroutine(string sceneName)
        {
            AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneName);
            while (!asyncLoad.isDone)
            {
                yield return null; // 進捗UI表示などに使えます
            }
        }
    }
}
