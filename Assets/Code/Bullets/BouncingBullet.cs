using System.Collections.Generic;
using Code.Bullets.Base;
using Code.Damageable;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Code.Bullets
{
    public class BouncingBullet : Bullet
    {
        [SerializeField, Min(0)] private int _maxBounces = 3;
        [SerializeField, Min(0f)] private float _bouncingRadius = 5f;

        private Collider[] _collidersInSphere;
        private List<IDamageable> _nearestTargets;
        private IDamageable _currentTarget;
        private int _bounces;
        private Collider _lastHittedCollider;

        protected override void Awake()
        {
            base.Awake();
            _collidersInSphere = new Collider[_maxBounces];
            _nearestTargets = new List<IDamageable>(_maxBounces);
        }

        protected override void OnCollisionEnter(Collision collision) => 
            TryTakeDamage(collision);

        protected override void TryTakeDamage(Collision collision)
        {
            if (_lastHittedCollider == collision.collider)
                return;
            
            if (_bounces >= _maxBounces || !IsDamageable(collision, out _currentTarget))
            {
                Cleanup();
                return;
            }

            SetPassabilityThroughCurrentTarget(collision.collider);

            Vector3 contactPoint = collision.contacts[0].point;
            
            SetPosition(contactPoint);
            TakeDamage(_currentTarget, contactPoint);
            
            if (CacheNearestTargets(contactPoint) == 0)
            {
                Cleanup();
                return;
            }

            SetNewTarget();

            if (BounceLimitHasBeenReached())
                Cleanup();
            else
                _nearestTargets.Clear();
        }

        private void SetPassabilityThroughCurrentTarget(Collider targetCollider)
        {
            if (_lastHittedCollider != null)
            {
                Physics.IgnoreCollision(collider, _lastHittedCollider, false);
                _lastHittedCollider = null;
            }

            _lastHittedCollider = targetCollider;
            Physics.IgnoreCollision(collider, _lastHittedCollider, true);
        }

        private bool IsDamageable(Collision collision, out IDamageable currentDamageable) => 
            collision.gameObject.TryGetComponent(out currentDamageable);

        private void SetPosition(Vector3 contactPoint) => 
            transform.position = contactPoint;

        private void TakeDamage(IDamageable damageable, Vector3 detonationPoint)
        {
            damageable.TakeDamage();
            Detonate(detonationPoint);
        }

        private int CacheNearestTargets(Vector3 contactPoint)
        {
            CacheNearestColliders(contactPoint);
            
            foreach (Collider col in _collidersInSphere)
            {
                if (col != null
                    && col.TryGetComponent(out IDamageable damageable)
                    && damageable != _currentTarget)
                {
                    _nearestTargets.Add(damageable);
                }
            }

            return _nearestTargets.Count;
        }

        private void CacheNearestColliders(Vector3 contactPoint) => 
            Physics.OverlapSphereNonAlloc(contactPoint, _bouncingRadius, _collidersInSphere);

        private void SetNewTarget()
        {
            IDamageable damageable = GetRandomNearestDamageable();
            Vector3 nextDamageablePosition = damageable.CenterPosition;
            Vector3 direction = (nextDamageablePosition - transform.position).normalized;

            rigidbody.velocity = direction * speed;
            rigidbody.angularVelocity = Vector3.zero;
            transform.rotation = Quaternion.LookRotation(direction);
        }

        private IDamageable GetRandomNearestDamageable() => 
            _nearestTargets[Random.Range(0, _nearestTargets.Count)];

        private bool BounceLimitHasBeenReached() => 
            ++_bounces >= _maxBounces;

        private void Cleanup()
        {
            _collidersInSphere = null;
            _nearestTargets.Clear();
            _currentTarget = null;
            _lastHittedCollider = null;
            Destroy(gameObject);
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(transform.position, _bouncingRadius);
        }
    }
}