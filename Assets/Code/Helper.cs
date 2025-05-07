using Code.Bullets.Base;
using Code.Weapons.Base;
using UnityEngine;

namespace Code
{
    public class Helper : MonoBehaviour
    {
        [SerializeField] private Weapon _weapon;
        [SerializeField] private Bullet _bulletPrefab;

        private void Awake()
        {
            _weapon.ChangeBulletType(_bulletPrefab);
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Space))
                _weapon.Fire();
            
            if (Input.GetKeyDown(KeyCode.R))
                _weapon.Reload();
        }

        private void OnValidate()
        {
            _weapon.ChangeBulletType(_bulletPrefab);
        }
    }
}