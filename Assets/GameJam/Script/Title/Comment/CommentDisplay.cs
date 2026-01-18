using UnityEngine;
using TMPro;

namespace Tech.C
{
    public class CommentDisplay : MonoBehaviour
    {
        [Header("コメント抽選クラス（Serializable）")]
        [SerializeField] private CommentRandomSelector randomSelector;
        [Header("コメント移動クラス（Prefabにアタッチ）")]
        [SerializeField] private CommentMover commentMoverPrefab;
        [Header("表示するTMPオブジェクトの親")] 
        [SerializeField] private Transform displayParent;
        [Header("コメントPool")]
        [SerializeField] private CommentPool commentPool;
        [Header("表示間隔（秒）")]
        [SerializeField] private float displayInterval = 2f;

        // コメント開始位置の設定（16方向）
        private const int START_POS_COUNT = 16;
        private const float RADIUS = 450f; // Canvas座標系で調整
        private Vector3[] startPositions;
        private static readonly Vector3 CENTER = Vector3.zero;

        private float timer;

        private void Awake()
        {
            // 16方向の開始位置を円周上に生成
            startPositions = new Vector3[START_POS_COUNT];
            float angleStep = 360f / START_POS_COUNT;
            for (int i = 0; i < START_POS_COUNT; i++)
            {
                float rad = Mathf.Deg2Rad * (angleStep * i);
                float x = Mathf.Cos(rad) * RADIUS;
                float y = Mathf.Sin(rad) * RADIUS;
                startPositions[i] = new Vector3(x, y, 0f);
            }
        }

        private void Update()
        {
            timer += Time.deltaTime;
            if (timer >= displayInterval)
            {
                DisplayRandomComment();
                timer = 0f;
            }
        }

        /// <summary>
        /// ランダムコメントを生成・表示
        /// </summary>
        private const float INIT_FONT_SIZE = 26f;
        private static readonly Color INIT_COLOR = Color.red;
        private const float INIT_SPEED = 100f;
        private const float INIT_ACCEL = 50f;
        private const float INIT_FADE_START = 1.0f;
        private const float INIT_FADE_DURATION = 1.0f;

        private void DisplayRandomComment()
        {
            string comment = randomSelector != null ? randomSelector.GetRandomComment() : "";
            if (string.IsNullOrEmpty(comment)) return;

            // PoolからTMP取得
            var tmp = commentPool.GetComment();
            tmp.transform.SetParent(displayParent, false);
            tmp.text = comment;
            tmp.fontSize = INIT_FONT_SIZE;
            tmp.color = INIT_COLOR;
            tmp.alignment = TextAlignmentOptions.Center;
            tmp.gameObject.SetActive(true);

            // 16方向からランダムで開始位置を選択
            int idx = Random.Range(0, START_POS_COUNT);
            Vector3 startPos = startPositions[idx];

            // CommentMoverをアタッチ（またはPrefabから生成）
            CommentMover mover = tmp.GetComponent<CommentMover>();
            if (mover == null)
            {
                mover = tmp.gameObject.AddComponent<CommentMover>();
            }

            // 初期化（速度・加速度・フェードタイミングは定数で管理）
            mover.Initialize(
                tmp,
                startPos,
                CENTER,
                INIT_SPEED,
                INIT_ACCEL,
                INIT_FADE_START,
                INIT_FADE_DURATION,
                (comment) => commentPool.ReturnComment(comment) // コールバックでプールに戻す
            );
        }
    }
}
