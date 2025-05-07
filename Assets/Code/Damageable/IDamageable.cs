using UnityEngine;

namespace Code.Damageable
{
    public interface IDamageable
    {
        Vector3 CenterPosition { get; }
        void TakeDamage();
    }
}