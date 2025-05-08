using System;
using Code.Bullets.Base;
using UnityEngine;

namespace Code.Weapons.Base
{
    public abstract class Weapon : MonoBehaviour
    {
        [field: SerializeField, Min(0f)] public float DelayBetweenBullets { get; private set; }
        [field: SerializeField] public Transform BulletReleasePoint { get; private set; }
        [field: SerializeField, Min(0f)] public uint ClipSize { get; private set; }
        [field: SerializeField, Min(0f)] public float ReloadingDuration { get; private set; }

        public event Action OnFire;
        public event Action OnReloading;

        private bool isReloading => Time.time - _lastReloadingTimeCode < ReloadingDuration;

        private Bullet _bulletsType;
        private uint _ammoClip;
        private float _lastShotTime;
        private float _lastReloadingTimeCode;

        public void ChangeBulletType<T>(T bulletPrefab) where T : Bullet
        {
            _bulletsType = bulletPrefab;
            Reload();
        }

        public void Reload()
        {
            if (_bulletsType == null)
            {
                Debug.LogWarning("You need change bullets type!");
                return;
            }
            
            if (isReloading)
                return;

            _lastReloadingTimeCode = Time.time;
            
            OnReloading?.Invoke();
            _ammoClip = ClipSize;
        }

        public void Fire()
        {
            if (_ammoClip == 0)
            {
                Reload();
                return;
            }
            
            if (isReloading)
                return;
            
            if (Time.time - _lastShotTime > DelayBetweenBullets)
            {
                Bullet bullet = Instantiate(_bulletsType, BulletReleasePoint.position, Quaternion.identity);
                bullet.Shoot();

                --_ammoClip;
                
                _lastShotTime = Time.time;
                
                OnFire?.Invoke();
            }
        }
    }
}