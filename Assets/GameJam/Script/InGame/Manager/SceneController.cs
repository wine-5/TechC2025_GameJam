using UnityEngine;
using UnityEngine.SceneManagement;
using System;
using System.Collections;
using Tech.C.Audio;

namespace Tech.C
{
    public class SceneController : Singleton<SceneController>
    {
        public void LoadScene(string sceneName)
        {
            PlayBGMForScene(sceneName);
            SceneManager.LoadScene(sceneName);
        }
        
        private void PlayBGMForScene(string sceneName)
        {
            switch (sceneName)
            {
                case "Title":
                    AudioManager.I.PlayBGM(BgmType.Title);
                    break;
                case "InGame":
                case "GameScene":
                    AudioManager.I.PlayBGM(BgmType.InGame);
                    break;
                case "GoodEnd":
                    AudioManager.I.PlayBGM(BgmType.GoodEnd);
                    break;
                case "BadEnd":
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
