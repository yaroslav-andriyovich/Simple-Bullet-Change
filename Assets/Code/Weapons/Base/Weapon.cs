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
        public event Action OnReloaded;

        public uint CurrentAmmoClip => _currentAmmoClip;

        private bool isReloading => Time.time - _lastReloadingTimeCode < ReloadingDuration;

        private Bullet _bulletType;
        private uint _currentAmmoClip;
        private float _lastShotTime;
        private float _lastReloadingTimeCode;

        public void ChangeBulletType(Bullet bulletPrefab)
        {
            _bulletType = bulletPrefab;
            Reload();
        }

        public void Reload()
        {
            if (_bulletType == null)
            {
                Debug.LogWarning("You need change bullet type!");
                return;
            }
            
            if (isReloading)
                return;

            UpdateReloadingTime();
            OnReloading?.Invoke();
            FillAmmoClip();
            OnReloaded?.Invoke();
        }

        public void Fire()
        {
            if (_currentAmmoClip == 0)
            {
                Reload();
                return;
            }
            
            if (isReloading)
                return;

            if (Time.time - _lastShotTime < DelayBetweenBullets)
                return;
            
            CreateBullet();
            DecreaseCurrentAmmoClip();
            UpdateLastShotTime();
            OnFire?.Invoke();
        }

        private void UpdateReloadingTime() => 
            _lastReloadingTimeCode = Time.time;

        private void FillAmmoClip() => 
            _currentAmmoClip = ClipSize;

        private void CreateBullet()
        {
            Bullet bullet = Instantiate(_bulletType, BulletReleasePoint.position, Quaternion.identity);
            bullet.Shoot();
        }

        private void DecreaseCurrentAmmoClip() => 
            --_currentAmmoClip;

        private void UpdateLastShotTime() => 
            _lastShotTime = Time.time;
    }
}