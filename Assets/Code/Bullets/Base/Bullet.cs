using Code.Damageable;
using UnityEngine;

namespace Code.Bullets.Base
{
    [RequireComponent(typeof(Rigidbody))]
    public abstract class Bullet : MonoBehaviour
    {
        [SerializeField, Min(0f)] protected float speed = 5f;
        [SerializeField, Min(0f)] protected float lifetime = 5f;
        [SerializeField] protected ParticleSystem detonationParticlePrefab;
        [SerializeField] protected new Collider collider;

        protected new Rigidbody rigidbody;
        
        protected virtual void Awake() => 
            rigidbody = GetComponent<Rigidbody>();

        private void Start() => 
            DestroyAfterTime();

        protected virtual void OnCollisionEnter(Collision collision)
        {
            TryTakeDamage(collision);
            Detonate(collision.contacts[0].point);
            Destroy(gameObject);
        }

        public virtual void Shoot() => 
            rigidbody.velocity = speed * transform.forward;

        public virtual void Detonate(Vector3 point)
        {
            if (detonationParticlePrefab != null)
            {
                ParticleSystem explosionParticle = Instantiate(detonationParticlePrefab, point, Quaternion.identity);
                explosionParticle.Play();
            }
        }

        protected virtual void TryTakeDamage(Collision collision)
        {
            if (collision.gameObject.TryGetComponent(out IDamageable damageable))
                damageable.TakeDamage();
        }

        private void DestroyAfterTime() => 
            Destroy(gameObject, lifetime);
    }
}