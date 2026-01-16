using UnityEngine;
using Tech.C.Interface;
using Tech.C.Audio;
using Tech.C.Effect;

namespace Tech.C.Item
{
    /// <summary>
    /// 娯楽系アイテムの挙動を管理するコントローラー
    /// </summary>
    public class EntertainmentItemController : MonoBehaviour, IFallingItem
    {
        [SerializeField] private EntertainmentType entertainmentType;

        [Header("設定")]
        [SerializeField] private int entertainmentValue = 10;
        
        // fallSpeedはItemManagerから設定されるため、インスペクターには表示しない
        private float fallSpeed = 2f;
        
        [Header("エフェクト")]
        [SerializeField] private GameObject hitEffectPrefab; // ヒット時のエフェクトプレハブ

        private Rigidbody2D rb;

        private ItemPool itemPool;
        private int poolIndex;

        private ItemMover mover;
        private bool isPaused = false;

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

        // 毎フレームの落下・移動処理
        public void Fall()
        {
            rb.linearVelocity = new Vector2(0, -fallSpeed);
        }
        /// <summary>
        /// 外部からタイプをセットする
        /// </summary>
        public void SetType(EntertainmentType type)
        {
            entertainmentType = type;
        }

        // Pool参照をセットするメソッド
        public void SetPool(ItemPool pool, int index)
        {
            itemPool = pool;
            poolIndex = index;
        }

        // 共通返却処理
        private void ReturnToPool()
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
                // パーティクルエフェクトを再生
                PlayHitParticle();
                
                // エフェクト再生
                if (EffectFactory.I != null)
                {
                    EffectFactory.I.PlayEntertainmentEffect();
                }
                
                AudioManager.I.PlaySE(SeType.GetEntertainmentItem);
                GaugeController.I.AddEntertainment(entertainmentValue);
                OnCollected(); // 弾に当たった場合はPoolに返却
            }
        }
        
        /// <summary>
        /// ヒット時のエフェクトを表示
        /// </summary>
        private void PlayHitParticle()
        {
            if (hitEffectPrefab != null)
            {
                // エフェクトの位置をアイテムの位置に設定
                hitEffectPrefab.transform.position = transform.position;
                
                // エフェクトを表示
                hitEffectPrefab.SetActive(true);
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