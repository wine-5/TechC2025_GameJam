using System.Collections.Generic;
using UnityEngine;
using Tech.C.Item;
using Tech.C.Player;

namespace Tech.C.Effect
{
    /// <summary>
    /// エフェクトの生成・管理を行うFactory
    /// ObjectPoolを使用してエフェクトを再利用
    /// </summary>
    public class EffectFactory : Singleton<EffectFactory>
    {
        [Header("Effect Data")]
        [SerializeField] private EffectDataSO effectDataSO;

        [Header("Pool Settings")]
        [SerializeField] private int initialPoolSize = 5;

        private Dictionary<ItemType, EffectData> effectDataDictionary;
        private Dictionary<ItemType, ObjectPool<EffectController>> effectPools;

        protected override void Awake()
        {
            base.Awake();
            InitializeEffectData();
            InitializeEffectPools();
        }

        /// <summary>
        /// エフェクトデータを辞書に登録
        /// </summary>
        private void InitializeEffectData()
        {
            effectDataDictionary = new Dictionary<ItemType, EffectData>();

            if (effectDataSO == null)
            {
                Debug.LogError("[EffectFactory] EffectDataSOが設定されていません！");
                return;
            }

            if (effectDataSO.EffectDataList != null)
            {
                foreach (var data in effectDataSO.EffectDataList)
                {
                    if (data != null && data.EffectPrefab != null)
                    {
                        effectDataDictionary[data.ItemType] = data;
                    }
                }
            }
        }

        /// <summary>
        /// 各エフェクトタイプ用のObjectPoolを初期化
        /// </summary>
        private void InitializeEffectPools()
        {
            effectPools = new Dictionary<ItemType, ObjectPool<EffectController>>();

            foreach (var kvp in effectDataDictionary)
            {
                var itemType = kvp.Key;
                var data = kvp.Value;

                var pool = new ObjectPool<EffectController>(
                    () => CreateEffectInstance(data.EffectPrefab),
                    transform,
                    initialPoolSize
                );

                effectPools[itemType] = pool;
            }
        }

        /// <summary>
        /// エフェクトインスタンスを作成
        /// </summary>
        private EffectController CreateEffectInstance(GameObject prefab)
        {
            var instance = Instantiate(prefab);
            var controller = instance.GetComponent<EffectController>();

            if (controller == null)
            {
                controller = instance.AddComponent<EffectController>();
            }

            return controller;
        }

        /// <summary>
        /// エフェクトを再生（ItemTypeで指定）
        /// </summary>
        public void PlayEffect(ItemType itemType)
        {
            if (itemType == ItemType.None) return;

            if (!effectDataDictionary.ContainsKey(itemType))
            {
                Debug.LogWarning($"[EffectFactory] ItemType '{itemType}' のエフェクトが見つかりません");
                return;
            }

            if (!effectPools.ContainsKey(itemType))
            {
                Debug.LogError($"[EffectFactory] ItemType '{itemType}' のPoolが見つかりません");
                return;
            }

            var data = effectDataDictionary[itemType];
            var pool = effectPools[itemType];
            var effect = pool.Get();

            // Playerの位置を取得
            Transform playerTransform = PlayerDataProvider.I?.GetPlayerTransform();
            if (playerTransform == null)
            {
                Debug.LogWarning("[EffectFactory] PlayerTransformが見つかりません");
                effect.transform.position = Vector3.zero;
                effect.transform.SetParent(transform);
            }
            else
            {
                // Playerの子オブジェクトとして配置（追従）
                effect.transform.SetParent(playerTransform);
                effect.transform.localPosition = Vector3.zero;
            }

            // エフェクトを再生
            effect.Play(data.Duration, this);
        }

        /// <summary>
        /// 娯楽アイテムエフェクトを再生
        /// </summary>
        public void PlayEntertainmentEffect()
        {
            PlayEffect(ItemType.Entertainment);
        }

        /// <summary>
        /// ギャンブルアイテムエフェクトを再生
        /// </summary>
        public void PlayGamblingEffect()
        {
            PlayEffect(ItemType.Gambling);
        }

        /// <summary>
        /// エフェクトをPoolに返却
        /// </summary>
        public void ReturnEffect(EffectController effect)
        {
            if (effect == null) return;

            // 親をFactoryに戻す
            effect.transform.SetParent(transform);

            // Poolに返却するために、どのタイプか判定
            foreach (var kvp in effectPools)
            {
                kvp.Value.Return(effect);
                return;
            }
        }
    }
}
