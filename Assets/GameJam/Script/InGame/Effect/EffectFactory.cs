using System.Collections.Generic;
using UnityEngine;
using Tech.C.Item;
using Tech.C.Player;
using Tech.C.Pooling;

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

        [Header("Pool Reference")]
        [SerializeField] private ObjectPool effectPool;

        private Dictionary<ItemType, EffectData> effectDataDictionary;

        protected override void Awake()
        {
            base.Awake();
            InitializeEffectData();
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
        /// エフェクトを再生（ItemTypeで指定）
        /// </summary>
        public void PlayEffect(ItemType itemType)
        {
            if (itemType == ItemType.None) return;

            if (effectPool == null)
            {
                Debug.LogError("[EffectFactory] ObjectPoolが設定されていません！");
                return;
            }

            if (!effectDataDictionary.ContainsKey(itemType))
            {
                Debug.LogWarning($"[EffectFactory] ItemType '{itemType}' のエフェクトが見つかりません");
                return;
            }

            var data = effectDataDictionary[itemType];
            
            // ObjectPoolからエフェクトを取得
            GameObject effectObj = effectPool.GetObject(data.EffectPrefab);
            if (effectObj == null)
            {
                Debug.LogError($"[EffectFactory] エフェクトの取得に失敗しました: {data.EffectPrefab.name}");
                return;
            }

            var effect = effectObj.GetComponent<EffectController>();
            if (effect == null)
            {
                Debug.LogError($"[EffectFactory] EffectControllerが見つかりません: {effectObj.name}");
                effectPool.ReturnObject(effectObj);
                return;
            }

            // Playerの位置を取得
            Transform playerTransform = PlayerDataProvider.I?.GetPlayerTransform();
            if (playerTransform == null)
            {
                Debug.LogWarning("[EffectFactory] PlayerTransformが見つかりません");
                effectObj.transform.position = Vector3.zero;
            }
            else
            {
                // Playerの子オブジェクトとして配置（追従）
                effectObj.transform.SetParent(playerTransform);
                effectObj.transform.localPosition = Vector3.zero;
                effectObj.transform.localRotation = Quaternion.identity;
                effectObj.transform.localScale = Vector3.one;
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

            if (effectPool == null)
            {
                Debug.LogError("[EffectFactory] ObjectPoolが設定されていません！");
                Destroy(effect.gameObject);
                return;
            }

            effectPool.ReturnObject(effect.gameObject);
        }
    }
}
