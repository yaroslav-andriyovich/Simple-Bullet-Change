using UnityEngine;

namespace Code.Bullets.Configs
{
    [CreateAssetMenu(fileName = "BouncingBulletConfig", menuName = "BulletConfigs/BouncingBulletConfig", order = 1)]
    public class BouncingBulletConfig : BulletConfigBase
    {
        [field: SerializeField, Min(0)] public int MaxBounces { get; private set; } = 3;
        [field: SerializeField, Min(0f)] public float BouncingRadius { get; private set; } = 8f;
    }
}