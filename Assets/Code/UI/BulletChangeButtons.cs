using Code.Bullets;
using Code.Weapons.Base;
using UnityEngine;
using UnityEngine.UI;

namespace Code.UI
{
    public class BulletChangeButtons : MonoBehaviour
    {
        [Header("Bullets & Buttons")]
        [SerializeField] private Button _simpleBulletButton;
        [SerializeField] private SimpleBullet _simpleBulletPrefab;
        [Space]
        [SerializeField] private Button _explodingBulletButton;
        [SerializeField] private ExplodingBullet _explodingBulletPrefab;
        [Space]
        [SerializeField] private Button _bouncingBulletButton;
        [SerializeField] private BouncingBullet _bouncingBulletPrefab;
        
        [Header("Weapon")]
        [SerializeField] private Weapon _weapon;

        private void Awake()
        {
            _simpleBulletButton.onClick.AddListener(ChangeSimpleBullet);
            _explodingBulletButton.onClick.AddListener(ChangeExplodingBullet);
            _bouncingBulletButton.onClick.AddListener(ChangeBouncingBullet);
        }

        private void OnDestroy()
        {
            _simpleBulletButton.onClick.RemoveListener(ChangeSimpleBullet);
            _explodingBulletButton.onClick.RemoveListener(ChangeExplodingBullet);
            _bouncingBulletButton.onClick.RemoveListener(ChangeBouncingBullet);
        }

        private void ChangeSimpleBullet() => 
            _weapon.ChangeBulletType(_simpleBulletPrefab);
        
        private void ChangeExplodingBullet() => 
            _weapon.ChangeBulletType(_explodingBulletPrefab);
        
        private void ChangeBouncingBullet() => 
            _weapon.ChangeBulletType(_bouncingBulletPrefab);
    }
}