using UnityEngine;

namespace Tech.C.Effect
{
    /// <summary>
    /// エフェクトデータを管理するScriptableObject
    /// </summary>
    [CreateAssetMenu(fileName = "EffectData", menuName = "Tech.C/EffectData")]
    public class EffectDataSO : ScriptableObject
    {
        [Header("Effect List")]
        [SerializeField] private EffectData[] effectDataList;

        public EffectData[] EffectDataList => effectDataList;
    }
}
