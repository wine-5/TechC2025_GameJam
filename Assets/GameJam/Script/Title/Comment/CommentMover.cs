using UnityEngine;
using TMPro;
using System;

namespace Tech.C
{
    public class CommentMover : MonoBehaviour
    {
        [Header("移動速度")]
        public float moveSpeed = 2f;
        [Header("加速度")]
        public float acceleration = 1f;
        [Header("中心座標（ターゲット）")]
        public Vector3 targetPosition = Vector3.zero;
        [Header("開始位置（Inspectorで調整可）")]
        public Vector3 startPosition = Vector3.zero;
        [Header("フェードアウト開始までの時間")]
        public float fadeStartTime = 1.0f;
        [Header("フェードアウト時間")]
        public float fadeDuration = 1.0f;

        private TextMeshProUGUI tmp;
        private float elapsed;
        private float currentSpeed;
        private bool isFading;
        private Color baseColor;
        private Action<TextMeshProUGUI> onCompleteCallback;

        public void Initialize(TextMeshProUGUI tmpObj, Vector3 start, Vector3 target, float speed, float accel, float fadeStart, float fadeTime, Action<TextMeshProUGUI> onComplete = null)
        {
            tmp = tmpObj;
            startPosition = start;
            targetPosition = target;
            moveSpeed = speed;
            acceleration = accel;
            fadeStartTime = fadeStart;
            fadeDuration = fadeTime;
            onCompleteCallback = onComplete;
            elapsed = 0f;
            currentSpeed = moveSpeed;
            isFading = false;
            baseColor = tmp.color;
            tmp.transform.localPosition = startPosition;
            tmp.color = baseColor;
            enabled = true;
        }

        private void Update()
        {
            if (tmp == null) return;
            elapsed += Time.deltaTime;

            // 移動
            Vector3 dir = (targetPosition - tmp.transform.localPosition).normalized;
            currentSpeed += acceleration * Time.deltaTime;
            tmp.transform.localPosition += dir * currentSpeed * Time.deltaTime;

            // フェードアウト開始
            if (!isFading && elapsed >= fadeStartTime)
            {
                isFading = true;
            }
            // フェードアウト処理
            if (isFading)
            {
                float fadeProgress = (elapsed - fadeStartTime) / fadeDuration;
                float alpha = Mathf.Lerp(1f, 0f, fadeProgress);
                tmp.color = new Color(baseColor.r, baseColor.g, baseColor.b, alpha);
                if (fadeProgress >= 1f)
                {
                    tmp.gameObject.SetActive(false);
                    enabled = false;
                    onCompleteCallback?.Invoke(tmp);
                }
            }
        }
    }
}
