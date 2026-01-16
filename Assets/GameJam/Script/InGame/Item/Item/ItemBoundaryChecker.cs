using UnityEngine;
using Tech.C.Interface;

namespace Tech.C.Item
{
    /// <summary>
    /// アイテムが画面外に出た時の検知を行うコンポーネント
    /// </summary>
    public class ItemBoundaryChecker : MonoBehaviour
    {
        [Header("境界設定")]
        [SerializeField] private float bottomBoundary = -10f; // 画面下端の境界Y座標
        [SerializeField] private float sideBoundary = 15f;    // 画面左右の境界X座標

        private IFallingItem fallingItem;

        void Start()
        {
            // IFallingItemインターフェースを取得
            fallingItem = GetComponent<IFallingItem>();

            if (fallingItem == null)
            {
                Debug.LogWarning($"{gameObject.name} にIFallingItemが見つかりません");
            }
        }

        void Update()
        {
            CheckBoundary();
        }

        /// <summary>
        /// アイテムが境界外に出たかをチェック
        /// </summary>
        private void CheckBoundary()
        {
            Vector3 pos = transform.position;

            // 画面外に出た場合の判定
            bool isOutOfBounds = pos.y < bottomBoundary ||
                               pos.x < -sideBoundary ||
                               pos.x > sideBoundary;

            if (isOutOfBounds && fallingItem != null)
            {
                fallingItem.OnFallen();
            }
        }

        /// <summary>
        /// 境界設定を外部から変更
        /// </summary>
        public void SetBoundaries(float bottom, float side)
        {
            bottomBoundary = bottom;
            sideBoundary = side;
        }
    }
}
