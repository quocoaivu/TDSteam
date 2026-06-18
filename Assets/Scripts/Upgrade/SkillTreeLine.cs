using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace Upgrade
{
    public enum SkillTreeLineState
    {
        Locked,   // prereq chưa mở
        Ready,    // prereq đã mở (đạt điều kiện) nhưng node đích chưa mở
        Unlocked, // node đích đã mở
    }

    // Đường nối prereq -> node trong skill tree. 2 việc:
    // 1) Hình học: kéo dãn/xoay 1 Image mỏng nối 2 node, bám theo khi kéo node trong editor.
    // 2) Trạng thái: panel gọi ApplyState để đổi màu (đạt prereq) / đổ đầy (unlock) bằng DOTween.
    [ExecuteAlways]
    [RequireComponent(typeof(RectTransform))]
    public class SkillTreeLine : MonoBehaviour
    {
        public int FromNodeID
        {
            get { return fromNodeID; }
        }

        public int ToNodeID
        {
            get { return toNodeID; }
        }

        // Called right after the panel instantiates this from the template. Binds the line to the
        // two node RectTransforms (prereq -> node) and lays out its geometry.
        public void Setup(RectTransform from, RectTransform to, int fromId, int toId)
        {
            fromNode = from;
            toNode = to;
            fromNodeID = fromId;
            toNodeID = toId;
            Refresh();
        }

        // Panel gọi mỗi lần refresh. animate=false cho lần mở panel (set tức thì),
        // animate=true khi người chơi vừa unlock (chạy tween).
        public void ApplyState(SkillTreeLineState state, bool animate)
        {
            if (hasState && state == currentState)
            {
                return; // chưa đổi trạng thái -> bỏ qua, tránh chạy tween thừa
            }
            hasState = true;
            currentState = state;

            switch (state)
            {
                case SkillTreeLineState.Locked:
                    StopGlow();
                    SetVisual(lockedColor, 0f, animate, null);
                    break;
                case SkillTreeLineState.Ready:
                    SetVisual(readyColor, 0f, animate, null);
                    StartGlow(); // đốm sáng chạy dọc báo "đủ điều kiện, sẵn sàng mở"
                    break;
                case SkillTreeLineState.Unlocked:
                    StopGlow();
                    SetVisual(unlockedColor, 1f, animate, Flash); // đổ đầy xong thì flash
                    break;
            }
        }

        private void SetVisual(Color baseColor, float fillTarget, bool animate, TweenCallback onFillComplete)
        {
            if (baseImage != null)
            {
                baseImage.DOKill();
                if (animate)
                {
                    baseImage.DOColor(baseColor, animDuration);
                }
                else
                {
                    baseImage.color = baseColor;
                }
            }
            if (fillImage != null)
            {
                fillImage.DOKill();
                if (animate)
                {
                    Tweener t = fillImage.DOFillAmount(fillTarget, animDuration);
                    if (onFillComplete != null)
                    {
                        t.OnComplete(onFillComplete);
                    }
                }
                else
                {
                    fillImage.fillAmount = fillTarget;
                }
            }
        }

        // Chớp sáng nhanh ngay khi đường vừa đổ đầy xong.
        private void Flash()
        {
            if (fillImage == null)
            {
                return;
            }
            fillImage.DOColor(Color.white, 0.08f).SetLoops(2, LoopType.Yoyo);
        }

        // Đốm sáng chạy lặp từ phía prereq -> node dọc theo đường.
        private void StartGlow()
        {
            if (glow == null)
            {
                return;
            }
            Refresh(); // bảo đảm độ dài đường mới nhất
            float half = ((RectTransform)transform).sizeDelta.x * 0.5f;
            glow.gameObject.SetActive(true);
            glow.anchoredPosition = new Vector2(-half, 0f);
            glowTween = glow.DOAnchorPosX(half, glowDuration)
                .SetEase(Ease.InOutSine)
                .SetLoops(-1, LoopType.Restart);
        }

        private void StopGlow()
        {
            if (glowTween != null)
            {
                glowTween.Kill();
                glowTween = null;
            }
            if (glow != null)
            {
                glow.gameObject.SetActive(false);
            }
        }

        private void OnEnable()
        {
            Refresh();
        }

        private void OnDisable()
        {
            StopGlow();       // dừng tween lặp khi panel đóng để tránh rò rỉ
            hasState = false; // mở lại panel sẽ apply trạng thái từ đầu (khởi động lại glow nếu Ready)
        }

        // Trong editor: theo dõi node di chuyển để vẽ lại. Bỏ qua lúc chạy game (node đứng yên).
        private void Update()
        {
            if (Application.isPlaying)
            {
                return;
            }
            if (fromNode == null || toNode == null)
            {
                return;
            }
            if (fromNode.anchoredPosition == lastFrom && toNode.anchoredPosition == lastTo)
            {
                return; // chưa đổi vị trí -> khỏi vẽ lại, tránh dirty scene liên tục
            }
            Refresh();
        }

        // Cập nhật hình học đường theo vị trí 2 node.
        public void Refresh()
        {
            if (fromNode == null || toNode == null)
            {
                return;
            }
            Vector2 from = fromNode.anchoredPosition;
            Vector2 to = toNode.anchoredPosition;
            Vector2 dir = to - from;

            RectTransform rect = (RectTransform)transform;
            rect.pivot = new Vector2(0.5f, 0.5f);            // xoay quanh tâm đường
            rect.anchoredPosition = from + dir * 0.5f;       // đặt ở trung điểm
            rect.sizeDelta = new Vector2(dir.magnitude, thickness); // dài = khoảng cách 2 node
            rect.localEulerAngles = new Vector3(0f, 0f, Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg);

            lastFrom = from;
            lastTo = to;
        }

        [Header("Node tham chiếu (prereq -> node)")]
        [SerializeField]
        private RectTransform fromNode;

        [SerializeField]
        private RectTransform toNode;

        [SerializeField]
        private int fromNodeID; // prereq

        [SerializeField]
        private int toNodeID;   // node phụ thuộc

        [Header("Hình ảnh")]
        [SerializeField]
        private Image baseImage; // đường nền

        [SerializeField]
        private Image fillImage; // phần đổ đầy (Image type = Filled)

        [SerializeField]
        private RectTransform glow; // đốm sáng chạy dọc khi Ready

        [SerializeField]
        private float thickness = 6f;

        [Header("Màu theo trạng thái")]
        [SerializeField]
        private Color lockedColor = new Color(0.4f, 0.45f, 0.55f, 1f);

        [SerializeField]
        private Color readyColor = new Color(0.95f, 0.82f, 0.3f, 1f);

        [SerializeField]
        private Color unlockedColor = new Color(0.5f, 0.9f, 0.4f, 1f);

        [SerializeField]
        private float animDuration = 0.3f;

        [SerializeField]
        private float glowDuration = 0.9f; // thời gian đốm sáng chạy hết 1 lượt

        private bool hasState;
        private SkillTreeLineState currentState;
        private Vector2 lastFrom;
        private Vector2 lastTo;
        private Tween glowTween;
    }
}
