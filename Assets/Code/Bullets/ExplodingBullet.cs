using Code.Bullets.Base;
using Code.Bullets.Configs;
using Code.Damageable;
using UnityEngine;

namespace Code.Bullets
{
    public class ExplodingBullet : Bullet
    {
        private ExplodingBulletConfig Config => configBase as ExplodingBulletConfig;
        
        protected override void TakeDamage(Collision collision)
        {
            Vector3 contactPoint = collision.contacts[0].point;
            Collider[] colliders = Physics.OverlapSphere(contactPoint, Config.ExplosionRadius);
            
            foreach (Collider col in colliders)
            {
                if (col.gameObject.TryGetComponent(out IDamageable damageable))
                    damageable.TakeDamage();
            }
        }
        
        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, Config.ExplosionRadius);
        }
    }
}