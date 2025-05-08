using System.Collections.Generic;
using Code.Bullets.Base;
using Code.Bullets.Configs;
using Code.Damageable;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Code.Bullets
{
    public class BouncingBullet : Bullet
    {
        private BouncingBulletConfig Config => configBase as BouncingBulletConfig;

        private Collider[] _collidersInSphere;
        private List<IDamageable> _nearestTargets;
        private IDamageable _currentTarget;
        private int _currentBounces;
        private Collider _lastColliderThatGotHit;

        protected override void Awake()
        {
            base.Awake();
            _collidersInSphere = new Collider[Config.MaxBounces];
            _nearestTargets = new List<IDamageable>(Config.MaxBounces);
        }

        protected override void OnCollisionEnter(Collision collision) => 
            TakeDamage(collision);

        protected override void TakeDamage(Collision collision)
        {
            if (_lastColliderThatGotHit == collision.collider)
                return;
            
            if (_currentBounces >= Config.MaxBounces || !IsDamageable(collision, out _currentTarget))
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
            if (_lastColliderThatGotHit != null)
            {
                Physics.IgnoreCollision(collider, _lastColliderThatGotHit, false);
                _lastColliderThatGotHit = null;
            }

            _lastColliderThatGotHit = targetCollider;
            Physics.IgnoreCollision(collider, _lastColliderThatGotHit, true);
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
            Physics.OverlapSphereNonAlloc(contactPoint, Config.BouncingRadius, _collidersInSphere);

        private void SetNewTarget()
        {
            IDamageable target = GetRandomNearestTarget();
            Vector3 nextTargetPosition = target.CenterPosition;
            Vector3 direction = (nextTargetPosition - transform.position).normalized;

            rigidbody.velocity = direction * Config.Speed;
            rigidbody.angularVelocity = Vector3.zero;
            transform.rotation = Quaternion.LookRotation(direction);
        }

        private IDamageable GetRandomNearestTarget() => 
            _nearestTargets[Random.Range(0, _nearestTargets.Count)];

        private bool BounceLimitHasBeenReached() => 
            ++_currentBounces >= Config.MaxBounces;

        private void Cleanup()
        {
            _collidersInSphere = null;
            _nearestTargets.Clear();
            _currentTarget = null;
            _lastColliderThatGotHit = null;
            Destroy(gameObject);
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(transform.position, Config.BouncingRadius);
        }
    }
}