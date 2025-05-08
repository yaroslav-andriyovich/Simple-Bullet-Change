using DG.Tweening;
using UnityEngine;

namespace Code.Hands
{
    public class IdleAnimator : MonoBehaviour
    {
        [SerializeField] private float _moveDistance = 1f;
        [SerializeField] private float _duration = 1f;
        [SerializeField] private Ease _ease;

        private void Start()
        {
            Vector3 upPosition = transform.position + Vector3.up * _moveDistance;

            transform.DOMove(upPosition, _duration)
                .SetEase(_ease)
                .SetLoops(-1, LoopType.Yoyo)
                .SetLink(gameObject);
        }
    }
}