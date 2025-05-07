using Code.Damageable;
using UnityEngine;

namespace Code.Enemies.Base
{
    public abstract class Enemy : MonoBehaviour, IDamageable
    {
        public virtual Vector3 CenterPosition => transform.position;
        
        public virtual void TakeDamage()
        {
        }
    }
}