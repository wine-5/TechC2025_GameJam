using UnityEngine;
using UnityEngine.InputSystem;
using Tech.C.Bullet;
using Tech.C.Player;

namespace Tech.C.Player
{
    /// <summary>
    /// 入力を受けて、プレイヤーを移動させる処理
    /// </summary>
    public class PlayerController : MonoBehaviour
    {
        [SerializeField] private float moveSpeed = 5f;
        [SerializeField] private BulletType bulletType = BulletType.Normal;
        private Vector2 moveInput;
        private Rigidbody2D rb;
        private PlayerAnimationController animCtrl;
        private float lastMoveX = 0f;
        private bool isPaused = false;

        private void Start()
        {
            rb = GetComponent<Rigidbody2D>();
            animCtrl = GetComponent<PlayerAnimationController>();
            
            // PlayerDataProviderに登録
            if (PlayerDataProvider.I != null)
            {
                PlayerDataProvider.I.RegisterPlayerTransform(transform);
            }
        }

        private void Update()
        {
            // PlayerDataProviderに位置を更新
            if (PlayerDataProvider.I != null)
            {
                PlayerDataProvider.I.UpdatePosition(transform.position);
            }
            
            // PauseManagerの状態をチェック
            if (Tech.C.System.PauseManager.I != null)
            {
                bool pauseManagerPaused = Tech.C.System.PauseManager.I.IsPaused;

                // ポーズ状態が変わった場合
                if (isPaused != pauseManagerPaused)
                {
                    if (pauseManagerPaused)
                    {
                        OnPause();
                    }
                    else
                    {
                        OnResume();
                    }
                    isPaused = pauseManagerPaused;
                }
            }
        }

        /// <summary>
        /// 入力を取得して移動の方向を決める
        /// </summary>
        /// <param name="context"></param>
        public void OnMove(InputAction.CallbackContext context)
        {
            // ポーズ中は入力を無視
            if (isPaused) return;
            
            // 横方向のみ取得
            float x = context.ReadValue<Vector2>().x;
            moveInput = new Vector2(x, 0);
        }

        public void OnFire(InputAction.CallbackContext context)
        {
            // ポーズ中は入力を無視
            if (isPaused) return;
            
            if (context.performed)
            {
                // Playerの少し上から弾を生成することで、Playerのコライダーとの干渉を防ぐ
                Vector3 bulletSpawnPosition = transform.position + Vector3.up * 0.5f;
                BulletManager.I.SpawnBullet(bulletType, bulletSpawnPosition);
            }
        }

        void FixedUpdate()
        {
            // ポーズ中は移動処理を停止
            if (!isPaused)
            {
                MovePlayer();
            }
        }

        /// <summary>
        /// プレイヤーの移動を行う
        /// </summary>
        private void MovePlayer()
        {
            Vector2 targetVelocity = new Vector2(moveInput.x * moveSpeed, rb.linearVelocity.y);
            rb.linearVelocity = targetVelocity;
            SetAnimationByDirection();
        }

        /// <summary>
        /// 移動方向に応じてアニメーションを切り替える
        /// </summary>
        private void SetAnimationByDirection()
        {
            if (animCtrl == null) Debug.LogError("Animator == null");

            if (moveInput.x == 0 && lastMoveX == 0)
            {
                animCtrl.SetIdle();
            }
            else if (moveInput.x > 0)
            {
                animCtrl.SetMoveRight();
            }
            else if (moveInput.x < 0)
            {
                animCtrl.SetMoveLeft();
            }

            lastMoveX = moveInput.x;
        }

        // IPausableインターフェースの実装
        public void OnPause()
        {
            isPaused = true;
            // ポーズ時は移動を停止
            if (rb != null)
            {
                rb.linearVelocity = Vector2.zero;
            }
            moveInput = Vector2.zero;
        }

        public void OnResume()
        {
            isPaused = false;
        }

        public bool IsPaused => isPaused;
    }
}
