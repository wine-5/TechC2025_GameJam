using UnityEngine;

namespace Tech.C.Player
{
    /// <summary>
    /// Playerの情報を他のクラスに提供するデータプロバイダー
    /// PlayerControllerとEffectFactory等の間の疎結合を実現
    /// </summary>
    public class PlayerDataProvider : Singleton<PlayerDataProvider>
    {
        private Vector3 playerPosition;
        private Transform playerTransform;

        /// <summary>
        /// Playerの位置を更新（PlayerControllerから呼び出される）
        /// </summary>
        public void UpdatePosition(Vector3 position) => playerPosition = position;

        /// <summary>
        /// PlayerのTransformを登録（PlayerControllerから呼び出される）
        /// </summary>
        public void RegisterPlayerTransform(Transform transform) => playerTransform = transform;

        /// <summary>
        /// Playerの現在位置を取得
        /// </summary>
        public Vector3 GetPosition() => playerPosition;

        /// <summary>
        /// PlayerのTransformを取得
        /// </summary>
        public Transform GetPlayerTransform() => playerTransform;
    }
}
