using UnityEngine;
using Tech.C.Audio;

namespace Tech.C
{
    /// <summary>
    /// ButtonのOnClickからシーンを変更するためのラッパークラス
    /// </summary>
    public class SceneChangeButtonHandler : MonoBehaviour
    {
        [SerializeField]
        private SceneType targetScene = SceneType.Title;

        /// <summary>
        /// ButtonのOnClickから呼び出されるメソッド
        /// 指定されたシーンに切り替える
        /// </summary>
        public void ChangeScene()
        {
            AudioManager.I.PlaySE(SeType.ButtonClick);
            
            if (SceneController.I != null)
            {
                SceneController.I.LoadScene(targetScene);
            }
            else
            {
                Debug.LogWarning("SceneController instance not found!");
            }
        }
    }
}