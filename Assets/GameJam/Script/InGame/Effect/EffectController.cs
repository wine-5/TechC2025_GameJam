using UnityEngine;

namespace Tech.C.Effect
{
    /// <summary>
    /// エフェクトの制御を行うコンポーネント
    /// VFX Prefabにアタッチして使用
    /// </summary>
    public class EffectController : MonoBehaviour
    {
        private float duration;
        private float elapsedTime;
        private bool isPlaying;
        private EffectFactory factory;

        /// <summary>
        /// エフェクトの再生を開始
        /// </summary>
        public void Play(float duration, EffectFactory factory)
        {
            this.duration = duration;
            this.factory = factory;
            elapsedTime = 0f;
            isPlaying = true;
        }

        void Update()
        {
            if (!isPlaying) return;

            elapsedTime += Time.deltaTime;

            if (elapsedTime >= duration)
            {
                Stop();
            }
        }

        /// <summary>
        /// エフェクトを停止してPoolに返却
        /// </summary>
        private void Stop()
        {
            isPlaying = false;
            factory?.ReturnEffect(this);
        }

        /// <summary>
        /// 強制停止
        /// </summary>
        public void ForceStop()
        {
            Stop();
        }
    }
}
