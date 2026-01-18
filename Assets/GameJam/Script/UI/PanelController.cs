using UnityEngine;

namespace Tech.C.UI
{
    /// <summary>
    /// パネルの表示/非表示を制御するシンプルなクラス
    /// </summary>
    public class PanelController : MonoBehaviour
    {
        [Header("制御対象パネル")]
        [SerializeField] private GameObject targetPanel;

        private void Start()
        {
            // 初期状態で非表示にする
            if (targetPanel != null)
            {
                targetPanel.SetActive(false);
            }
        }

        /// <summary>
        /// パネルを表示
        /// </summary>
        public void ShowPanel()
        {
            if (targetPanel != null)
            {
                targetPanel.SetActive(true);
            }
        }

        /// <summary>
        /// パネルを非表示
        /// </summary>
        public void HidePanel()
        {
            if (targetPanel != null)
            {
                targetPanel.SetActive(false);
            }
        }

        /// <summary>
        /// パネルの表示/非表示を切り替え
        /// </summary>
        public void TogglePanel()
        {
            if (targetPanel != null)
            {
                targetPanel.SetActive(!targetPanel.activeSelf);
            }
        }

        /// <summary>
        /// パネルが表示中かどうか
        /// </summary>
        public bool IsVisible => targetPanel != null && targetPanel.activeSelf;
    }
}