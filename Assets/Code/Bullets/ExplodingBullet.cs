using Code.Bullets.Base;
using Code.Damageable;
using UnityEngine;

namespace Code.Bullets
{
    public class ExplodingBullet : Bullet
    {
        [SerializeField, Min(0f)] private float _explosionRadius = 5f;
        
        protected override void TryTakeDamage(Collision collision)
        {
            Vector3 contactPoint = collision.contacts[0].point;
            Collider[] colliders = Physics.OverlapSphere(contactPoint, _explosionRadius);
            
            foreach (Collider col in colliders)
            {
                if (col.gameObject.TryGetComponent(out IDamageable damageable))
                    damageable.TakeDamage();
            }
        }
        
        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, _explosionRadius);
        }
    }
}