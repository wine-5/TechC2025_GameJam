using System;
using UnityEngine;
using Tech.C.Item;

namespace Tech.C.Effect
{
    /// <summary>
    /// エフェクトデータ
    /// </summary>
    [Serializable]
    public class EffectData
    {
        [SerializeField] private ItemType itemType;
        [SerializeField] private GameObject effectPrefab;
        [SerializeField] private float duration = 1.0f;

        public ItemType ItemType => itemType;
        public GameObject EffectPrefab => effectPrefab;
        public float Duration => duration;
    }
}
