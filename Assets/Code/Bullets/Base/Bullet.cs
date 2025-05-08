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

        protected virtual void Start() => 
            DestroyAfterTime();

        protected virtual void OnCollisionEnter(Collision collision)
        {
            TakeDamage(collision);
            Detonate(collision.contacts[0].point);
            Destroy(gameObject);
        }

        public void Shoot() => 
            rigidbody.velocity = speed * transform.forward;

        public void Detonate(Vector3 point)
        {
            if (detonationParticlePrefab != null)
            {
                ParticleSystem explosionParticle = Instantiate(detonationParticlePrefab, point, Quaternion.identity);
                explosionParticle.Play();
            }
        }

        protected virtual void TakeDamage(Collision collision)
        {
            if (collision.gameObject.TryGetComponent(out IDamageable damageable))
                damageable.TakeDamage();
        }

        private void DestroyAfterTime() => 
            Destroy(gameObject, lifetime);
    }
}