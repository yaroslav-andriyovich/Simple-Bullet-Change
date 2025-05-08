using UnityEngine;

namespace Code.Bullets.Configs
{
    [CreateAssetMenu(fileName = "ExplodingBulletConfig", menuName = "BulletConfigs/ExplodingBulletConfig", order = 1)]
    public class ExplodingBulletConfig : BulletConfigBase
    {
        [field: SerializeField, Min(0f)] public float ExplosionRadius { get; private set; } = 10f;
    }
}