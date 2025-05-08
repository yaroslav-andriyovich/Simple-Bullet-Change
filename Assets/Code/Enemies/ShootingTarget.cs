using Code.Enemies.Base;
using DG.Tweening;
using UnityEngine;

namespace Code.Enemies
{
    [RequireComponent(typeof(AudioSource))]
    public class ShootingTarget : Enemy
    {
        [Header("Materials")]
        [SerializeField] private MeshRenderer _meshRenderer;
        [SerializeField] private Color _defaultColor = Color.white;
        [SerializeField] private Color _hitColor = Color.red;
        
        [Header("Animation")]
        [SerializeField, Min(0f)] private float _animationTime = 0.25f;
        [SerializeField] private float _animationAngle = -10f;
        [SerializeField] private Ease _animationEase = Ease.Linear;
        
        [Header("Sound")]
        [SerializeField, Range(0f, 2f)] private float _pitchMin = 0.5f;
        [SerializeField, Range(0f, 2f)] private float _pitchMax = 1.5f;
        
        [Header("Hit Collider")]
        [SerializeField] private Vector3 _offsetFromBottom = new Vector3(0f,  2.5f, 0f);

        public override Vector3 CenterPosition => transform.position + _offsetFromBottom;

        private Material _material;
        private AudioSource _audio;
        private Sequence _sequence;

        private void Awake()
        {
            _material = _meshRenderer.material;
            _audio = GetComponent<AudioSource>();
        }

        public override void TakeDamage()
        {
            base.TakeDamage();
            PlayHitAnimation();
            PlayTiltAnimation();
            PlaySound();
        }

        private void PlayHitAnimation()
        {
            _material.DORewind();
            _material.DOColor(_hitColor, _animationTime)
                .OnComplete(AnimateToNormalColor);
        }

        private void PlayTiltAnimation()
        {
            if (_sequence != null)
                _sequence.Rewind();
            
            float partTime = _animationTime / 2;
            Quaternion initialRotation = transform.rotation;
            
            _sequence = DOTween.Sequence();

            _sequence.Append(transform.DORotateQuaternion(
                    initialRotation * Quaternion.Euler(_animationAngle, 0f, 0f),
                    partTime)
                .SetEase(_animationEase));

            _sequence.Append(transform.DORotateQuaternion(
                    initialRotation,
                    partTime)
                .SetEase(_animationEase));

            _sequence.Play();
        }

        private void AnimateToNormalColor() => 
            _material.DOColor(_defaultColor, _animationTime);

        private void PlaySound()
        {
            _audio.pitch = Random.Range(_pitchMin, _pitchMax);
            _audio.Play();
        }
    }
}