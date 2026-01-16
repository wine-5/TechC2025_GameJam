using UnityEngine;
using Tech.C.Interface;
using Tech.C.Audio;
using Tech.C.Effect;

namespace Tech.C.Item
{
    /// <summary>
    /// ギャンブル系アイテムの挙動を管理するコントローラー
    /// </summary>
    public class GamblingItemController : MonoBehaviour, IFallingItem
    {
        [SerializeField] private GamblingType gamblingType;

        [Header("設定")]
        [SerializeField] private int gambleValueValue = 10;
        
        // fallSpeedはItemManagerから設定されるため、インスペクターには表示しない
        private float fallSpeed = 2f;

        private Rigidbody2D rb;

        // Pool参照を保持
        private ItemPool itemPool;
        private int poolIndex;

        private ItemMoveType moveType;
        private ItemMover mover;
        private bool isPaused = false;
        /// <summary>
        /// 外部からタイプをセットする
        /// </summary>
        public void SetType(GamblingType type)
        {
            gamblingType = type;
        }

        // Pool参照をセットするメソッド
        public void SetPool(ItemPool pool, int index)
        {
            itemPool = pool;
            poolIndex = index;
        }

        void Awake()
        {
            mover = GetComponent<ItemMover>();
        }

        void Start()
        {
            rb = GetComponent<Rigidbody2D>();
        }

        void Update()
        {
            // ポーズ状態をチェック
            CheckPauseState();
            
            // ポーズ中でなければ移動処理
            if (!isPaused)
            {
                mover.MoveItem();
            }
        }
        
        /// <summary>
        /// ポーズ状態をチェックして更新
        /// </summary>
        private void CheckPauseState()
        {
            if (Tech.C.System.PauseManager.I != null)
            {
                bool pauseManagerPaused = Tech.C.System.PauseManager.I.IsPaused;
                
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

        public void Fall()
        {
            if (rb != null)
                rb.linearVelocity = new Vector2(0, -fallSpeed);
        }

        // アイテムをPoolに返却
        public void ReturnToPool()
        {
            if (itemPool != null)
            {
                itemPool.ReturnItem(gameObject, poolIndex);
            }
            else
                Debug.LogError("ItemPoolに返却できません");
        }

        // 落下時の処理
        public void OnFallen()
        {
            ReturnToPool();
        }

        // プレイヤー取得時の処理
        public void OnCollected()
        {
            ReturnToPool();
        }

        // 弾との衝突判定
        void OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag("Bullet"))
            {
                // エフェクト再生
                if (EffectFactory.I != null)
                {
                    EffectFactory.I.PlayGamblingEffect();
                }
                
                AudioManager.I.PlaySE(SeType.GetGamblingItem);
                GaugeController.I.AddGamble(gambleValueValue);
                OnCollected(); // 弾に当たった場合はPoolに返却
            }
        }

        public void SetFallSpeed(float speed)
        {
            fallSpeed = speed;
            // ItemMoverにも速度を設定
            if (mover != null)
            {
                mover.SetFallSpeed(speed);
            }
        }
        
        // ポーズ機能
        public void OnPause()
        {
            isPaused = true;
            // Rigidbody2Dがある場合は速度を停止
            if (rb != null)
            {
                rb.linearVelocity = Vector2.zero;
            }
        }
        
        public void OnResume()
        {
            isPaused = false;
            // ポーズ解除時は特に何もしない（MoveItemで自動的に再開）
        }
        
        public bool IsPaused => isPaused;
    }
}