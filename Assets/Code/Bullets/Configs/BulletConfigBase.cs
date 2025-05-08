using UnityEngine;

namespace Code.Bullets.Configs
{
    [CreateAssetMenu(fileName = "BulletConfig", menuName = "BulletConfigs/BulletConfig", order = 1)]
    public class BulletConfigBase : ScriptableObject
    {
        [field: SerializeField, Min(0f)] public float Speed { get; private set; } = 20f;
        [field: SerializeField, Min(0f)] public float LifeTime { get; private set; } = 5f;
        [field: SerializeField] public ParticleSystem DetonationParticlePrefab{ get; private set; }
    }
}