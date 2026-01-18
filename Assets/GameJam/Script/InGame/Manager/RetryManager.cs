using UnityEngine;
using UnityEngine.SceneManagement;
using System;

namespace Tech.C
{
    public class RetryManager : MonoBehaviour
    {       
        public void RestartGame()
        {
            string lastSceneName = PlayerPrefs.GetString("lastPlayedScene");
            // 値が空でなければ、そのシーンをロードする
            if (!string.IsNullOrEmpty(lastSceneName))
            {
                // SceneControllerを使用してBGMも正しく変更されるようにする
                if (Enum.TryParse<SceneType>(lastSceneName, out SceneType sceneType))
                {
                    SceneController.I.LoadScene(sceneType);
                }
                else
                {
                    // 万一SceneTypeに変換できない場合は直接ロード
                    SceneManager.LoadScene(lastSceneName);
                }
            }
            else
            {
                SceneController.I.LoadScene(SceneType.Title);
            }
        }
    }
}
