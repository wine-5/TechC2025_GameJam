using UnityEngine;
using Tech.C.Audio;

namespace Tech.C.Title
{
    /// <summary>
    /// タイトルシーンの初期化を管理
    /// </summary>
    public class TitleSceneManager : MonoBehaviour
    {
        void Start()
        {
            if (AudioManager.I != null)
                AudioManager.I.PlayBGM(BgmType.Title);
        }
    }
}
